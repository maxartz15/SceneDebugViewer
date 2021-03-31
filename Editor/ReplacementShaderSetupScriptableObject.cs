using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TAO.SceneDebugViewer.Editor
{
#if UNITY_EDITOR
	[CreateAssetMenu(menuName = "SceneDebugViewer/ReplacementShaderSetup")]
	public class ReplacementShaderSetupScriptableObject : ScriptableObject
	{
		[Header("GUI")]
		public GUIContent content = new GUIContent();
		[Header("Shader")]
		public Shader shader = null;
		public string replacementTag = "";
		public List<GlobalShaderParameter> parameters = new List<GlobalShaderParameter>();

		public void Replace()
		{
			SetupShaderParameters();

			foreach (SceneView s in SceneView.sceneViews)
			{
				s.SetSceneViewShaderReplace(shader, replacementTag);
				s.Repaint();
			}
		}

		protected void SetupShaderParameters()
		{
			foreach (var p in parameters)
			{
				p.Set();
			}
		}

		[System.Serializable]
		public struct GlobalShaderParameter
		{
			public string m_name;

			public ParameterType m_parameterType;

			public Texture2D m_texture;
			public Vector4 m_vector;
			public Color m_color;
			public float m_float;
			public int m_int;

			public void Set()
			{
				switch (m_parameterType)
				{
					case ParameterType.Texture:
						Shader.SetGlobalTexture(m_name, m_texture);
						break;
					case ParameterType.Vector:
						Shader.SetGlobalVector(m_name, m_vector);
						break;
					case ParameterType.Color:
						Shader.SetGlobalColor(m_name, m_color);
						break;
					case ParameterType.Float:
						Shader.SetGlobalFloat(m_name, m_float);
						break;
					case ParameterType.Int:
						Shader.SetGlobalInt(m_name, m_int);
						break;
					default:
						break;
				}
			}
		}

		public enum ParameterType
		{
			Texture,
			Vector,
			Color,
			Float,
			Int
		}
	}
#endif
}