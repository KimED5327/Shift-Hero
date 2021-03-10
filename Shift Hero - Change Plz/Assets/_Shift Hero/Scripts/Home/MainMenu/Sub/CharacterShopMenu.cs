using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShopMenu : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] GameObject _goUI = null;
    [SerializeField] CharacterSwapSlot[] _slots = null;
    [SerializeField] Image _imgCharacter = null;
    [SerializeField] Text _txtInfo = null;

    [Header("Buy Character")]
    [SerializeField] GameObject _goBuyUI = null;
    [SerializeField] Image _imgBuyCharacter = null;
    [SerializeField] Text _txtBuyCharName = null;
    [SerializeField] Text _txtBuyCharClass = null;
    [SerializeField] Text _txtBuyCharDesc = null;
    [SerializeField] Text _txtBuyCharPrice = null;


    int _enrollIndex;               // 스탠바이 슬롯 인덱스
    int _curChoiceCharacterID;      // 선택한 캐릭터의 ID값
    int _buyID;                     // 구매할 캐릭터의 ID값

    // 필요 컴포넌트
    CharacterManager _characterManager;
    CharacterMenu _characterMenu;
    WorldMenu _worldMenu;

    private void Awake()
    {
        _characterManager = FindObjectOfType<CharacterManager>();
        _characterMenu = FindObjectOfType<CharacterMenu>();
        _worldMenu = FindObjectOfType<WorldMenu>();
    }

    /// <summary>
    /// 캐릭터 교체, 구매 메뉴 호출
    /// </summary>
    /// <param name="id"></param>
    public void ShowMenu(int curIndex, int id)
    {
        _enrollIndex = curIndex;        // 스탠바이 슬롯 인덱스 캐싱
        _curChoiceCharacterID = id;     // id 캐싱

        ChoiceCharacterSlotSetting();   // 선택한 캐릭터 세팅
        SettingSlot();                  // 캐릭터 슬롯들 세팅
    }

    // 선택된 캐릭터 슬롯 정보 세팅
    void ChoiceCharacterSlotSetting()
    {
        PlayerData playerData = PlayerDB.GetPlayerData(_curChoiceCharacterID);

        _imgCharacter.sprite = playerData.sprite;
        _txtInfo.text = $"{playerData.name} {playerData.job} (Lv. {playerData.level}";
    }

    // 캐릭터 슬롯들 세팅
    void SettingSlot()
    {
        int[] standByCharIDs = _characterManager.GetStandbyCharactersID();

        // 슬롯 번호와 플레이어 ID 값을 매치해서 세팅
        for (int i = 0, id = 0; i < _slots.Length; i++, id++)
        {
            _slots[i].SetSlot(id, this);
        }

        // 이미 스탠바이 중인 캐릭터는 교체 불가 처리
        for(int i = 0; i <standByCharIDs.Length; i++)
        {
            _slots[standByCharIDs[i]].StandbyCharacter();
        }
    }


    /// <summary>
    /// 버튼 호출
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isBuy"></param>
    public void CallSlot(int id, bool isBuy)
    {
        // 교체
        if (!isBuy)
        {
            _characterManager.SetStandBySlot(_enrollIndex, id);  // 스탠 바이 캐릭터 교체
            _characterMenu.StandBySlotSetting();                // 상위 메뉴도 스탠바이 슬롯 재반영

            _goUI.SetActive(false);
        }
        // 구입 UI 활성화
        else
        {
            _buyID = id;
            _goBuyUI.SetActive(true);
            PlayerData pData = PlayerDB.GetPlayerData(_buyID);
            _imgBuyCharacter.sprite = pData.sprite;
            _txtBuyCharName.text = pData.name;
            _txtBuyCharClass.text = pData.job;
            _txtBuyCharDesc.text = pData.desc.Replace("#", "\n");
            _txtBuyCharPrice.text = $"{string.Format("{0:###,0}", pData.price)} G";
        }
    }

    /// <summary>
    /// 구입 시도
    /// </summary>
    public void OnClickBuyBtn()
    {
        // 골드 소모 필요

        PlayerDB.SetBuyCharacterID(_buyID); // 구입 완료
        SettingSlot();                      // 슬롯 재세팅
        _goBuyUI.SetActive(false);          // 구매창 비활성화
    }

    /// <summary>
    /// 구입 취소
    /// </summary>
    public void OnClickCancelBtn()
    {
        _goBuyUI.SetActive(false);          // 구매창 비활성화
    }

    public void OnClickBackBtn()
    {
        _goUI.SetActive(false);
    }
}
