using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFollow : MonoBehaviour
{
    Transform _tfTarget;
    [SerializeField] Vector3 _offset = Vector3.zero;

    public void SetTarget(Transform target)
    {
        _tfTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tfTarget == null) return;

        transform.position = _tfTarget.position + _offset;
    }
}
