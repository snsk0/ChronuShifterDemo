using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Kamifuta.Attribute
{
    [CustomPropertyDrawer(typeof(NonEditableAttribute))]
    public class NonEditableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(_position, _property, _label);
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif
