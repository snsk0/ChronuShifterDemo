using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chronus.KinematicPhysics.Editor
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(CharacterBody))]
    public class CharacterBodyInspector : UnityEditor.Editor
    {
        private bool _velocityFold = false;

        public override void OnInspectorGUI()
        {
            //オブジェクトをロード
            CharacterBody characterBody = target as CharacterBody;
            serializedObject.Update();

            FieldInfo colliderFieldInfo = typeof(CharacterBody).GetField("_collider", BindingFlags.NonPublic | BindingFlags.Instance);
            GUI.enabled = false;
            EditorGUILayout.Toggle("Collider", (CapsuleCollider)colliderFieldInfo.GetValue(characterBody));
            GUI.enabled = true;
            //GUI.enabled = false;
            //EditorGUILayout.ObjectField("Collider", characterBody.Collider, typeof(Collider), true);
            //GUI.enabled = true;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxSlopeAngle"));

            _velocityFold = EditorGUILayout.Foldout(_velocityFold, "Velocity");
            if(_velocityFold)
            {
                GUI.enabled = false;
                EditorGUILayout.FloatField("Magnitude", characterBody.Velocity.magnitude);
                EditorGUILayout.Vector3Field("Vector", characterBody.Velocity);
                GUI.enabled = true;
            }

            FieldInfo fieldInfo = typeof(CharacterBody).GetField("_isStickGround", BindingFlags.NonPublic | BindingFlags.Instance);
            GUI.enabled = false;
            EditorGUILayout.Toggle("IsStickGround", (bool)fieldInfo.GetValue(characterBody));
            GUI.enabled = true;

            //値を適用
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
