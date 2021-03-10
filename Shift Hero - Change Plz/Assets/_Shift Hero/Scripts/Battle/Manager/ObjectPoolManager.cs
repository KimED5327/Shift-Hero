using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    FloatingText,
    Archer_Arrow_01,
    Mage_Ball_01,
    Shard_Hit_Effect,
    MagicBall_Hit_Effect,
    Normal_Attack_Effect,
    Bow_Fire_Effect,
    Magic_Fire_Effect,
    Bow_Hit_Effect,
    Player_BackStep_Effect,
    Player_DefenceStep_Effect,
    Player_Teteport_Effect,
    Player_Change_Pre,
    Player_Change,
    Counter_Effect,
}

[System.Serializable]
public class Pool
{
    public string poolName;
    public PoolType type;
    public GameObject go;
    public int count;
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField] Pool[] _pools = null;
    Queue<GameObject>[] _queues;
    Dictionary<PoolType, Queue<GameObject>> _poolDic = new Dictionary<PoolType, Queue<GameObject>>();
    

    // Start is called before the first frame update
    void Start()
    {
        _queues = new Queue<GameObject>[_pools.Length];

        for (int i = 0; i < _pools.Length; i++)
        {
            _queues[i] = new Queue<GameObject>();
            AddPool(_pools[i].go, _pools[i].count, ref _queues[i]);
            _poolDic.Add(_pools[i].type, _queues[i]);
        }
    }

    void AddPool(GameObject go, int count, ref Queue<GameObject> queue)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(go, Vector3.zero, go.transform.rotation, this.transform);
            clone.gameObject.SetActive(false);
            queue.Enqueue(clone);
        }
    }

    void FindPoolAndAdd(ref Queue<GameObject> queue, PoolType type)
    {
        for (int i = 0; i < _pools.Length; i++)
        {
            if (_pools[i].type == type)
            {
                AddPool(_pools[i].go, 2, ref queue);
            }
        }
    }

    public GameObject GetObjectFromPool(PoolType type, bool active)
    {
        if (_poolDic.ContainsKey(type))
        {
            Queue<GameObject> queue = _poolDic[type];
            
            if (queue.Count <= 0)
                FindPoolAndAdd(ref queue, type);
            
            GameObject go = queue.Dequeue();
            go.SetActive(active);
            return go;
        }
        else
        {
            Debug.LogError(type + " 은 존재하지 않는 키값입니다.");
            return null;
        }
    }

    public GameObject GetObjectFromPool(PoolType type, Vector3 pos, bool active)
    {
        if (_poolDic.ContainsKey(type))
        {
            Queue<GameObject> queue = _poolDic[type];

            if (queue.Count <= 0)
                FindPoolAndAdd(ref queue, type);

            GameObject go = queue.Dequeue();
            go.transform.position = pos;
            go.SetActive(active);
            return go;
        }
        else
        {
            Debug.LogError(type + " 은 존재하지 않는 키값입니다.");
            return null;
        }
    }

    public void PushObjectAtPool(PoolType type, GameObject go)
    {
        if(_poolDic.ContainsKey(type))
        {
            go.SetActive(false);
            _poolDic[type].Enqueue(go);
        }
        else
        {
            Debug.LogError(type + " 은 존재하지 않는 키값입니다.");
        }
    }
}
