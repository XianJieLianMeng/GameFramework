using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class OdinTest : SerializedMonoBehaviour
{
    [Title("数据类型")]
    [FoldoutGroup("基本数据")]
    //基本数据类型
    public bool boolValue;
  
    [ShowIf("boolValue")]
    [FoldoutGroup("基本数据")]
    public string str;

    [MinValue(0)]
    [FoldoutGroup("基本数据")]
    public int minValueLimit = 1;

    [MaxValue(100)]
    [FoldoutGroup("基本数据")]
    public int maxValueLimit = 99;

    [Wrap(0, 100)]
    [FoldoutGroup("基本数据")]
    public int valueLimit = 50;

    [MinMaxSlider(0, 100)]
    public Vector2 v;

    [ProgressBar(0, 100, ColorMember = "ChangeColor")]
    public float hpBar;

    public Color ChangeColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, MathUtilities.LinearStep(0, 100, value));
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [EnumToggleButtons]
    public Direction direction;

    [Title("数据结构")]
    [ListDrawerSettings(NumberOfItemsPerPage = 5)]
    public List<int> list;

    public Dictionary<int, string> map;

    //PopUp
    [ListDrawerSettings(NumberOfItemsPerPage = 5)]
    public List<string> stringList;

    public string selectString;
}
