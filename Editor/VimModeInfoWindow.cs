using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class VimModeInfoWindow : EditorWindow {
	private static List<string> SceneList = new List<string>();
	private static VimModeInfoWindow scene = null;
	// シーン一覧ウィンドウを表示する
	[UnityEditor.MenuItem("ShortCutCommand/VimSceneView &o")]
	public static void VimSceneView() {
		UnityEngine.Debug.Log("VimSceneView");
		if (PlayerPrefs.HasKey("vim_unite") == false) {
			PlayerPrefs.SetString("vim_unite", "");
		}

		if (scene == null) {
			scene = EditorWindow.GetWindow<VimModeInfoWindow>();
			scene.Show();
		} else {
			scene.Close();
			scene = null;
		}
	}

	Vector2 scrollPos = Vector2.zero;
	int selected = 0;
	void OnGUI() {
		//List<string> list = GetSaveFileHistory();
		//if (list.Count > 0) {
		//	int height = list.Count * 30;
		//	scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		//	for (int i = 0; i < list.Count; i++) {
		//		//ボタンを表示
		//		if (GUILayout.Button(list[i])) {
		//			if (list[i].Contains(".cs")) {
		//				string path = list[i].Replace("/", "\\");
		//				System.Diagnostics.Process.Start("MonoDevelop.exe", path);
		//				AddSaveFileHistory(list[i]);

		//			} else if (list[i].Contains(".unity")) {
		//				EditorSceneManager.SaveOpenScenes();
		//				EditorSceneManager.OpenScene(list[i], OpenSceneMode.Single);
		//				AddSaveFileHistory(list[i]);
		//				break;
		//			}
		//		}
		//	}
		//}
		//EditorGUILayout.EndScrollView();
		
		List<string> list = GetSaveFileHistory();
		if (list.Count > 0) {
			int height = list.Count * 30;
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			for (int i = 0; i < list.Count; i++) {
				using (new EditorGUILayout.HorizontalScope ()) {
					//ボタンを表示
					if (list[i].Contains(".cs")) {
						if (GUILayout.Button(list[i])) {
							string path = list[i].Replace("/", "\\");
							System.Diagnostics.Process.Start("MonoDevelop.exe", path);
							AddSaveFileHistory(list[i]);
						}
						if (GUILayout.Button(list[i])) {
							string path = list[i].Replace("/", "\\");
							// 右側にボタンを追加し、VIMで開ける用のボタンを作る
							System.Diagnostics.Process.Start("C:\\Yamashita\\vim80-kaoriya-win64\\gvim.exe", "--remote-tab-silent " + path);
							AddSaveFileHistory(list[i]);
						}
					} else if (list[i].Contains(".unity")) {
						if (GUILayout.Button(list[i])) {
							EditorSceneManager.SaveOpenScenes();
							EditorSceneManager.OpenScene(list[i], OpenSceneMode.Single);
							AddSaveFileHistory(list[i]);
						}
					}
				}
			}
		}
		EditorGUILayout.EndScrollView();
	}





	private static int SortString(string left, string right) {
		return string.Compare(left, right);
	}

	public static List<string> GetSaveFileHistory() {
		string saveString = PlayerPrefs.GetString("vim_unite");
		string[] split = saveString.Split("\n"[0]);

		return new List<string>(split);
	}

	public static void AddSaveFileHistory(string path) {
		List<string> list = VimModeInfoWindow.GetSaveFileHistory();
		if (list.Contains(path)) {
			list.Remove(path);
			list.Insert(0, path);
		} else {
			if (list.Count >= 30) {
				list.RemoveAt(list.Count);
			}
			list.Insert(0, path);
		}

		string saveString = "";
		for (int i = 0; i < list.Count; i++) {
			if (i != 0) {
				saveString += "\n";
			}
			saveString += list[i];
		}
		PlayerPrefs.SetString("vim_unite", saveString);
	}
}
