using UnityEngine;
using UnityEditor;
using System;

namespace TAO.SceneDebugViewer.Editor
{
	[CustomPropertyDrawer(typeof(ReplacementShaderSetupScriptableObject.GlobalShaderParameter))]
	public class GlobalShaderPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			Rect rectFoldout = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
			property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);

			position.height = EditorGUIUtility.singleLineHeight;

			if (property.isExpanded)
			{
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

				SerializedProperty name = property.FindPropertyRelative("m_name");
				EditorGUI.PropertyField(position, name);

				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

				SerializedProperty type = property.FindPropertyRelative("m_parameterType");
				EditorGUI.PropertyField(position, type);

				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

				ReplacementShaderSetupScriptableObject.ParameterType parameterType = (ReplacementShaderSetupScriptableObject.ParameterType)type.enumValueIndex;
				SerializedProperty p = null;
				bool includeChildren = false;

				switch (parameterType)
				{
					case ReplacementShaderSetupScriptableObject.ParameterType.Texture:
						p = property.FindPropertyRelative("m_texture");
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Vector:
						p = property.FindPropertyRelative("m_vector");

						if (p.isExpanded)
						{
							position.height = (EditorGUIUtility.singleLineHeight * 4) + (EditorGUIUtility.standardVerticalSpacing * 3);
							includeChildren = true;
						}

						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Color:
						p = property.FindPropertyRelative("m_color");
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Float:
						p = property.FindPropertyRelative("m_float");
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Int:
						p = property.FindPropertyRelative("m_int");
						break;
					default:
						break;
				}

				EditorGUI.PropertyField(position, p, includeChildren);
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			int totalLines = 1;

			if (property.isExpanded)
			{
				totalLines += 3;

				SerializedProperty type = property.FindPropertyRelative("m_parameterType");
				ReplacementShaderSetupScriptableObject.ParameterType parameterType = (ReplacementShaderSetupScriptableObject.ParameterType)type.enumValueIndex;

				switch (parameterType)
				{
					case ReplacementShaderSetupScriptableObject.ParameterType.Texture:
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Vector:
						if (property.FindPropertyRelative("m_vector").isExpanded)
						{
							totalLines += 4;
						}
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Color:
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Float:
						break;
					case ReplacementShaderSetupScriptableObject.ParameterType.Int:
						break;
					default:
						break;
				}
			}

			return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
		}
	}
}
