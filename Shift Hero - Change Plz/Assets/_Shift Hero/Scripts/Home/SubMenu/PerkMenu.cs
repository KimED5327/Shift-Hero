using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkInfo
{
    public int priorID;     // 선행조건 ID값
    public int id;
    public string name;
    public string desc;
    public Sprite sprite;
    public int sp;
    public bool isLearn;
    public string option;
    public float optionNum;

    public PerkInfo(int priorID, int id, string name, string desc, Sprite sprite, int sp, bool isLearn, string option, float optionNum)
    {
        this.priorID = priorID;
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.sprite = sprite;
        this.sp = sp;
        this.isLearn = isLearn;
        this.option = option;
        this.optionNum = optionNum;
    }
}

public class PerkMenu : MonoBehaviour
{
    static readonly string path = "/perk.dat";
    static PerkInfo[] _perkInfo;

    [Header("Self UI")]
    [SerializeField] GameObject _goUI = null;
    [SerializeField] Text _txtLeftSP = null;
    [SerializeField] PerkSlot[] _slots = null;

    [Header("Learning UI")]
    [SerializeField] GameObject _goLearningUI = null;
    [SerializeField] Text _txtLearnLeftSP = null;
    [SerializeField] Text _txtLearnName = null;
    [SerializeField] Text _txtLearnDesc = null;
    [SerializeField] Text _txtLearnSP = null;
    [SerializeField] Image _imgLearnSprite = null;

    PerkInfo _learnPerk;
    MyInfo _myInfo;

    private void Awake()
    {
        _myInfo = FindObjectOfType<MyInfo>();
    }

    /// <summary>
    /// 퍽 세팅
    /// </summary>
    /// <param name="perks"></param>
    public void SetPerk(PerkInfo[] perks)
    {
        _perkInfo = perks; // 캐싱
        LoadPerkData(); // 학습 정보 로드
    }

    /// <summary>
    /// 특정 ID 값의 퍽을 가져옴
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PerkInfo GetPerkInfo(int id) {
        

        for(int i = 0; i < _perkInfo.Length; i++)
        {
            if(_perkInfo[i].id == id)
            {
                return _perkInfo[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 메뉴 호출
    /// </summary>
    public void ShowMenu()
    {
        _txtLeftSP.text = $"SP {_myInfo.GetSkillPoint()}";
        SettingSlot();
        _goUI.SetActive(true);
    }

    /// <summary>
    /// 메뉴 닫기
    /// </summary>
    public void HideMenu()
    {
        _goUI.SetActive(false);
    }

    /// <summary>
    ///  퍽 슬롯 세팅
    /// </summary>
    void SettingSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetPerkSlot(_perkInfo[i], this);
        }
        _txtLeftSP.text = $"SP {_myInfo.GetSkillPoint()}";
    }

    /// <summary>
    /// 슬롯에서 부모 메뉴 호출
    /// </summary>
    /// <param name="id"></param>
    public void OnCallSlot(int id)
    {
        _learnPerk = GetPerkInfo(id);

        // 선행 조건이 있고 선행조건을 달성하지 못 한 경우
        if (_learnPerk.priorID != -1 && !GetPerkInfo(_learnPerk.priorID).isLearn)
        {
            // 메시지 출력
            Message.instance.ShowMsg(StringData.msgLockPerk);
            return;
        }

        // 학습 상태가 아니면 학습 UI 출력
        if (!_learnPerk.isLearn)
        {
            _goLearningUI.SetActive(true);
            _txtLearnName.text = _learnPerk.name;
            _txtLearnDesc.text = _learnPerk.desc;
            _txtLearnSP.text = _learnPerk.sp.ToString();
            _txtLearnLeftSP.text = _myInfo.GetSkillPoint().ToString();
            _imgLearnSprite.sprite = _learnPerk.sprite;
        }
    }


    /// <summary>
    /// 퍽 학습 시도
    /// </summary>
    public void OnClickLearnBtn()
    {
        // SP가 충분하면
        if(_myInfo.GetSkillPoint() >= _learnPerk.sp)
        {
            // 학습 후 슬롯 셋팅
            _learnPerk.isLearn = true;
            BonusAbility.ApplyBonusAbility(_learnPerk.option, _learnPerk.optionNum);
            _myInfo.DecreaseSkillPoint(_learnPerk.sp);
            SettingSlot();
            _goLearningUI.SetActive(false);
            
            SavePerkData(); // 세이브
        }
        // SP가 부족하면 메세지 출력
        else
        {
            Message.instance.ShowMsg(StringData.msgNotEnoughSP);
            _myInfo.IncreaseSkillPoint(_learnPerk.sp); // 테스트용
            _goLearningUI.SetActive(false);
        }

        _txtLeftSP.text = $"SP {_myInfo.GetSkillPoint()}";
    }

    // 퍽 학습 UI 닫기
    public void OnClickLearnBackBtn()
    {
        _goLearningUI.SetActive(false);
        _learnPerk = null;
    }

    // 퍽 학습 데이터 저장
    public void SavePerkData()
    {
        bool[] perkLearningStatus = new bool[_perkInfo.Length];
        for (int i = 0; i < perkLearningStatus.Length; i++)
        {
            perkLearningStatus[i] = _perkInfo[i].isLearn;
        }

        SaveData<bool[]>.DataSave(perkLearningStatus, path);
    }

    // 퍽 학습 데이터 로드
    public void LoadPerkData()
    {
        bool[] perkLearningStatus = SaveData<bool[]>.DataLoad(path);

        if(perkLearningStatus != null)
        {
            for (int i = 0; i < perkLearningStatus.Length; i++)
            {
                _perkInfo[i].isLearn = perkLearningStatus[i];
            }
        }
        else
        {
            SavePerkData();
        }
    }
}
