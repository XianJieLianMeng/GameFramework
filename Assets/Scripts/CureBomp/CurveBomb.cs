using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class CurveBomb : MonoBehaviour
{
    [MinValue(0)]
    public float initialSpeed = 5;

    [MinValue(0)]
    [MaxValue(90)]
    public float initialAngle = 45;

    private Rigidbody _rigidbody;

    private Vector3 _newPosition;

    public Vector3 originalPosition { get; private set; }

    private void Start()
    {
        UpdateOriginalPosition();
        float y = initialSpeed * Mathf.Sin(initialAngle * Mathf.Deg2Rad);
        float z = initialSpeed * Mathf.Cos(initialAngle * Mathf.Deg2Rad);

        Vector3 initialVelocity = transform.forward * z + transform.up * y;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = initialVelocity;
    }

    public void UpdateOriginalPosition()
    {
        originalPosition = transform.position;
        _newPosition = originalPosition;
    }

    public Vector3 GetWorldPosition(float totalTime)
    {
        float deltaZ = initialSpeed * Mathf.Cos(initialAngle * Mathf.Deg2Rad) * totalTime;
        float deltaY = initialSpeed * Mathf.Sin(initialAngle * Mathf.Deg2Rad) * totalTime
             - 0.5f * GetGravity() * totalTime * totalTime;
        _newPosition = transform.forward * deltaZ + transform.up * deltaY + GetOffsetPosition();
        return _newPosition;
    }

    public float GetGravity()
    {
        return Mathf.Abs(Physics.gravity.y);
    }

    public float GetTotalTime()
    {
        return 2 * initialSpeed * Mathf.Sin(initialAngle * Mathf.Deg2Rad) / (GetGravity());
    }

    public Vector3 GetOffsetPosition()
    {
        if (transform.parent == null)
        {
            return Vector3.zero;
        }
        else
        {
            return transform.parent.position;
        }
    }
}
