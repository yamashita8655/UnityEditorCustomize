using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System.Collections.Generic;

public class ReplaceMissingAnimationCurve  : EditorWindow
{
	Animator animator;

	List<AnimationClip> missingClips = new List<AnimationClip> ();
	List<string> missingPathList = new List<string> ();

	string oldPath, newPath;

	Vector2 scroll;

	[MenuItem ("Assets/ReplaceAnimatorMissingPath")]
	static void Init ()
	{
		var window = ReplaceMissingAnimationCurve.GetWindow<ReplaceMissingAnimationCurve> ();
		window.Show ();
	}

	void OnGUI ()
	{
		if (animator == null) {
			EditorGUILayout.HelpBox ("chose animator", MessageType.Info);
		}
		EditorGUI.BeginChangeCheck ();
		animator = EditorGUILayout.ObjectField ("animator", animator, typeof(Animator), true) as Animator;
		if (EditorGUI.EndChangeCheck ()) {
			FindPath ();

			if (missingPathList.Count != 0) {
				oldPath = missingPathList.First ();
			}
		}

		using( var scrollview = new EditorGUILayout.ScrollViewScope(scroll)){
			scroll = scrollview.scrollPosition;
			if (missingClips.Count != 0) {
				EditorGUILayout.HelpBox ("replace correct path", MessageType.Info);

				oldPath = EditorGUILayout.TextField ("old Path", oldPath);
				newPath = EditorGUILayout.TextField ("replace path", newPath);

				EditorGUILayout.HelpBox ("animator has missing path", MessageType.None);
				foreach (var path in missingPathList) {
					EditorGUILayout.SelectableLabel (path, GUILayout.Height (15));
				}

				EditorGUILayout.HelpBox ("missing clips", MessageType.None);
				foreach (var clip in missingClips) {
					EditorGUILayout.ObjectField (clip, typeof(AnimationClip), false);
				}

				if (GUILayout.Button ("Apply")) {
					ReplacePath (missingClips.ToArray (), oldPath, newPath);
					FindPath ();
				}
			} else if( animator != null) {
				EditorGUILayout.HelpBox ("(^ ^)b", MessageType.Info);
			}
		}
	}

	void FindPath ()
	{
		UnityEditor.Animations.AnimatorController controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
		var clips = controller.animationClips.Distinct ().ToList ();

		missingClips.Clear ();
		missingPathList.Clear ();
		int missingCount = 0;

		foreach (var clip in clips) {
			var bindings = AnimationUtility.GetCurveBindings (clip);

			foreach (var binding in bindings) {
				if (animator.transform.Find (binding.path) == null) {
					missingPathList.Add (binding.path);
					missingCount++;
				}
			}

			if (missingCount != 0)
				missingClips.Add (clip);
		}

		missingPathList = missingPathList.Distinct ().ToList ();
		missingClips = missingClips.Distinct ().ToList ();
	}

	static void ReplacePath (AnimationClip[] clips, string oldPath, string newPath)
	{
		Undo.RecordObjects (clips, "replace animationclip paths");
		foreach (var clip in clips) {
			var bindings = AnimationUtility.GetCurveBindings (clip);
			var removeBindings = bindings.Where (c => c.path.Contains (oldPath));

			foreach (var binding in removeBindings) {
				var curve = AnimationUtility.GetEditorCurve (clip, binding);
				var newBinding = binding;
				newBinding.path = newBinding.path.Replace (oldPath, newPath);
				AnimationUtility.SetEditorCurve (clip, binding, null);
				AnimationUtility.SetEditorCurve (clip, newBinding, curve);
			}
		}
	}
}