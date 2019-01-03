using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public int intValue = 1;

    public float floatValue = 2.0f;

    public bool boolValue = true;

    public Direction direction = Direction.Left;

    public GameObject prefab;
}
