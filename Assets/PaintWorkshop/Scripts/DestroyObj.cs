using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public float Speed;

    float _timer;
    bool _isDetected;
    GameObject _part;
    Vector3 _iniScale;
    Vector3 _targetScale = new Vector3(.001f, .001f, .001f);

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bumper") || other.gameObject.CompareTag("Hood") || other.gameObject.CompareTag("Door"))
        {
            _part = other.gameObject.transform.parent.gameObject;
            _iniScale = _part.transform.localScale;
            _part.GetComponent<Rigidbody>().isKinematic = true;
            _isDetected = true;
        }
    }

    private void Update()
    {
        if(_isDetected)
        {
            _timer += Time.deltaTime;
            _part.transform.localScale = Vector3.Lerp(_iniScale, _targetScale, _timer * Speed);
            if (_part.transform.localScale == _targetScale)
            {
                Destroy(_part);
                _timer = 0;
                _isDetected = false;
            }
        }
    }
}
