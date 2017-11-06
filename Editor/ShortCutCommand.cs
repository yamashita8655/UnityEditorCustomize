using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class ShortCutCommand {
	public static bool IsCommandMode = false;
	public static GameObject SaveObject = null;

	// 検証
	[UnityEditor.MenuItem("ShortCutCommand/Test % ")]
	public static void Test() {
		var asm = Assembly.GetAssembly(typeof(EditorWindow));
		var T = asm.GetType("UnityEditor.PreferencesWindow");
		var M = T.GetMethod("ShowPreferencesWindow", BindingFlags.NonPublic | BindingFlags.Static);
		M.Invoke(null, null);

		//var asm2 = Assembly.GetAssembly(M.GetType());
		//int i = 0;
//		var T2 = asm.GetType("UnityEditor.PreferencesWindow");
//		var M2 = T.GetMethod("ShowPreferencesWindow", BindingFlags.NonPublic | BindingFlags.Static);

//		var T2 = M.ty
/*		string name = "";
//		foreach(MethodInfo mi in T.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)){
		foreach(MethodInfo mi in T.GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)){
			//if(mi.IsGenericMethod && mi.IsGenericMethodDefinition && mi.ContainsGenericParameters){
				name += mi.Name+"\n";
//				if(mi.Name == methodName && mi.GetParameters().Length == paramTypes.Length){
//					MethodInfo genericMi = mi.MakeGenericMethod(new Type[]{ typeof(DateTime) } );
//					Debug.Log(genericMi.na);
			//}
		}
		Debug.Log(name);*/

		//string test = EditorPrefs.GetString("Background");
		//Debug.Log(test);

		//EditorPrefs.SetString("Background", "Background;1;1;0;1");
	}


	//// VIMモードON
	//[UnityEditor.MenuItem("ShortCutCommand/VimModeOn %i")]
	//public static void VimModeOn() {
	//	IsCommandMode = true;
	//}
	//
	//// VIMモードOFF
	//[UnityEditor.MenuItem("ShortCutCommand/VimModeOff %[")]
	//public static void VimModeOff() {
	//	IsCommandMode = false;
	//}
	
	// 実行/停止を行う
	//[UnityEditor.MenuItem("ShortCutCommand/VimRun % ")]
	[UnityEditor.MenuItem("ShortCutCommand/VimRun & ")]
	public static void VimRun() {
		if (EditorApplication.isPlaying) {
			EditorApplication.isPlaying = false;
		} else {
			EditorApplication.isPlaying = true;
		}
	}

	// GameObjectのpos/rotate/scaleを初期化する
	[UnityEditor.MenuItem("ShortCutCommand/VimParameterClear &u")]
	public static void VimParameterClear() {
		Debug.Log("Para Clear");
		GameObject obj = Selection.activeGameObject;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// GameObjectの内容を保存する
	[UnityEditor.MenuItem("ShortCutCommand/VimParameterSave &y")]
	public static void VimParameterSave() {
		Debug.Log("Para Save");
		SaveObject = Selection.activeGameObject;
	}

	// GameObjectのpos/rotate/scaleを上書きする
	[UnityEditor.MenuItem("ShortCutCommand/VimParameterPaste &v")]
	public static void VimParameterPaste() {
		if (SaveObject != null) {
			Debug.Log("Para Paste");
			GameObject obj = Selection.activeGameObject;
			obj.transform.localPosition = SaveObject.transform.localPosition;
			obj.transform.localScale= SaveObject.transform.localScale;
			obj.transform.localRotation = SaveObject.transform.localRotation;
		}
	}

	// GameObjectを移動させる
	[UnityEditor.MenuItem("ShortCutCommand/VimMoveObject &p")]
	public static void VimMoveObject() {
		if (SaveObject != null) {
			Debug.Log("Para Move");
			GameObject obj = Selection.activeGameObject;
			if (obj != null) {
				SaveObject.transform.SetParent(obj.transform);
			} else {
				SaveObject.transform.SetParent(null);
			}
		}
	}

	// AddComponentをする
	[UnityEditor.MenuItem("ShortCutCommand/VimAddComponent &a")]
	public static void VimAddComponent() {
		Debug.Log("AddComponent");
		EditorApplication.ExecuteMenuItem("Component/Add...");
	}

	// 開く
	// かつ、履歴に残す
	[UnityEditor.MenuItem("ShortCutCommand/VimOpen &[")]
	public static void VimOpen() {
		Debug.Log("VimOpen");
		if (PlayerPrefs.HasKey("vim_unite") == false) {
			PlayerPrefs.SetString("vim_unite", "");
		}

		List<string> list = VimModeInfoWindow.GetSaveFileHistory();

		foreach (var val in Selection.assetGUIDs) {
			var path = AssetDatabase.GUIDToAssetPath( val );
			if (path.Contains(".cs")) {
				path = path.Replace("/", "\\");
				System.Diagnostics.Process.Start("MonoDevelop.exe", path);
				//			System.Diagnostics.Process.Start("C:\\yamashita\\github\\GVIM\\vim73\\win32\\gvim.exe", "--remote-tab-silent " + path);
				VimModeInfoWindow.AddSaveFileHistory(path);
			} else if (path.Contains(".unity")) {
				EditorSceneManager.SaveOpenScenes();
				EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
				VimModeInfoWindow.AddSaveFileHistory(path);
			}

			Debug.Log(path);
//			Process.Start("MonoDevelop.exe", "C:\\yamashita\\github\\hakusura\\Assets\\Editor\\VimModeInfoWindow.cs");
		}
	}
}
