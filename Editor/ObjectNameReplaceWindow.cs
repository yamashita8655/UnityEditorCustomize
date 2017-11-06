using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ObjectReplaceWindow : EditorWindow {
	private static ObjectReplaceWindow scene = null;
	// シーン一覧ウィンドウを表示する
	[UnityEditor.MenuItem("ShortCutCommand/OpenObjectReplaceWindow &2")]
	public static void OpenObjectReplaceWindow() {
		UnityEngine.Debug.Log("OpenObjectReplaceWindow");

		if (scene == null) {
			scene = EditorWindow.GetWindow<ObjectReplaceWindow>();
			scene.Show();
		} else {
			scene.Close();
			scene = null;
		}

		Debug.Log(Selection.gameObjects);
	}

	string replaceString = "";
	string replaceNumberKeyString = "";
	
	string replaceSameParentString = "";
	string replaceSameParentNumberKeyString = "";
	void OnGUI() {
		using (new EditorGUILayout.HorizontalScope()) {
			replaceString = EditorGUILayout.TextField(replaceString);
			replaceNumberKeyString = EditorGUILayout.TextField(replaceNumberKeyString);
		}
		if (GUILayout.Button("Replace")) {
			if (string.IsNullOrEmpty(replaceString)) {
			} else {
				if (string.IsNullOrEmpty(replaceNumberKeyString)) {
					for (int i = 0; i < Selection.gameObjects.Length; i++) {
						Selection.gameObjects[i].name = replaceString;
					}
				} else {
					for (int i = 0; i < Selection.gameObjects.Length; i++) {
						Selection.gameObjects[i].name = string.Format("{0}{1}", replaceString, (int.Parse(replaceNumberKeyString) + i).ToString());
					}
				}
			}
		}
		
		using (new EditorGUILayout.HorizontalScope()) {
			replaceSameParentString = EditorGUILayout.TextField(replaceSameParentString);
			replaceSameParentNumberKeyString = EditorGUILayout.TextField(replaceSameParentNumberKeyString);
		}
		
		if (GUILayout.Button("Replace")) {
			if (string.IsNullOrEmpty(replaceSameParentString)) {
			} else {
				if (string.IsNullOrEmpty(replaceSameParentNumberKeyString)) {
					for (int i = 0; i < Selection.gameObjects.Length; i++) {
						Selection.gameObjects[i].name = replaceSameParentString;
					}
				} else {
					int minSibIndex = 0;
					for (int i = 0; i < Selection.gameObjects.Length; i++) {
						int sibling = Selection.gameObjects[i].transform.GetSiblingIndex();
						if (i == 0) {
							minSibIndex = sibling;
						}
						if (sibling <= minSibIndex) {
							minSibIndex = sibling;
						}
					}

					for (int i = 0; i < Selection.gameObjects.Length; i++) {
						int sibling = Selection.gameObjects[i].transform.GetSiblingIndex();
						Selection.gameObjects[i].name = string.Format("{0}{1}", replaceSameParentString, (int.Parse(replaceSameParentNumberKeyString) + sibling-minSibIndex).ToString());// 0から開始したいため
					}
				}
			}
		}
	}
}
