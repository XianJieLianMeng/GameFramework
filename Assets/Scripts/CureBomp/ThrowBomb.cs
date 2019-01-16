using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class ThrowBomb : MonoBehaviour
{
    public float initialSpeed = 5;

    [MinValue(0)]
    [MaxValue(90)]
    public float initialAngle = 45;

    private Vector3 _newPosition;

    private Rigidbody _rigidbody;

    public Vector3 originalPosition { get; private set; }

    private void Start()
    {
        UpdateOriginalPosition();
        float y = initialSpeed * Mathf.Sin(initialAngle * Mathf.Deg2Rad);
        float z = initialSpeed * Mathf.Cos(initialAngle * Mathf.Deg2Rad);

        Vector3 initialVelocity = new Vector3(0, y, z);
        _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.velocity = initialVelocity;
    }

    public void UpdateOriginalPosition()
    {
        originalPosition = transform.position;
        _newPosition = originalPosition;
    }

    public Vector3 GetPosition(float totalTime)
    {
        _newPosition.z = initialSpeed * Mathf.Cos(initialAngle*Mathf.Deg2Rad) * totalTime 
            + originalPosition.z;

        _newPosition.y = initialSpeed * Mathf.Sin(initialAngle*Mathf.Deg2Rad) * totalTime
            - 0.5f * GetGravity() * totalTime * totalTime 
            + originalPosition.y;
        return _newPosition;
    }

    public float GetTotalTime()
    {
        return 2* initialSpeed * Mathf.Sin(initialAngle * Mathf.Deg2Rad) / (GetGravity());
    }

    public float GetGravity()
    {
        return Mathf.Abs(Physics.gravity.y);
    }
}
