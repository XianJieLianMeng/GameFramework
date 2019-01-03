
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Variable))]  //使用特性说明是用来绘制 Variable 类的
public class VariableEditor : Editor
{
    //持有Variable类的引用，将editor转换为 variable
    private Variable _variable { get { return target as Variable; } }

    #region 使用序列化属性来绘制

    private SerializedProperty _intValue;

    private SerializedProperty _floatValue;

    private SerializedProperty _boolValue;

    private SerializedProperty _direction;

    private SerializedProperty _prefab;

    private void OnEnable()
    {
        _intValue = serializedObject.FindProperty("intValue");
        _floatValue = serializedObject.FindProperty("floatValue");
        _boolValue = serializedObject.FindProperty("boolValue");
        _direction = serializedObject.FindProperty("direction");
        _prefab = serializedObject.FindProperty("prefab"); 
    }

    #endregion

    public override void OnInspectorGUI()
    {
        //使用EditorGUILayout绘制属性

        //_variable.intValue = EditorGUILayout.IntField("Int", _variable.intValue);

        //_variable.floatValue = EditorGUILayout.FloatField("Float", _variable.floatValue);

        //_variable.boolValue = EditorGUILayout.Toggle("Bool", _variable.boolValue);

        //_variable.direction = (Variable.Direction) EditorGUILayout.EnumPopup("Direction", _variable.direction);

        //_variable.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _variable.prefab,typeof(GameObject),true);

        //使用专门绘制序列化属性的方法，可以实现 ctrl + z 撤销方法

        //serializedObject.Update();

        //EditorGUILayout.PropertyField(_intValue);

        //EditorGUILayout.PropertyField(_floatValue);

        //EditorGUILayout.PropertyField(_boolValue);

        //EditorGUILayout.PropertyField(_direction);

        //EditorGUILayout.PropertyField(_prefab);

        //serializedObject.ApplyModifiedProperties();

        //使用第一种方式实现撤销

        // 1.检查是否在inspector面板上对属性进行操作
        EditorGUI.BeginChangeCheck();

        // 2.获取属性

        int intValue = EditorGUILayout.IntField("Int", _variable.intValue);

        float floatValue = EditorGUILayout.FloatField("Float", _variable.floatValue);

        bool boolValue = EditorGUILayout.Toggle("Bool", _variable.boolValue);

        Variable.Direction direction = (Variable.Direction)EditorGUILayout.EnumPopup("Direction", _variable.direction);

        GameObject prefab = (GameObject) EditorGUILayout.ObjectField("Prefab", _variable.prefab,typeof(GameObject),true);

        if(EditorGUI.EndChangeCheck())
        {
            //先保存下来，再进行赋值操作
            Undo.RegisterCompleteObjectUndo(_variable,"Change Variable");

            _variable.intValue = intValue;

            _variable.floatValue = floatValue;

            _variable.boolValue = boolValue;

            _variable.direction = direction;

            _variable.prefab = prefab;
        }

        //总结，使用序列化属性绘制代码更简单些
    }
}
