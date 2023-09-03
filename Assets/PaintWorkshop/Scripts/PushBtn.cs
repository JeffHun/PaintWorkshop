using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushBtn : MonoBehaviour
{
    public float Threshold = .1f;

    public float DeadZone = .025f;

    bool _isPressed;
    Vector3 _startPos;
    ConfigurableJoint _joint;

    public UnityEvent OnPressed, OnReleased;

    private void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (!_isPressed && GetValue() + Threshold >= 1)
            Pressed();
        if (_isPressed && GetValue() - Threshold <= 0)
            Released();
    }

    float GetValue()
    {
        float value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < DeadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    void Pressed()
    { 
        _isPressed = true;
        OnPressed.Invoke();
    }

    void Released()
    {
        _isPressed = false;
        OnReleased.Invoke();
    }
}
