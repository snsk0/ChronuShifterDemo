using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chronus.KinematicPhysics.Editor
{
    #if UNITY_EDITOR
    public class PhysicsSettingWindow : EditorWindow
    {
        [SerializeField] private float _collisionOffset;

        [MenuItem("Chronus/Physics")]
        private static void Open()
        {
            GetWindow<PhysicsSettingWindow>("PhysicsSetting");
        }

        private void OnGUI()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_collisionOffset"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}
