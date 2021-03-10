using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneInvenMenu : MonoBehaviour
{
    static readonly int RUNE = 0, CHARM = 1;

    // UI
    [SerializeField] GameObject _goUI = null;
    [SerializeField] GameObject _choiceRuneNumUI = null;
    
    // 룬 슬롯
    [SerializeField] EquipItemSlot[] _runeEquipSlots = null;

    // 룬 확인 UI
    [Header("Rune Choice UI")]
    [Header("ChoiceSlotUI")]
    [SerializeField] Image _imgChoiceRuneSprite = null;
    [SerializeField] Text _txtChoiceRuneName = null;
    [SerializeField] Text _txtChoiceRuneDesc = null;
    [SerializeField] Text _txtChoiceRuneOption = null;

    [Header("EquipSlotUI")]
    [SerializeField] Image _imgSlotOne = null;
    [SerializeField] Image _imgSlotTwo = null;
    [SerializeField] Text _txtSlotOneOption = null;
    [SerializeField] Text _txtSlotTwoOption = null;


    // 인벤 슬롯
    [SerializeField] InvenRuneSlot[] _slots = null;

    int _curCharacterID;            // 선택한 캐릭터의 ID
    int _choiceRuneID;              // 선택한 룬의 ID 
    int _curTab;                    // 현재 탭 번호

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

    // 룬 인벤 보여주기
    public void ShowRuneInven(int characterID)
    {
        _curCharacterID = characterID;      // 선택했던 캐릭터 ID 캐싱
        RuneEquipSlotClear();               // 룬 슬롯 초기화
        SettingSlot(RUNE);                  // 인벤 룬 세팅
        RenewRuneEquipSlot();               // 장착한 룬 반영
    }

    // 장착 슬롯 초기화
    void RuneEquipSlotClear()
    {
        for (int i = 0; i < _runeEquipSlots.Length; i++)
        {
            _runeEquipSlots[i].ClearSlot();
        }
    }

    // 인벤 슬롯 초기화
    void AllInvenSlotClear()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].ClearSlot();
            _slots[i].HideButton();
        }
    }


    // 장착한 아이템 반영
    void RenewRuneEquipSlot()
    {
        EquipList equipList = _equip.GetEquipList(_curCharacterID);
        _runeEquipSlots[0].SetSlot(equipList.equipSlots[EquipManager.RUNE_ONE]);
        _runeEquipSlots[1].SetSlot(equipList.equipSlots[EquipManager.RUNE_TWO]);
    }

    // 슬롯 세팅
    void SettingSlot(int value)
    {
        _curTab = value;
        AllInvenSlotClear();

        string itemType = "";

        // 해당 캐릭터 전용 무기 타입 GET
        if (_curTab == RUNE)
            itemType = ItemDB.RUNE;
        // 해당 캐릭터 전용 갑옷 타입 GET
        else if (_curTab == CHARM)
            itemType = ItemDB.CHARM;

        // 해당 타입의 장비를 인벤토리에서 가져옴.
        List<int> items = _inven.GetItemTypeList(itemType);

        // 인벤토리에서 가져온 게 있다면
        if (items.Count > 0)
        {
            // 그 개수만큼 슬롯 세팅
            for (int i = 0; i < items.Count; i++)
            {
                _slots[i].SetSlot(items[i], this);

                if (value == RUNE)
                    _slots[i].ShowButton();
                else
                    _slots[i].HideButton();
            }
        }

    }

    /// <summary>
    /// 어느 룬 슬롯에 장착할지 초이스
    /// </summary>
    /// <param name="runeId"></param>
    public void ShowWhichRuneChoiceWindow(int runeId)
    {
        if(runeId != 0)
        {
            // UI 출력
            _choiceRuneID = runeId;
            _choiceRuneNumUI.SetActive(true);

            // 선택한 룬 세팅
            Item rune = ItemDB.GetItem(runeId);
            _imgChoiceRuneSprite.sprite = rune.sprite;
            _txtChoiceRuneName.text = rune.name;
            _txtChoiceRuneDesc.text = rune.desc;
            _txtChoiceRuneOption.text = rune.optionDesc;

            // 장착중인 룬 세팅
            int[] equipIDList = _equip.GetEquipList(_curCharacterID).equipSlots;
            Item runeSlotOneItem = ItemDB.GetItem(equipIDList[EquipManager.RUNE_ONE]);
            Item runeSlotTwoItem = ItemDB.GetItem(equipIDList[EquipManager.RUNE_TWO]);

            if (runeSlotOneItem != null)
            {
                _imgSlotOne.gameObject.SetActive(true);
                _imgSlotOne.sprite = runeSlotOneItem.sprite;
                _txtSlotOneOption.text = runeSlotOneItem.optionDesc;
            }
            else
            {
                _imgSlotOne.gameObject.SetActive(false);
                _txtSlotOneOption.text = "-";
            }
            if (runeSlotTwoItem != null)
            {
                _imgSlotTwo.gameObject.SetActive(true);
                _imgSlotTwo.sprite = runeSlotTwoItem.sprite;
                _txtSlotTwoOption.text = runeSlotTwoItem.optionDesc;
            }
            else
            {
                _imgSlotTwo.gameObject.SetActive(false);
                _txtSlotTwoOption.text = "-";
            }
        }
    }




    /// <summary>
    /// 첫번째 룬 슬롯에 장착
    /// </summary>
    public void OnClickRuneOneSlot()
    {
        if (_choiceRuneID != 0)
        {
            // 인벤 감소
            _inven.DecreaseItem(_choiceRuneID);

            // 기존에 장착된 게 있다면 인벤에 푸시
            int popItemID = _equip.SetEquipSlot(_curCharacterID, _choiceRuneID, EquipManager.RUNE_ONE);
            if (popItemID > 0)
                _inven.AcquireItem(popItemID);

            // 슬롯 다시 반영
            RenewSlot();
        }
    }

    /// <summary>
    /// 두번째 룬 슬롯에 장착
    /// </summary>
    public void OnClickRuneTwoSlot()
    {
        _inven.DecreaseItem(_choiceRuneID);

        int popItemID = _equip.SetEquipSlot(_curCharacterID, _choiceRuneID, EquipManager.RUNE_TWO);
        if(popItemID > 0)
            _inven.AcquireItem(popItemID);
        RenewSlot();
    }

    // 장착 반영
    void RenewSlot()
    {
        _inven.DecreaseItem(_choiceRuneID, 1);   // 인벤에 룬 감소 적용
        SettingSlot(_curTab);                    // 인벤 재반영
        RenewRuneEquipSlot();                    // 룬 장착 반영
        _characterMenu.RenewEquipSlot();         // 상위 캐릭터 메뉴에 룬 장착 반영
        _choiceRuneNumUI.SetActive(false);       // 선택 윈도우 숨김
    }
    
    /// <summary>
    /// 장착 취소
    /// </summary>
    public void OnClickRuneChoiceBack()
    {
        _choiceRuneNumUI.SetActive(false);
    }

    /// <summary>
    /// 부적 소지품 보여주기
    /// </summary>
    public void OnClickCharmTab()
    {
        SettingSlot(CHARM);
    }

    /// <summary>
    /// 룬 소지품 보여주기
    /// </summary>
    public void OnClickRuneTab()
    {
        SettingSlot(RUNE);
    }

    /// <summary>
    /// 뒤로가기
    /// </summary>
    public void OnClickBackButton()
    {
        _goUI.SetActive(false);
    }

}
