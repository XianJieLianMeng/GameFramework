
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Layout))] //使用特性说明是用来绘制Layout类的
public class LayoutEditor : Editor
{
    private Layout _layout { get { return target as Layout; } }

    public override void OnInspectorGUI()
    {

        //绘制颜色

        _layout.backGroundColor = EditorGUILayout.ColorField("Color", _layout.backGroundColor);

        GUI.color = _layout.backGroundColor;

        //绘制水平布局

        EditorGUILayout.BeginHorizontal("box");
        GUI.color = Color.white;

        EditorGUILayout.LabelField("Horizontal", GUILayout.MaxWidth(100));
        //设置字符串的宽度
        EditorGUILayout.LabelField("H2",GUILayout.MaxWidth(100));
        EditorGUILayout.LabelField("H3",GUILayout.MaxWidth(100));
        EditorGUILayout.LabelField("H4", GUILayout.MaxWidth(100));

        EditorGUILayout.EndHorizontal();

        //绘制垂直布局

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Vertical");
        EditorGUILayout.LabelField("垂直",GUILayout.MaxHeight(100));
        EditorGUILayout.LabelField("V1", GUILayout.MaxHeight(100));
        EditorGUILayout.LabelField("V2", GUILayout.MaxHeight(100));
        EditorGUILayout.LabelField("V3", GUILayout.MaxHeight(100));

        EditorGUILayout.EndVertical();

        //编辑器折叠

       _layout.isShow = EditorGUILayout.Foldout(_layout.isShow, "Foldout");

        if(_layout.isShow)
        {
          _layout.percent = EditorGUILayout.Slider("Percent",_layout.percent,1,10);
            GUILayout.Button("Button");
        }
    }
}
