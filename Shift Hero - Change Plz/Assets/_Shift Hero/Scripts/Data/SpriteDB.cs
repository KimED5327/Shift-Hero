using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSprite
{
    public Sprite[] sprite;
}

[System.Serializable]
public class PlayerSprite
{
    public int[] id;
    public Sprite[] sprite;
}

[System.Serializable]
public class MonsterSprite
{
    public Sprite[] sprite;
}

[System.Serializable]
public class AchieveSprite
{
    public Sprite[] sprite;
}

[System.Serializable]
public class PerkSprite
{
    public Sprite[] sprite;
}


public class SpriteDB : MonoBehaviour
{
    public static bool isSetting = false;

    [SerializeField] ItemSprite _itemSprites;
    [SerializeField] PlayerSprite _playerSprites;
    [SerializeField] MonsterSprite _monsterSprites;
    [SerializeField] AchieveSprite _achieveSprites;
    [SerializeField] AchieveSprite _perkSprites;

    static Dictionary<int, Sprite> _itemSpriteDic = new Dictionary<int, Sprite>();
    static Dictionary<int, Sprite> _playerSpriteDic = new Dictionary<int, Sprite>();
    static Dictionary<int, Sprite> _monsterSpriteDic = new Dictionary<int, Sprite>();
    static Dictionary<int, Sprite> _achieveSpriteDic = new Dictionary<int, Sprite>();
    static Dictionary<int, Sprite> _perkSpriteDic = new Dictionary<int, Sprite>();

    // Start is called before the first frame update
    void Awake()
    {
        if (!isSetting)
        {
            isSetting = true;

            for (int i = 0; i < _itemSprites.sprite.Length; i++)
                _itemSpriteDic.Add(int.Parse(_itemSprites.sprite[i].name), _itemSprites.sprite[i]);

            for (int i = 0; i < _playerSprites.sprite.Length; i++)
                _playerSpriteDic.Add(_playerSprites.id[i], _playerSprites.sprite[i]);

            for (int i = 0; i < _monsterSprites.sprite.Length; i++)
                _monsterSpriteDic.Add(i, _monsterSprites.sprite[i]);

            for (int i = 0; i < _achieveSprites.sprite.Length; i++)
                _achieveSpriteDic.Add(i, _achieveSprites.sprite[i]);

            for (int i = 0; i < _perkSprites.sprite.Length; i++)
                _perkSpriteDic.Add(i, _perkSprites.sprite[i]);
        }
        
    }

    public static Sprite GetItemSprite(int id)
    {
        if (_itemSpriteDic.ContainsKey(id))
        {
            return _itemSpriteDic[id];
        }
        else
        {
            Debug.LogError(id + "는 존재하지 않는 키값입니다.");
            return null;
        }
    }
    public static Sprite GetPlayerSprite(int id)
    {
        if (_playerSpriteDic.ContainsKey(id))
        {
            return _playerSpriteDic[id];
        }
        else
        {
            Debug.LogError(id + "는 존재하지 않는 키값입니다.");
            return null;
        }
    }

    public static Sprite GetMonsterSprite(int id)
    {
        if (_monsterSpriteDic.ContainsKey(id))
        {
            return _monsterSpriteDic[id];
        }
        else
        {
            Debug.LogError(id + "는 존재하지 않는 키값입니다.");
            return null;
        }
    }

    public static Sprite GetAchieveSprite(int id)
    {
        if (_achieveSpriteDic.ContainsKey(id))
        {
            return _achieveSpriteDic[id];
        }
        else
        {
            Debug.LogError(id + "는 존재하지 않는 키값입니다.");
            return null;
        }
    }

    public static Sprite GetPerkSprite(int id)
    {
        if (_perkSpriteDic.ContainsKey(id))
        {
            return _perkSpriteDic[id];
        }
        else
        {
            Debug.LogError(id + "는 존재하지 않는 키값입니다.");
            return null;
        }
    }
}
