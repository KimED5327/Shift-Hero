using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterData
{
    public int id;
    public Sprite sprite;
    public string name;
    public string desc;
    public int gold;
    public int exp;
    public int hp;
    public int def;
    public int atk;
    public float attackSpeed;
    public float attackRange;
    public float viewRange;
    public float criRate;
    public float criMultiply;
    public float avoidanceRate;
    public float moveSpeed;
    public int recoverHp;
    public float recoverTime;
    public string animPath;

    public MonsterData(int id, Sprite sprite, string name, string desc, int gold, int exp, int hp, int def, int atk, float attackSpeed, float attackRange, float viewRange, float criRate, float criMultiply, float avoidanceRate, float moveSpeed, int recoverHp, float recoverTime, string animPath)
    {
        this.id = id;
        this.sprite = sprite;
        this.name = name;
        this.desc = desc;
        this.gold = gold;
        this.exp = exp;
        this.hp = hp;
        this.def = def;
        this.atk = atk;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.viewRange = viewRange;
        this.criRate = criRate;
        this.criMultiply = criMultiply;
        this.avoidanceRate = avoidanceRate;
        this.moveSpeed = moveSpeed;
        this.recoverHp = recoverHp;
        this.recoverTime = recoverTime;
        this.animPath = animPath;
    }
}

public class MonsterDB : MonoBehaviour
{ 
    // 몬스터 최대 개수
    static int _monsterMaxCount;
    
    // 몬스터 딕셔너리
    static Dictionary<int, MonsterData> _monsterDic = new Dictionary<int, MonsterData>();

    /// <summary>
    /// 몬스터 추가
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public static void AddMonster(int id, MonsterData data)
    {
        _monsterMaxCount++;
        _monsterDic.Add(id, data);
    }

    /// <summary>
    /// 해당 id의 몬스터 데이터 리턴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static MonsterData GetMonsterData(int id)
    {
        if (_monsterDic.ContainsKey(id))
            return _monsterDic[id];
        else
            return null;
    }
  
    public static int GetMonsterMaxCount() { return _monsterMaxCount; }
}
