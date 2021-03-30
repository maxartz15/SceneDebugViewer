using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TAO.SceneDebugViewer.Editor
{
	public class SceneDebugViewerWindow : EditorWindow
	{
		static List<ReplacementShaderSetupScriptableObject> options = new List<ReplacementShaderSetupScriptableObject>();

		[MenuItem("Window/SceneDebugViewer")]
		static void Init()
		{
			options.Clear();

			string[] guids = AssetDatabase.FindAssets("t:ReplacementShaderSetupScriptableObject", null);
			foreach (string guid in guids)
			{
				options.Add((ReplacementShaderSetupScriptableObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(ReplacementShaderSetupScriptableObject)));
			}

			SceneDebugViewerWindow window = (SceneDebugViewerWindow)EditorWindow.GetWindow(typeof(SceneDebugViewerWindow));
			window.Show();
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Reset"))
			{
				foreach (SceneView s in SceneView.sceneViews)
				{
					s.SetSceneViewShaderReplace(null, null);
					s.Repaint();
				}
			}

			foreach (var o in options)
			{
				if (GUILayout.Button(o.content))
				{
					o.Replace();
				}
			}
		}
	}
}