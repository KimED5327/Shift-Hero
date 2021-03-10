using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInvenMenu : MonoBehaviour
{
    [SerializeField] GameObject _goUI = null;

    // 장비 슬롯
    [SerializeField] EquipItemSlot[] _equipSlots = null;

    // 인벤 슬롯
    [SerializeField] InvenEquipSlot[] _slots = null;


    int _curCharacterID;            // 선택한 캐릭터의 ID
    int _curTab;                    // 선택한 현재 탭

    // 필요 컴포넌트
    Inventory _inven;
    EquipManager _equip;
    CharacterMenu _characterMenu;

    private void Awake()
    {
        _inven = FindObjectOfType<Inventory>();
        _equip = FindObjectOfType<EquipManager>();
        _characterMenu = FindObjectOfType<CharacterMenu>();
    }

    // 장비 인벤 슬롯 보여주기
    public void ShowEquipInven(int characterID)
    {
        _curCharacterID = characterID;      // 현재 선택된 캐릭터 ID 캐싱
        _curTab = EquipManager.WEAPON;      // 무기부터 출력

        EquipSlotClear();                   // 초기화
        SettingSlot();                      // 인벤 슬롯 세팅
        RenewEquipSlot();                   // 장착 슬롯 세팅
    }
    
    // 장착한 아이템 반영
    void RenewEquipSlot()
    {
        EquipList equipList = _equip.GetEquipList(_curCharacterID);
        _equipSlots[EquipManager.WEAPON].SetSlot(equipList.equipSlots[EquipManager.WEAPON], this);
        _equipSlots[EquipManager.ARMOR].SetSlot(equipList.equipSlots[EquipManager.ARMOR], this);
    }

    // 장착 슬롯 초기화
    void EquipSlotClear()
    {
        for(int i = 0; i < _equipSlots.Length; i++)
        {
            _equipSlots[i].ClearSlot();
        }
    }

    // 인벤 슬롯 초기화
    void AllInvenSlotClear()
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].ClearSlot();
        }
    }


    // 슬롯 세팅
    void SettingSlot()
    {
        AllInvenSlotClear();

        string equipType = "";

        // 해당 캐릭터 전용 무기 타입 GET
        if(_curTab == EquipManager.WEAPON)
            equipType = ItemDB.GetWeaponTypeFromCharacterID(_curCharacterID);
        // 해당 캐릭터 전용 갑옷 타입 GET
        else if (_curTab == EquipManager.ARMOR)
            equipType = ItemDB.GetArmorTypeFromCharacterID(_curCharacterID);

        // 해당 타입의 장비를 인벤토리에서 가져옴.
        List<int> items = _inven.GetEquipItem(equipType);

        // 인벤토리에서 가져온 게 있다면
        if(items.Count > 0)
        {
            // 그 개수만큼 슬롯 세팅
            for(int i = 0; i < items.Count; i++)
            {
                _slots[i].SetSlot(items[i], this);
            }
        }

    }

    // 인벤 슬롯에서 장착 클릭
    public void OnClickEquipButton(int itemId)
    {
        if(itemId > 0)
        {
            _inven.DecreaseItem(itemId);     // 인벤에서 감소
            int popItemID = _equip.SetEquipSlot(_curCharacterID, itemId, _curTab); // 장착시도
            if (popItemID > 0)                  // 장착 후 교체된 아이템이 있다면
                _inven.AcquireItem(popItemID);  // 인벤에 푸시
            SettingSlot();                      // 인벤 슬롯 재세팅
            RenewEquipSlot();                   // 장착 슬롯 반영
            _characterMenu.RenewEquipSlot();    // 캐릭터 메뉴에도 반영
        }

    }

    // 무기 정렬
    public void OnClickWeaponTab()
    {
        _curTab = EquipManager.WEAPON;
        SettingSlot();
    }

    // 방어구 정렬
    public void OnClickArmorTab()
    {
        _curTab = EquipManager.ARMOR;
        SettingSlot();
    }

    // 뒤로가기
    public void OnClickBackButton()
    {
        _goUI.SetActive(false);
    }

}
