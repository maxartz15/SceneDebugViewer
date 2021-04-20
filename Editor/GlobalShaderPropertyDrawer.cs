using UnityEngine;
using UnityEditor;

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
				SerializedProperty p = property.FindPropertyRelative(parameterType.ToString());

				position.height = EditorGUI.GetPropertyHeight(p, includeChildren: true);
				EditorGUI.PropertyField(position, p, includeChildren: true);
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var spacing = EditorGUIUtility.standardVerticalSpacing;
			var height = EditorGUIUtility.singleLineHeight + spacing;

			if (property.isExpanded)
			{
				height += (EditorGUIUtility.singleLineHeight + spacing) * 2;

				SerializedProperty type = property.FindPropertyRelative("m_parameterType");
				ReplacementShaderSetupScriptableObject.ParameterType parameterType = (ReplacementShaderSetupScriptableObject.ParameterType)type.enumValueIndex;

				height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(parameterType.ToString()), true);
			}

			return height;
		}
	}
}