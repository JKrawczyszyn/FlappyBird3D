using Entry.Models;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Asset))]
public class AssetDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float singleElementWidth = position.width / 3f;
        var labelWidth = 70;
        var spacing = 10;

        var x = position.x;
        var y = position.y;
        var width = singleElementWidth;
        var height = 20f;
        EditorGUI.PropertyField(new Rect(x, y, width, height), property.FindPropertyRelative("name"), GUIContent.none);

        x += width + spacing;
        width = labelWidth - spacing;
        EditorGUI.LabelField(new Rect(x, y, width, height), "Asset Tag", new GUIStyle {alignment = TextAnchor.MiddleRight });

        x += width + spacing;
        width = singleElementWidth - labelWidth - spacing;
        EditorGUI.PropertyField(new Rect(x, y, width, height), property.FindPropertyRelative("assetTag"), GUIContent.none);

        x += width + spacing;
        width = labelWidth - spacing;
        EditorGUI.LabelField(new Rect(x, y, width, height), "Scene Tag", new GUIStyle {alignment = TextAnchor.MiddleRight });

        x += width + spacing;
        width = singleElementWidth - labelWidth - spacing;
        EditorGUI.PropertyField(new Rect(x, y, width, height), property.FindPropertyRelative("sceneName"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
