using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor;

[CustomEditor(typeof(ConfirmableButton), true)]
[CanEditMultipleObjects]
public class ConfirmableButtonEditor : ButtonEditor
{
    SerializedProperty onConfirmProperty;
    protected override void OnEnable() {
        base.OnEnable();
        onConfirmProperty = serializedObject.FindProperty("onConfirm");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(onConfirmProperty);
        serializedObject.ApplyModifiedProperties();
    }
}
