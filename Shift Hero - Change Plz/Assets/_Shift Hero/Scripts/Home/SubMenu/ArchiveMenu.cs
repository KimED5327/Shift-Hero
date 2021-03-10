using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveMenu : MonoBehaviour
{
    const int MONSTER = 0, ITEM = 1, CHARACTER = 2, ARCHIEVE = 3;
    const int SLOT_MAX_COUNT = 8;

    [Header("Self")]
    [SerializeField] GameObject _goUI = null;
    [SerializeField] GameObject _goChoiceHighlight = null;
    [SerializeField] Transform[] _tfButtonPos = null;
    [SerializeField] Text _txtPage = null;

    [Header("Book")]
    [SerializeField] GameObject _goBookPanel = null;
    [SerializeField] BookSlot[] _bookSlots = null;

    [Header("Achieve")]
    [SerializeField] GameObject _goAchievePanel = null;
    [SerializeField] AchievementSlot[] _achieveSlot = null;


    int _curTabIndex = 0;
    int _curPage = 0;
    int _maxPage = 0;
    int _curMaxElementCount = 0;
    int _maxElementCount = 0;

    Inventory _inven;

    void Awake()
    {
        _inven = FindObjectOfType<Inventory>();
    }

    public void ShowMenu()
    {
        _goUI.SetActive(true);
        OnClickTab(0);
    }

    /// <summary>
    /// 탭 클릭
    /// </summary>
    /// <param name="index"></param>
    public void OnClickTab(int index)
    {
        // 하이라이트 연출 이동
        _goChoiceHighlight.transform.localPosition = _tfButtonPos[index].localPosition;

        _curPage = 0;
        _curTabIndex = index;

        SettingPanel();     // 패널 활성화 분류
        SettingMaxPage();   // 최대 페이지 세팅
        SettingSlot();      // 슬롯 세팅
    }

    // 분류값에 맞는 패널 활성화
    void SettingPanel()
    {
        // 도감 활성화
        if (_curTabIndex < ARCHIEVE)
        {
            _goBookPanel.SetActive(true);
            _goAchievePanel.SetActive(false);
        }
        // 도전과제 활성화
        else
        {
            _goAchievePanel.SetActive(true);
            _goBookPanel.SetActive(false);
        }
    }

    // 최대 요소 개수 및 최대 페이지 수 세팅
    void SettingMaxPage()
    {
        switch (_curTabIndex)
        {
            case MONSTER:
                _maxElementCount = MonsterDB.GetMonsterMaxCount();
                _curMaxElementCount = AchieveDB.GetMonsterGetListCount();
                break;
            case ITEM:
                _maxElementCount = ItemDB.GetItemMaxCount();
                _curMaxElementCount = AchieveDB.GetItemGetListCount();
                break;
            case CHARACTER:
                _maxElementCount = PlayerDB.GetPlayerMaxCount();
                _curMaxElementCount = AchieveDB.GetPlayerGetCount();
                break;
            case ARCHIEVE:
                _maxElementCount = AchieveDB.GetAchieveMaxCount();
                _curMaxElementCount = AchieveDB.GetAchieveMaxCount();
                break;
        }

        _maxPage = (_maxElementCount - 1) / SLOT_MAX_COUNT;
    }

    // 슬롯 세팅 분류
    void SettingSlot()
    {
        // 도감 슬롯 세팅
        if (_curTabIndex < ARCHIEVE)
        {
            SettingBookSlot();
        }
        // 도전과제 슬롯 세팅
        else
        {
            SettingAchieveSlot();
        }
    }


    // 도감 세팅
    void SettingBookSlot()
    {
        for(int i = 0; i < SLOT_MAX_COUNT; i++)
        {
            int id = i + (SLOT_MAX_COUNT * _curPage);

            // id값이 최대갯수보다 작으면 슬롯 세팅
            // 그렇지 않으면 슬롯 제거
            if (id < _curMaxElementCount)
            {
                _bookSlots[i].SetType((BookSlot.BookType)_curTabIndex, id, this);
                _bookSlots[i].gameObject.SetActive(true);
            }
            else if (id < _maxElementCount)
            {
                _bookSlots[i].ClearSlot();
                _bookSlots[i].gameObject.SetActive(true);
            }
            else
                _bookSlots[i].gameObject.SetActive(false);
        }

        _txtPage.text = $"{_curPage + 1} / {_maxPage + 1}";
    }


    // 도전 과제 세팅
    void SettingAchieveSlot()
    {
        for (int i = 0; i < SLOT_MAX_COUNT; i++)
        {
            int id = i + (SLOT_MAX_COUNT * _curPage);

            // id값이 최대갯수보다 작으면 슬롯 세팅
            // 그렇지 않으면 슬롯 제거
            if (id < _curMaxElementCount)
            {
                _achieveSlot[i].gameObject.SetActive(true);
                _achieveSlot[i].SetSlot(id, this);
            }
            else
                _achieveSlot[i].gameObject.SetActive(false);
        }

        _txtPage.text = $"{_curPage + 1} / {_maxPage + 1}";
    }

    /// <summary>
    /// 이전 페이지로
    /// </summary>
    public void OnClickPriorPage()
    {
        if (--_curPage < 0)
            _curPage = 0;

        SettingSlot();
    }

    /// <summary>
    /// 다음 페이지로
    /// </summary>
    public void OnClickNextPage()
    {
        if (++_curPage > _maxPage)
            _curPage = _maxPage;

        SettingSlot();
    }

    /// <summary>
    /// 서브 메뉴 닫기
    /// </summary>
    public void OnClickBack()
    {
        _goUI.SetActive(false);
    }



    /// <summary>
    /// 도전과제 보상 획득
    /// </summary>
    /// <param name="id"></param>
    public void OnCallAchieveRewardBtn(int id)
    {
        // 보상 획득
        List<AchieveReward> rewardList = AchieveDB.GetAchieveDetail(id, AchieveDB.GetAchieve(id).clearCount).rewards;
        for (int i = 0; i < rewardList.Count; i++)
        {
             _inven.AcquireItem(rewardList[i].rewardID, rewardList[i].rewardCount);
        }


        // 클리어 처리
        AchieveDB.AchieveClear(id);
        
        // 도전과제 슬롯 리셋팅
        SettingAchieveSlot();
        MyInfo.CallMyInfoChange();  // HUD 반영
    }
}
