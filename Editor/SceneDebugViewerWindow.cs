using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TAO.SceneDebugViewer.Editor
{
	public class SceneDebugViewerWindow : EditorWindow
	{
		private static SceneDebugViewerWindow window = null;
		private static List<ReplacementShaderSetupScriptableObject> options = new List<ReplacementShaderSetupScriptableObject>();

		[MenuItem("Window/SceneDebugViewer")]
		static void Init()
		{
			Load();

			window = (SceneDebugViewerWindow)GetWindow(typeof(SceneDebugViewerWindow));
			window.titleContent = new GUIContent("SDV");
			window.maxSize = new Vector2(101, window.maxSize.y);
			window.minSize = new Vector2(101, window.minSize.y);
			window.Show();
		}

		private void OnGUI()
		{
			using (new GUILayout.VerticalScope())
			{
				if (GUILayout.Button("Reload"))
				{
					Load();
				}

				GUILayout.Space(6);

				if (GUILayout.Button("Default", GUILayout.Height(44)))
				{
					foreach (SceneView s in SceneView.sceneViews)
					{
						s.SetSceneViewShaderReplace(null, null);
						s.Repaint();
					}
				}

				GUILayout.Space(6);

				for (int i = 0; i < options.Count; i += 2)
				{
					GUILayout.BeginHorizontal();

					if (GUILayout.Button(options[i].content, GUILayout.Height(44)))
					{
						options[i].Replace();
					}

					if (i + 1 < options.Count)
					{
						if (GUILayout.Button(options[i + 1].content, GUILayout.Height(44)))
						{
							options[i + 1].Replace();
						}
					}

					GUILayout.EndHorizontal();
				}
			}
		}

		private static void Load()
		{
			options.Clear();

			string[] guids = AssetDatabase.FindAssets("t:ReplacementShaderSetupScriptableObject", null);

			foreach (string guid in guids)
			{
				options.Add((ReplacementShaderSetupScriptableObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(ReplacementShaderSetupScriptableObject)));
			}
		}
	}
}