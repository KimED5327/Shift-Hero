using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] PoolType _poolType = 0;
    [SerializeField] float _destroyTime = 0f;

    void OnEnable()
    {
        Invoke(nameof(BackObjectPool), _destroyTime);
    }

    void BackObjectPool()
    {
        if(gameObject.activeSelf)
            ObjectPoolManager.Instance.PushObjectAtPool(_poolType, this.gameObject);
    }
}
