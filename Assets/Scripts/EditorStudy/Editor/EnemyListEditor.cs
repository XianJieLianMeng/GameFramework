using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyList))]
public class EnemyListEditor : Editor
{
    private EnemyList _enemyList { get { return target as EnemyList; } }

    public override void OnInspectorGUI()
    {
        GUI.color = _enemyList.backGroundColor;
        GUILayout.BeginVertical("box");
        GUI.color = Color.white;

        DrawSettingData();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Enemy", EditorStyles.boldLabel);
        //绘制每个敌人数据
        for (int i = 0; i < _enemyList.enemies.Count; i++)
        {
            _enemyList.showEnemyFoldout[i] = EditorGUILayout.Foldout(_enemyList.showEnemyFoldout[i], ("ShowEnemy" + i));

            if (_enemyList.showEnemyFoldout[i])
            {
                EnemyList.Enemy enemy = _enemyList.enemies[i];
               
                enemy.hp = EditorGUILayout.IntSlider("HP", enemy.hp, _enemyList.minValue, _enemyList.maxValue);
                enemy.atk = EditorGUILayout.IntSlider("ATK", enemy.atk, _enemyList.minValue, _enemyList.maxValue);
                enemy.def = EditorGUILayout.IntSlider("DEF", enemy.def, _enemyList.minValue, _enemyList.maxValue);

                DrawProgressBar(enemy.hp, "Hp");
                DrawProgressBar(enemy.atk, "Atk");
                DrawProgressBar(enemy.def, "Def");

                GUI.color = _enemyList.buttonColor;
                if (GUILayout.Button("Remove"))
                {
                    _enemyList.enemies.Remove(enemy);
                    _enemyList.showEnemyFoldout.RemoveAt(i);
                }
                GUI.color = Color.white;
            }
        }

        EditorGUILayout.EndVertical();

        GUI.color = _enemyList.buttonColor;
        if(GUILayout.Button("Add a enemy"))
        {
            EnemyList.Enemy enemy = new EnemyList.Enemy();
            _enemyList.enemies.Add(enemy);
            _enemyList.showEnemyFoldout.Add(true);
        }
        GUI.color = Color.white;

        EditorGUILayout.EndVertical();
    }

    private void DrawSettingData()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Setting", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        _enemyList.minValue = EditorGUILayout.IntField("MinValue", _enemyList.minValue);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Color", GUILayout.MaxWidth(80));
        _enemyList.minValueColor = EditorGUILayout.ColorField(_enemyList.minValueColor);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        
        _enemyList.maxValue = EditorGUILayout.IntField("MaxValue", _enemyList.maxValue);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Color", GUILayout.MaxWidth(80));
        _enemyList.maxValueColor = EditorGUILayout.ColorField(_enemyList.maxValueColor);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");

        _enemyList.backGroundColor = EditorGUILayout.ColorField("BgColor", _enemyList.backGroundColor);
        _enemyList.buttonColor = EditorGUILayout.ColorField("ButtonColor", _enemyList.buttonColor);

        EditorGUILayout.EndVertical();
    }

    private void DrawProgressBar(float value, string text)
    {
        float normalizeValue = (value - _enemyList.minValue) / (_enemyList.maxValue - _enemyList.minValue);

        Color maxColor = _enemyList.maxValueColor;
        Color minColor = _enemyList.minValueColor;

        float r = Mathf.Lerp(minColor.r, maxColor.r, normalizeValue);
        float g = Mathf.Lerp(minColor.g, maxColor.g, normalizeValue);
        float b = Mathf.Lerp(minColor.b, maxColor.b, normalizeValue);

        Color color = new Color(r, g, b, 0.8f);

        Rect rect = GUILayoutUtility.GetRect(50, 20);
        GUI.color = color;
        EditorGUI.ProgressBar(rect, normalizeValue, text);
        GUI.color = Color.white;
    }
}
