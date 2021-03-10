using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageInfo{
    public string name;
    public Sprite sprite;
}

public class WorldMenu : MonoBehaviour
{
    static readonly int MAX_STAGE = 11;
    static readonly string stageScene = "Stage";

    // 맵 정보
    [SerializeField] Text _txtThema = null;
    [SerializeField] Text _txtStage = null;
    [SerializeField] Image _imgThema = null;

    // 출전 캐릭터 정보
    [SerializeField] Image[] _imgChoiceCharacters = null;

    // 테마 정보
    [SerializeField] StageInfo[] _stagesInfo = null;
    [SerializeField] GameObject _goLock = null;

    int _choiceThema = 0;
    bool _isLockChoiceThema = false;

    Inventory _inven;

    private void Start()
    {
        _inven = FindObjectOfType<Inventory>();
        SetStageInfo();
    }

    // 테마 변경 (-1)
    public void OnClickLeftButton()
    {
        if (--_choiceThema < 0)
            _choiceThema = _stagesInfo.Length - 1;

        SetStageInfo();
    }

    // 테마 변경 (+1)
    public void OnClickRightButton()
    {
        if (++_choiceThema > _stagesInfo.Length - 1)
            _choiceThema = 0;

        SetStageInfo();
    }

    // 테마 정보 세팅
    void SetStageInfo()
    {
        // 테마 정보 세팅
        _imgThema.sprite = _stagesInfo[_choiceThema].sprite;
        _txtThema.text = _stagesInfo[_choiceThema].name;

        // 해금 여부 판별
        _isLockChoiceThema = AchieveDB.GetClearStageCount() < _choiceThema * MAX_STAGE;
        _goLock.SetActive(_isLockChoiceThema);
        _imgThema.color = _isLockChoiceThema ? Color.gray : Color.white;

        // 해금된 테마라면
        if (!_isLockChoiceThema)
        {
            // 기록 세팅
            int curThemaStageClearRecord = AchieveDB.GetClearStageCount() / (_choiceThema + 1) % MAX_STAGE;

            if (curThemaStageClearRecord < MAX_STAGE)
                _txtStage.text = $"최종 기록 : {curThemaStageClearRecord} STAGE";
            else
                _txtStage.text = "최종 기록 : All Clear";
        }
        // 미해금된 테마라면
        else
        {
            _txtStage.text = $"최종 기록 : -";
        }

    }

    // 시작
    public void BtnStagePlay()
    {
        // 미해금된 테마라면 메세지 출력
        if (_isLockChoiceThema)
        {
            Message.instance.ShowMsg(StringData.msgLockThema);
            return;
        }

        // 클로버가 있으면 보너스
        if(_inven.GetClover() >= 1)
        {
            _inven.DecreaseClover(1);
            GameManager.isCloverEnough = true;
        }
        else
            GameManager.isCloverEnough = false;


        int startStage = _choiceThema * MAX_STAGE;

        GameManager.themaNum = _choiceThema;
        GameManager.stageNum = startStage;
        GameManager.spriteThema = _imgThema.sprite;
        StartCoroutine(Loading());
    }

    /// <summary>
    /// 출전 캐릭터 이미지 교체
    /// </summary>
    /// <param name="index"></param>
    /// <param name="sprites"></param>
    public void SettingCharcaterSprite(int index, Sprite sprites)
    {
        _imgChoiceCharacters[index].sprite = sprites;
    }



    IEnumerator Loading()
    {
        ScreenEffect.Instance.FadeOut(1f);

        yield return new WaitUntil(() => ScreenEffect.Instance.IsFinishEffect());

        ScreenEffect.Instance.FadeIn(1f);

        SceneManager.LoadScene(stageScene);
    }
}
