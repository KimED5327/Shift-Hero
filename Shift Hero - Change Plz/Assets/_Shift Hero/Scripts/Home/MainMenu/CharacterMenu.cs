using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    static readonly int CENTER = 1, RIGHT = 2;

    // 선택 슬롯 하이라이트용
    [SerializeField] GameObject _goChoiceRect = null;

    // 캐릭터 선택 슬롯
    [SerializeField] CharacterSlot[] _characterSlots = null;
    [SerializeField] Transform[] _tfSlotPos = null;


    // 장착한 장비와 룬 슬롯
    [SerializeField] Image[] _imgEquipSlots = null;

    // 추가 UI
    [SerializeField] Text _txtCharacterInfo = null;
    [SerializeField] GameObject _goDetailInfo = null;
    [SerializeField] Text _txtDetailLeft = null;
    [SerializeField] Text _txtDetailRight = null;

    [SerializeField] GameObject _goEquipInventory = null;
    [SerializeField] GameObject _goRuneInventory = null;
    [SerializeField] GameObject _goCharacterShop = null;


    int _choiceSlotIndex;

    EquipInvenMenu _equipInvenMenu;
    RuneInvenMenu _runeInvenMenu;
    WorldMenu _worldMenu;
    CharacterShopMenu _charShopMenu;

    CharacterManager _characterManager;
    EquipManager _equip;


    private void Awake()
    {
        _equipInvenMenu = FindObjectOfType<EquipInvenMenu>();
        _characterManager = FindObjectOfType<CharacterManager>();
        _equip = FindObjectOfType<EquipManager>();
        _worldMenu = FindObjectOfType<WorldMenu>();
        _runeInvenMenu = FindObjectOfType<RuneInvenMenu>();
        _charShopMenu = FindObjectOfType<CharacterShopMenu>();
    }

    private void Start()
    {
        _choiceSlotIndex = 0;

        StandBySlotSetting();           // 스탠바이 슬롯 세팅
        SettingChoiceCharacterInfo();   // 선택된 캐릭터 정보 세팅
        RenewEquipSlot();
    }

    // 선택한 캐릭터 정보 띄우기
    void SettingChoiceCharacterInfo()
    {
        PlayerData data = PlayerDB.GetPlayerData(_characterSlots[_choiceSlotIndex].GetID());

        _txtCharacterInfo.text = $"{data.name} Lv. {data.level}\n({data.job})";

        ShowCharcaterDetailInfo();
    }

    /// <summary>
    /// 스탠바이 슬롯 세팅
    /// </summary>
    public void StandBySlotSetting()
    {
        int[] ids = _characterManager.GetStandbyCharactersID();
        for (int i = 0; i < _characterSlots.Length; i++)
        {
            _characterSlots[i].SetSlot(ids[i], this);
        }

        for (int i = 0; i < _characterSlots.Length; i++)
        {
            _worldMenu.SettingCharcaterSprite(i, _characterSlots[i].GetSprite());
        }

        SettingChoiceCharacterInfo();   // 선택된 캐릭터 정보 세팅
    }

    /// <summary>
    /// 선택한 캐릭터의 장착 슬롯 변경사항 재반영
    /// </summary>
    public void RenewEquipSlot()
    {
        EquipList equipList = _equip.GetEquipList(_characterSlots[_choiceSlotIndex].GetID());
        for(int i = 0; i < _imgEquipSlots.Length; i++)
        {
            if(equipList.equipSlots[i] > 0)
            {
                _imgEquipSlots[i].sprite = SpriteDB.GetItemSprite(equipList.equipSlots[i]);
                _imgEquipSlots[i].gameObject.SetActive(true);
            }
            else
            {
                _imgEquipSlots[i].gameObject.SetActive(false);
            }
        }

        ShowCharcaterDetailInfo();
    }

    /// <summary>
    /// 슬롯 클릭시, 선택 연출
    /// </summary>
    /// <param name="value"></param>
    public void OnClickSlot(int value)
    {
        _goChoiceRect.SetActive(true);
        _goChoiceRect.transform.position = _tfSlotPos[value].position;
        _choiceSlotIndex = value;
        SettingChoiceCharacterInfo();   // 선택한 캐릭터 정보 재반영
        RenewEquipSlot();               // 선택한 슬롯의 캐릭터 장비 반영
    }

    /// <summary>
    /// 1-2 슬롯  교체
    /// </summary>
    public void OnClickChangeLeft()
    {
        ChangeSlot(CENTER);
    }


    /// <summary>
    /// 2-3 슬롯  교체
    /// </summary>
    public void OnClickChangeRight()
    {
        ChangeSlot(RIGHT);
    }

    // 슬롯 교체
    void ChangeSlot(int defaultValue)
    {
        // 슬롯간 교체
        int tempID = _characterSlots[defaultValue].GetID();
        _characterSlots[defaultValue].SetSlot(_characterSlots[defaultValue - 1].GetID(), this);
        _characterSlots[defaultValue - 1].SetSlot(tempID, this);

        // 매니저에 반영
        _characterManager.SetStandBySlot(defaultValue, _characterSlots[defaultValue].GetID());
        _characterManager.SetStandBySlot(defaultValue - 1, _characterSlots[defaultValue - 1].GetID());

        StandBySlotSetting();   // 스탠바이 슬롯 재반영
        RenewEquipSlot();       // 선택된 캐릭터 장비 슬롯 재반영
        SettingChoiceCharacterInfo();   // 선택된 캐릭터 정보 재반영
    }

    /// <summary>
    /// 캐릭터 디테일 스테이터스 노출
    /// </summary>
    public void OnClickDetailInfo()
    {
        _goDetailInfo.SetActive(true);
    }

    /// <summary>
    /// 캐릭터 디테일 수치 보여주기
    /// </summary>
    public void ShowCharcaterDetailInfo()
    {
        Player player = _characterManager.GetPlayerFromID(_characterSlots[_choiceSlotIndex].GetID());

        _txtDetailLeft.text = $"{player.GetMaxHp()}\n{player.GetAtk()}\n{player.GetDef()}\n{player.GetRecoverHp()}";
        _txtDetailRight.text = $"{player.GetCriRate() * 100f}%\n{player.GetCriDmg() * 100f}%\n{player.GetAvdRate() * 100f}%\n{player.GetMoveSpeed()}";
    }

    /// <summary>
    /// 캐릭터 디테일 스테이터스 제거
    /// </summary>
    public void OnClickRemoveDetail()
    {
        _goDetailInfo.SetActive(false);
    }

    /// <summary>
    /// 장비 인벤토리 노출
    /// </summary>
    public void OnClickChangeEquip()
    {
        _goEquipInventory.SetActive(true);

        _equipInvenMenu.ShowEquipInven(_characterSlots[_choiceSlotIndex].GetID());
    }


    /// <summary>
    /// 부적 변경 창 노출
    /// </summary>
    public void OnClickChangeRune()
    {
        _goRuneInventory.SetActive(true);

        _runeInvenMenu.ShowRuneInven(_characterSlots[_choiceSlotIndex].GetID());
    }

    /// <summary>
    /// 캐릭터 변경 창 노출
    /// </summary>
    public void OnClickChangeCharacter()
    {
        _goCharacterShop.SetActive(true);

        _charShopMenu.ShowMenu(_choiceSlotIndex, _characterSlots[_choiceSlotIndex].GetID());
    }
}
