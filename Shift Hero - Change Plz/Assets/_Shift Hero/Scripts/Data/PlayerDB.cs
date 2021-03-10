using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int id;
    public Sprite sprite;
    public string name;
    public string job;
    public string desc;
    public bool isOpen;
    public int level;
    public int exp;
    public int hp;
    public int def;
    public int atk;
    public float attackSpeed;
    public float attackRange;
    public float criRate;
    public float criMultiply;
    public float avoidanceRate;
    public float moveSpeed;
    public int recoverHp;
    public float recoverTime;
    public int levelupHp;
    public int levelupAtk;
    public int levelupDef;
    public int price;

    public OriginStatusBuffer originStatus;
}



public class PlayerDB
{
    static Dictionary<int, PlayerData> _playerDic = new Dictionary<int, PlayerData>();

    static int _playerMaxCount;

    static int[] levelUpTable;
    static int[] accountLevelUpTable;

    public static void SetLevelUpTable(int[] pLevelupTable) => levelUpTable = pLevelupTable;
    public static void SetAccountLevelUpTable(int[] pAccountLevelUpTable) => accountLevelUpTable = pAccountLevelUpTable;


    /// <summary>
    /// 플레이어 데이터 추가
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public static void AddItem(int id, PlayerData data)
    {
        // 아처, 워리어, 힐러까진 잠금해제
        if(id <= 2)
        {
            data.isOpen = true;
        }
        _playerMaxCount++;
        _playerDic.Add(id, data);
    }

    /// <summary>
    /// 캐릭터 구입시 획득한 캐릭터 리스트에 추가
    /// </summary>
    /// <param name="id"></param>
    public static void SetBuyCharacterID(int id)
    {
        if (_playerDic.ContainsKey(id))
        {
            _playerDic[id].isOpen = true;
            AchieveDB.IncreasePlayerGetCount();
        }

    }

    /// <summary>
    /// 플레이어 데이터 가져오기초
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static PlayerData GetPlayerData(int id)
    {
        if (_playerDic.ContainsKey(id))
            return _playerDic[id];
        else
            return null;
    }


    /// <summary>
    /// 만렙이면 -1 리턴
    /// </summary>
    /// <param name="curLevel"></param>
    /// <returns></returns>
    public static int GetLevelUpNeedExp(int curLevel)
    {
        if (curLevel < levelUpTable.Length)
            return levelUpTable[curLevel - 1];
        else
            return -1;
    }

    /// <summary>
    /// 만렙이면 -1 리턴
    /// </summary>
    /// <param name="curLevel"></param>
    /// <returns></returns>
    public static int GetAccountLevelUpNeedExp(int curLevel)
    {
        if (curLevel < accountLevelUpTable.Length)
            return accountLevelUpTable[curLevel - 1];
        else
            return -1;
    }

    public static int GetPlayerMaxCount() { return _playerMaxCount; }
}
