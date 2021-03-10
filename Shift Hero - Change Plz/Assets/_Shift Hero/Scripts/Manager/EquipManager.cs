using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipList
{
    public int[] equipSlots;
}

public class EquipManager : MonoBehaviour
{
    static readonly string path = "/EquipItems.dat";
    const int MAX_EQUIP_SLOT = 4;         // 장착 슬롯 최대 개수

    // 슬롯 구분자
    public static readonly int WEAPON = 0, ARMOR = 1, RUNE_ONE = 2, RUNE_TWO = 3;

    // 슬롯 리스트
    EquipList[] _equipList;
    CharacterManager _characterManager;

    private void Awake()
    {
        _characterManager = FindObjectOfType<CharacterManager>();
    }

    /// <summary>
    /// count 수만큼 장착 슬롯 세팅
    /// </summary>
    /// <param name="count"></param>
    public void SetEquipList(int count)
    {
        // 캐릭터 수만큼 슬롯 증가
        _equipList = new EquipList[count];

        // 캐릭터마다 4개씩 슬롯 세팅
        for(int i = 0; i < _equipList.Length; i++)
        {
            _equipList[i] = new EquipList { equipSlots = new int[MAX_EQUIP_SLOT] };

            for (int k = 0; k < _equipList[i].equipSlots.Length; k++)
            {
                _equipList[i].equipSlots[k] = -1;
            }
        }

        LoadEquipSlot();

    }

    /// <summary>
    /// characterID의 장착 슬롯 slotNum에 itemID 아이템을 등록시킴
    /// </summary>
    /// <param name="characterID"></param>
    /// <param name="itemId"></param>
    /// <param name="slotNum"></param>
    /// <returns></returns>
    public int SetEquipSlot(int characterID, int itemId, int slotNum)
    {
        // 기존 장착된 아이템은 제거
        int tempID = _equipList[characterID].equipSlots[slotNum];
        // 제거된 장착 아이템이 있다면 반영된 옵션 수치 제거
        if(tempID > 0)
            ApplyEquipItemOption(characterID, tempID, -1);

        // 새로 아이템 장착
        _equipList[characterID].equipSlots[slotNum] = itemId;

        // 장착된 아이템의 옵션 수치 반영
        ApplyEquipItemOption(characterID, itemId, 1);

        SaveEquipSlot(); // 저장

        return tempID;
    }


    // 아이템 수치 반영
    public void ApplyEquipItemOption(int characterID, int itemId, int reverseValue = 1)
    {
        if (_characterManager == null) 
            _characterManager = FindObjectOfType<CharacterManager>();

        Player player = _characterManager.GetPlayerFromID(characterID);
        Item item = ItemDB.GetItem(itemId);
        for (int i = 0; i < item.options.Count; i++)
        {
            float value = item.options[i].num * reverseValue;

            switch (item.options[i].optionType)
            {
                case ItemDB.ATK: player.SetAtk((int)(player.GetAtk() + value)); break;
                case ItemDB.DEF: player.SetDef((int)(player.GetDef() + value)); break;
                case ItemDB.HP: player.SetHp((int)(player.GetMaxHp() + value)); break;
                case ItemDB.HP_RATE: player.SetHp(player.GetMaxHp() + (int)(player.GetOriginStatus().BufferHp * value)); break;
                case ItemDB.CRI_RATE: player.SetCriRate((int)(player.GetCriRate() + value)); break;
                case ItemDB.CRI_DMG: player.SetCriDamage((int)(player.GetCriDmg() + value)); break;
                case ItemDB.AVD_RATE: player.SetAvoidance((int)(player.GetAvdRate() + value)); break;
                case ItemDB.REC: player.SetRecoverHp((int)(player.GetRecoverHp() + value)); break;
                case ItemDB.REC_TIME: player.SetRecoverTime((int)(player.GetRecoverTime() + value)); break;
                case ItemDB.EXP_RATE: break;
                case ItemDB.GOLD_RATE: break;
                //case ItemDB.ATK_SPD_RATE: break;  // 공속 증가
            }
        }
    }

    /// <summary>
    /// characterID가 장착한 슬롯 정보를 가져옴
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public EquipList GetEquipList(int characterId)
    {
        return _equipList[characterId];
    }



    public void SaveEquipSlot()
    {
        Debug.Log(_equipList.Length);
        SaveData<EquipList[]>.DataSave(_equipList, path);
    }

    public void LoadEquipSlot()
    {
        EquipList[] equipList = SaveData<EquipList[]>.DataLoad(path);

        if(equipList != null)
        {
            _equipList = equipList;
            // 장비 로드 후, 장비 스텟 반영
            for (int i = 0; i < _equipList.Length; i++)
            {
                for (int k = 0; k < _equipList[i].equipSlots.Length; k++)
                {
                    if (_equipList[i].equipSlots[k] > 0)
                    {
                        ApplyEquipItemOption(i, _equipList[i].equipSlots[k]);
                    }

                }
            }
        }
        else
        {
            SaveEquipSlot();
        }

    }
}
