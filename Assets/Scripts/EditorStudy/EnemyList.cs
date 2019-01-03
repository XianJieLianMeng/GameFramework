using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField]
    public class Enemy
    {
        public int hp { get; set; }

        public int atk { get; set; }

        public int def { get; set; }
    }

    public List<Enemy> enemies = new List<Enemy>();

    public List<bool> showEnemyFoldout = new List<bool>();

    public int maxValue = 100;

    public int minValue = 0;

    public Color maxValueColor;

    public Color minValueColor;

    public Color backGroundColor;

    public Color buttonColor;
}
