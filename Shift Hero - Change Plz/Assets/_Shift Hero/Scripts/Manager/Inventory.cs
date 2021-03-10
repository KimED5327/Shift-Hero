using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InvenItem
{
    public int id;
    public int count;
}

[System.Serializable]
public class ETC
{
    public int gold;
    public int clover;
}

public class Inventory : MonoBehaviour
{
    static readonly string savePath = "/Inventory.dat"; // 저장 경로
    static readonly string saveCurrancyPath = "/Currency.dat"; // 저장 경로

    static readonly int MAX_COUNT = 99; // 슬롯 최대 중첩 개수

    int _gold = 50;                 // 재화
    int _curClover = 10;            // 클로버 (모두 소진시 경험치 획득, 골드 획득량 절반 감소)
    int _maxClover = 10;            // 최대 자동 충전 클로버 개수

    // 인벤 아이템 리스트 (무제한)
    List<InvenItem> _invenItems = new List<InvenItem>();

    MyInfo _myInfo; public void LinkMyInfo(MyInfo info) => _myInfo = info;

    void Awake()
    {
        LoadInventoryItems();
        LoadCurrency();
    }

    /// <summary>
    /// 아이템 획득시 호출. 획득한 아이템 ID와 그 개수를 전달
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void AcquireItem(int id, int count = 1)
    {
        // 골드 증가
        if (id == ItemDB.GOLD) { IncreaseGold(count); return; }

        // 경험치 증가
        else if (id == ItemDB.EXP) { _myInfo.IncreaseExp(count); return; }

        // 부적 종류면 능력 증가
        else if (ItemDB.IsSameItemType(id, ItemDB.CHARM)) {
            Item item = ItemDB.GetItem(id);
            for (int i = 0; i < item.options.Count; i++)
            {
                BonusAbility.ApplyBonusAbility(item.options[i].optionType, item.options[i].num);
            }
        }


        // 중첩 가능한 아이템이면
        if (ItemDB.GetItem(id).isStockable)
        {
            // 인벤토리부터 검색
            InvenItem item = _invenItems.Find(it => it.id == id && it.count < MAX_COUNT);

            // 인벤에 동일한 아이템이 있다면
            if (item != null)
            {
                // 더했을 경우의 MAX 오버플로우를 체크
                int overFlowCount = (item.count + count) - MAX_COUNT;

                // 오버플로우가 있다면
                if (overFlowCount > 0)
                {
                    // MAX로 변경 후 오버플로우를 새로운 인벤 칸에 재할당
                    item.count = MAX_COUNT;
                    _invenItems.Add(new InvenItem { id = id, count = overFlowCount });
                }
                // 오버플로우가 없다면
                else
                {
                    // 갯수 증가
                    item.count += count;
                }
            }
            // 인벤에 없다면
            else
            {
                // 새로 등록
                _invenItems.Add(new InvenItem { id = id, count = count });
            }

        }
        // 중첩 불가능한 아이템이면
        else
        {
            // 개수만큼 새로 할당
            for (int i = 0; i < count; i++)
            {
                _invenItems.Add(new InvenItem { id = id, count = count });
            }
        }

        // (업적용) 획득 아이템 추가
        AchieveDB.AddGetItemList(id);

        // 데이터 저장
        SaveInventoryItems();
    }

    // 특정 장비 유형만 가져오기
    public List<int> GetEquipItem(string equipType)
    {
        List<int> equipItems = new List<int>();
        for (int i = 0; i < _invenItems.Count; i++)
        {
            if (ItemDB.IsSameEquipType(_invenItems[i].id, equipType))
            {
                equipItems.Add(_invenItems[i].id);
            }
        }

        return equipItems;
    }

    // 특정 아이템 유형들만 가져오기
    public List<int> GetItemTypeList(string itemType)
    {
        List<int> itemtypeList = new List<int>();
        for (int i = 0; i < _invenItems.Count; i++)
        {
            if (ItemDB.IsSameItemType(_invenItems[i].id, itemType))
            {
                itemtypeList.Add(_invenItems[i].id);
            }
        }

        return itemtypeList;
    }

    /// <summary>
    /// 인벤 아이템 감소 (0개 되면 삭제)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void DecreaseItem(int id, int count = 1)
    {
        for (int i = 0; i < _invenItems.Count; i++)
        {
            if (_invenItems[i].id == id)
            {
                _invenItems[i].count -= count;
                if (_invenItems[i].count == 0)
                {
                    _invenItems.RemoveAt(i);
                }
            }
        }
        SaveInventoryItems();
    }



    // 골드 리턴
    public int GetGold() { return _gold; }
    // 골드 세팅
    public void SetGold(int gold)
    {
        _gold = gold;
        SaveCurrency();
    }
    // 골드 증가
    public void IncreaseGold(int value) {
        _gold += value;
        AchieveDB.IncreaseGoldCount(value);
        SaveCurrency();
    }

    /// <summary>
    /// value만큼 골드 감소
    /// </summary>
    /// <param name="value"></param>
    public void DecreaseGold(int value) {
        _gold -= value;
        if (_gold < 0) _gold = 0;
        SaveCurrency();
    }

    /// <summary>
    /// 골드가 Value 값 이상인지 체크
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsEnoughGold(int value) { return _gold >= value; }

    // 클로버 개수 리턴
    public int GetClover() { return _curClover; }

    // 클로버 증가
    public void IncreaseClover(int value = 1) {
        _curClover += value;
        if (_curClover > _maxClover)
            _curClover = _maxClover;
        SaveCurrency();
    }

    // 클로버 감소
    public void DecreaseClover(int value = 1) {
        _curClover -= value;
        if (_curClover < 0)
            _curClover = 0;
        SaveCurrency();
    }
    // 클로버 최대개수 리턴
    public int GetMaxClover() { return _maxClover; }

    // 인벤토리 내용 저장과 로드
    public void SaveInventoryItems() => SaveData<List<InvenItem>>.DataSave(_invenItems, savePath);
    public void LoadInventoryItems() 
    {
        _invenItems = SaveData<List<InvenItem>>.DataLoad(savePath);
        if (_invenItems == null)
            SaveInventoryItems();
    }
    public void SaveCurrency()
    {
        ETC etc = new ETC()
        {
            gold = _gold,
            clover = _curClover
        };
        SaveData<ETC>.DataSave(etc, saveCurrancyPath);
    }
    public void LoadCurrency()
    {
        ETC etc = SaveData<ETC>.DataLoad(saveCurrancyPath);
        if (etc != null)
        {
            _gold = etc.gold;
            _curClover = etc.clover;
        }
        else
            SaveCurrency();
    }
}
