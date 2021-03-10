using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveInfoData
{
    public int level;
    public int exp;
    public int sp;
}

public class MyInfo : MonoBehaviour
{
    static bool isInitialized = false;
    static readonly string path = "/Info.dat"; // 저장 경로

    static int _myLevel = 1;           // 계정 레밸
    static int _myExp = 0;             // 계정 경험치
    static int _mySP = 0;              // 계정 스킬 포인트

    string _myGrade = "브론즈";        // 계정 등급

    public delegate void ChangeInfoCheck();
    public static event ChangeInfoCheck e_changeInfoCheck;

    // UI 캐싱
    [SerializeField] Text _txtClover = null;
    [SerializeField] Text _txtGold = null;
    [SerializeField] Text _txtNickname = null;
    [SerializeField] Text _txtLevel = null;
    [SerializeField] Text _txtGrade = null;

    [SerializeField] Image _imgGrade = null;
    [SerializeField] Image _imgExpFilled = null;

    [SerializeField] Sprite[] _medalSprite = null;

    // 컴포넌트 캐싱
    Inventory _inven;

    // Start is called before the first frame update
    void Start()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            LoadInfoData();
        }

        _inven = FindObjectOfType<Inventory>();
        _inven.LinkMyInfo(this);
        // HUD INFO 변경 이벤트에 INFO 세팅 함수 대입
        e_changeInfoCheck = SetInfo;

        //전투 이후 레밸업 했는지 체크
        CheckLevelUp();
        SetInfo();
    }


    // HUD INFO 세팅
    void SetInfo()
    {
        // 등급 산정 (레밸 5 이하는 브론즈, 10 이하는 실버, 그 이상은 골드)
        if (_myLevel <= 5)
        {
            _myGrade = "브론즈";
            _imgGrade.sprite = _medalSprite[0];
        }
        else if (_myLevel <= 10)
        {
            _myGrade = "실버";
            _imgGrade.sprite = _medalSprite[1];
        }
        else
        {
            _myGrade = "골드";
            _imgGrade.sprite = _medalSprite[2];
        }

        // 정보 텍스트 세팅
        _txtGrade.text = _myGrade;
        _txtLevel.text = $"Lv. {_myLevel}";
        _txtNickname.text = StringData.myNickname;
        _txtGold.text = $"{string.Format("{0:###,0}", _inven.GetGold())} G";
        _txtClover.text = $"{_inven.GetClover()} / {_inven.GetMaxClover()}";

        // 경험치 게이지
        _imgExpFilled.fillAmount = (float)_myExp / PlayerDB.GetAccountLevelUpNeedExp(_myLevel);
    }

    /// <summary>
    /// 경험치 증가
    /// </summary>
    /// <param name="value"></param>
    public void IncreaseExp(int value)
    {
        _myExp += value;
        CheckLevelUp();
        SetInfo();
        SaveInfoData();
    }

    // 레밸업 체크 후 레밸업
    void CheckLevelUp()
    {
        bool isLevelUp = false;
        
        // 현재 경험치 > 필요경험치
        while (_myExp >= PlayerDB.GetAccountLevelUpNeedExp(_myLevel))
        {
            // 경험치 감소
            _myExp -= PlayerDB.GetAccountLevelUpNeedExp(_myLevel);

            // 레밸업 + 스킬 포인트 증가
            IncreaseSkillPoint(1);
            _myLevel++;
            isLevelUp = true;
        }

        SaveInfoData();

        // 레밸업 시 UI 출력
        if (isLevelUp)
            ShowLevelUpUI();
    }


    // 레밸업 UI 보여주기
    void ShowLevelUpUI()
    {
        Message.instance.ShowGoodmsg(StringData.msgGoodLevelUp);
    }

    /// <summary>
    /// HUD INFO 변경사항 반영
    /// </summary>
    public static void CallMyInfoChange()
    {
        e_changeInfoCheck.Invoke();
    }

    /// <summary>
    /// 현재 스킬 포인트 가져오기
    /// </summary>
    /// <returns></returns>
    public int GetSkillPoint() { return _mySP; }


    /// <summary>
    /// 스킬 포인트 증가
    /// </summary>
    /// <param name="value"></param>
    public void IncreaseSkillPoint(int value = 1) 
    { 
        _mySP += value; 
        SaveInfoData(); 
    }

    /// <summary>
    /// 스킬 포인트 감소
    /// </summary>
    /// <param name="value"></param>
    public void DecreaseSkillPoint(int value = 1) 
    { 
        _mySP -= value; 
        if (_mySP < 0) 
            _mySP = 0; 
        SetInfo(); 
        SaveInfoData(); 
    }
    
    /// <summary>
    /// 전투에서 획득한 경험치 전달
    /// </summary>
    /// <param name="value"></param>
    public static void GiveBattleExp(int value)
    {
        _myExp += value;
    }

    public static int GetExp() { return _myExp; }
    public static int GetLevel() { return _myLevel; }

    public void SaveInfoData()
    {
        SaveInfoData infoData = new SaveInfoData()
        {
            level = GetLevel(),
            exp = GetExp(),
            sp = GetSkillPoint()
        };
        SaveData<SaveInfoData>.DataSave(infoData, path);
    }

    public void LoadInfoData()
    {
        SaveInfoData infoData = SaveData<SaveInfoData>.DataLoad(path);

        if(infoData != null)
        {
            _myLevel = infoData.level;
            _myExp = infoData.exp;
            _mySP = infoData.sp;
        }
        else
        {
            SaveInfoData();
        }
    }
}
