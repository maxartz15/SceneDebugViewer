using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TAO.SceneDebugViewer.Editor
{
#if UNITY_EDITOR
	[CreateAssetMenu(menuName = "ReplacementShaderSetup", order = 99999)]
	public class ReplacementShaderSetupScriptableObject : ScriptableObject
	{
		[Header("GUI")]
		[SerializeField]
		private GContent content = new GContent();
		public GContent Content => content;

		[Header("Shader")]
		public Shader shader = null;
		public string replacementTag = "";
		public List<GlobalShaderParameter> parameters = new List<GlobalShaderParameter>();

		public virtual void Replace()
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

		private void OnValidate()
		{
			content.compact.text = null;
			content.compact.image = content.icon;
			content.compact.tooltip = content.tooltip;
			content.normal.text = string.Format("    {0}", name);
			content.normal.image = content.icon;
			content.normal.tooltip = content.tooltip;
		}

		[System.Serializable]
		public struct GlobalShaderParameter
		{
			public string m_name;

			public ParameterType m_parameterType;

			public Texture2D m_texture;
			public Vector4 m_vector;
			public Vector4[] m_vectorArray;
			public Color m_color;
			public Color[] m_colorArray;
			public float m_float;
			public float[] m_floatArray;
			public int m_int;
			public Matrix4x4 m_matrix;
			public Matrix4x4[] m_matrixArray;

			public void Set()
			{
				switch (m_parameterType)
				{
					case ParameterType.m_texture:
						Shader.SetGlobalTexture(m_name, m_texture);
						break;
					case ParameterType.m_vector:
						Shader.SetGlobalVector(m_name, m_vector);
						break;
					case ParameterType.m_vectorArray:
						Shader.SetGlobalVectorArray(m_name, m_vectorArray);
						break;
					case ParameterType.m_color:
						Shader.SetGlobalColor(m_name, m_color);
						break;
					case ParameterType.m_colorArray:
						Shader.SetGlobalVectorArray(m_name, ColorArrayToVector4Array(m_colorArray));
						break;
					case ParameterType.m_float:
						Shader.SetGlobalFloat(m_name, m_float);
						break;
					case ParameterType.m_floatArray:
						Shader.SetGlobalFloatArray(m_name, m_floatArray);
						break;
					case ParameterType.m_int:
						Shader.SetGlobalInt(m_name, m_int);
						break;
					case ParameterType.m_matrix:
						Shader.SetGlobalMatrix(m_name, m_matrix);
						break;
					case ParameterType.m_matrixArray:
						Shader.SetGlobalMatrixArray(m_name, m_matrixArray);
						break;
					default:
						break;
				}
			}
		}

		public enum ParameterType
		{
			m_texture,
			m_vector,
			m_vectorArray,
			m_color,
			m_colorArray,
			m_float,
			m_floatArray,
			m_int,
			m_matrix,
			m_matrixArray
		}

		private static Vector4[] ColorArrayToVector4Array(Color[] c)
		{
			Vector4[] v = new Vector4[c.Length];

			for (int i = 0; i < v.Length; i++)
			{
				v[i] = c[i];
			}

			return v;
		}

		[System.Serializable]
		public struct GContent
		{
			public Texture2D icon;
			public string tooltip;
			public int sortingOrder;

			[HideInInspector]
			public GUIContent compact;
			[HideInInspector]
			public GUIContent normal;
		}
	}

	public class RSSOComparer : IComparer<ReplacementShaderSetupScriptableObject>
	{
		public int Compare(ReplacementShaderSetupScriptableObject x, ReplacementShaderSetupScriptableObject y)
		{
			if (x == null || y == null)
			{
				return 0;
			}

			return x.Content.sortingOrder.CompareTo(y.Content.sortingOrder);
		}
	}
#endif
}