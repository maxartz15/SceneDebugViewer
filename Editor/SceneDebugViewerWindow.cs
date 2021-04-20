using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TAO.SceneDebugViewer.Editor
{
	public class SceneDebugViewerWindow : EditorWindow
	{
		public static SceneDebugViewerWindow window = null;
		public static List<ReplacementShaderSetupScriptableObject> options = new List<ReplacementShaderSetupScriptableObject>();

		private GUIStyle optionsButtonStyle = null;

		[MenuItem("Window/Analysis/SceneDebugViewer")]
		static void Init()
		{
			Load();

			window = (SceneDebugViewerWindow)GetWindow(typeof(SceneDebugViewerWindow));
			window.titleContent = new GUIContent("SDV");
			//window.maxSize = new Vector2(101, window.maxSize.y);
			window.minSize = new Vector2(101, window.minSize.y);
			window.Show();
		}

		private void OnGUI()
		{
			if (window == null)
			{
				window = (SceneDebugViewerWindow)GetWindow(typeof(SceneDebugViewerWindow));
			}

			using (new GUILayout.VerticalScope())
			{
				if (GUILayout.Button("Reload"))
				{
					Load();
				}

				if (window.position.width <= 101)
				{
					// Compact grid.
					optionsButtonStyle = new GUIStyle(GUI.skin.button)
					{
						alignment = TextAnchor.MiddleCenter,
						fixedHeight = 44
					};

					for (int i = 0; i < options.Count; i += 2)
					{
						GUILayout.BeginHorizontal();

						if (GUILayout.Button(options[i].Content.compact, optionsButtonStyle))
						{
							options[i].Replace();
						}

						if (i + 1 < options.Count)
						{
							if (GUILayout.Button(options[i + 1].Content.compact, optionsButtonStyle))
							{
								options[i + 1].Replace();
							}
						}

						GUILayout.EndHorizontal();
					}
				}
				else
				{
					// Normal list.
					optionsButtonStyle = new GUIStyle(GUI.skin.button)
					{
						alignment = TextAnchor.MiddleLeft,
						fixedHeight = 44
					};

					for (int i = 0; i < options.Count; i ++)
					{
						if (GUILayout.Button(options[i].Content.normal, optionsButtonStyle))
						{
							options[i].Replace();
						}
					}
				}
			}
		}

		private static void Load()
		{
			options.Clear();

			string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(ReplacementShaderSetupScriptableObject).ToString()), null);

			foreach (string guid in guids)
			{
				options.Add((ReplacementShaderSetupScriptableObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(ReplacementShaderSetupScriptableObject)));
			}

			SortOptions();
		}

		private static void SortOptions()
		{
			if (options != null)
			{
				RSSOComparer comparer = new RSSOComparer();

				options.Sort(comparer);
			}
		}
	}
}