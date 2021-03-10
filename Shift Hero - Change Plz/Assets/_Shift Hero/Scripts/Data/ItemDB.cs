using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public bool isStockable;
    public bool isCash;
    public int price;
    public string desc;
    public string optionDesc;
    public Sprite sprite;
    public List<ItemOption> options;

    public Item(int id, string name, bool isStockable, bool isCash, int price, string desc, string optionDesc, Sprite sprite, List<ItemOption> options)
    {
        this.id = id;
        this.name = name;
        this.isStockable = isStockable;
        this.isCash = isCash;
        this.price = price;
        this.desc = desc;
        this.optionDesc = optionDesc;
        this.sprite = sprite;
        this.options = options;
    }
}

[System.Serializable]
public class ItemOption
{
    public string optionType;
    public float num;
}

public class ItemDB{

    // 특수 아이템 id값
    public static readonly int GOLD = 0, EXP = 1;
    
    // ID값 구분자
    public static readonly string EQUIP = "1", RUNE = "2", CHARM = "3", ACCOUNT = "4", CONSUMABLE = "5";
    public static readonly string SWORD = "100", BOW = "101", STAFF = "102";
    public static readonly string PLATE_ARMOR = "110", LEATHER_ARMOR = "111";

    // 캐릭터 구분
    public const int ARCHER = 0, WARRIOR = 1, MAGE = 2;

    // 옵션 구분
    public const string
        HP = "HP", HP_RATE = "HP_RATE",
        ATK = "ATK", ATK_RATE = "ATK_RATE", ATK_SPD_RATE = "ATK_SPD_RATE",
        DEF = "DEF", DEF_RATE = "DEF_RATE", DEF_IGNORE = "DEF_IGNORE",
        CRI_RATE = "CRI_RATE", CRI_DMG = "CRI_DMG", AVD_RATE = "AVD_RATE",
        REC = "REC", REC_RATE = "REC_RATE", REC_TIME = "REC_TIME",
        GOLD_RATE = "GOLD_RATE", EXP_RATE = "EXP_RATE", BONUS_RATE = "BONUS_RATE",
        RESUR_RATE = "RESUR_RATE", MIRROR_RATE = "MIRROR_RATE", MOVE_SPD_RATE = "MOVE_SPD_RATE";
    
    
    static int _itemCount;  // 아이템 수
    // 아이템 딕셔너리
    static Dictionary<int, Item> _itemDic = new Dictionary<int, Item>();

    // 아이템 추가
    public static void AddItem(int id, Item item)
    {
        _itemCount++;
        _itemDic.Add(id, item);
    }

    /// <summary>
    /// id로 해당 아이템 정보를 가져옴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Item GetItem(int id)
    {
        if (_itemDic.ContainsKey(id))
            return _itemDic[id];
        else
            return null;
    }

    /// <summary>
    /// id값의 첫자리를 itemType으로 구분하기 위해 리턴시킴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetItemType(int id)
    {
        return id.ToString().Substring(0, 1);
    }

    /// <summary>
    /// id값의 2~3번째 자리를 EquipType으로 구분하기 위해 리턴시킴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetEquipType(int id)
    {
        return id.ToString().Substring(0, 3);
    }

    /// <summary>
    /// 같은 유형의 장비 아이템인지 체크
    /// </summary>
    /// <param name="idA"></param>
    /// <param name="equipType"></param>
    /// <returns></returns>
    public static bool IsSameEquipType(int idA, string equipType)
    {
        return string.Equals(GetEquipType(idA), equipType);
    }

    /// <summary>
    /// 같은 유형의 아이템인지 체크
    /// </summary>
    /// <param name="idA"></param>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public static bool IsSameItemType(int idA, string itemType)
    {
        return string.Equals(GetItemType(idA), itemType);
    }


    /// <summary>
    /// 특정 id의 캐릭터가 장착할 수 있는 갑옷 아이템의 EquipType을 리턴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetArmorTypeFromCharacterID(int id)
    {
        switch(id)
        {
            case ARCHER: return LEATHER_ARMOR;
            case WARRIOR: return PLATE_ARMOR;
            case MAGE: return LEATHER_ARMOR;
            default: return LEATHER_ARMOR;
        };
    }

    /// <summary>
    /// 특정 id의 캐릭터가 장착할 수 있는 무기 아이템의 EquipType을 리턴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetWeaponTypeFromCharacterID(int id)
    {
        switch (id)
        {
            case ARCHER: return BOW;
            case WARRIOR: return SWORD;
            case MAGE: return STAFF;
            default: return BOW;
        };
    }

    public static int GetItemMaxCount() { return _itemCount; }

}
