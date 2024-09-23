using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EndPointData))]
public class EndPointEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // EndPointData의 type을 가져옴
        var typeProperty = property.FindPropertyRelative("type");
        var urlProperty = property.FindPropertyRelative("url");

        // 라벨을 type으로 설정
        label.text = typeProperty.enumDisplayNames[typeProperty.enumValueIndex];
        
        // 기본 Drawer 사용
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 기본 높이 계산
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
