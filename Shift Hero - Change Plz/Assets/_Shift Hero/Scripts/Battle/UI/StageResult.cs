using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageResult : MonoBehaviour
{
    readonly string STAGE_START = "Start", STAGE_FAIL = "Fail", STAGE_CLEAR = "Clear", STAGE_CLEAR_TOUCH = "Clear_Disappear";
    [Header("Start UI")]
    [SerializeField] Text _txtCurStage = null;

    [Header("Clear UI")]
    [SerializeField] StageClearRewardSlot[] _rewardSlots = null;

    [Header("Fail UI")]
    [SerializeField] Text _txtLastStage = null;
    [SerializeField] Text _txtAnswerLeft = null;
    [SerializeField] Text _txtAnswerRight = null;
    [SerializeField] GameObject[] _goButton = null;

    bool _isTouch;

    Animator _anim;

    ApplyResult _applyResult;
    UnitManager _unitManager;

    // Start is called before the first frame update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _applyResult = FindObjectOfType<ApplyResult>();
        _unitManager = FindObjectOfType<UnitManager>();
    }

    /// <summary>
    /// 스테이지 시작 호출
    /// </summary>
    /// <param name="stageNum"></param>
    public void PlayStageStart(int stageNum)
    {
        _anim.SetTrigger(STAGE_START);
        _txtCurStage.text = $"STAGE {stageNum + 1}";
    }

    /// <summary>
    /// 스테이지 클리어 호출
    /// </summary>
    /// <param name="stageNum"></param>
    public void PlayStageClear(int stageNum)
    {
        List<StageClearReward> stageClearRewardList = StageDB.GetStage(stageNum).stageClearRewardList;

        // 보상 표시
        for (int i = 0; i < _rewardSlots.Length; i++)
        {
            // 해당 스테이지 클리어 보상 개수만큼만 표시
            if(i < stageClearRewardList.Count)
            {
                _rewardSlots[i].SetSlot(stageClearRewardList[i].id, stageClearRewardList[i].count, this);

                if (stageClearRewardList[i].id == ItemDB.EXP)
                    BattleData.IncreaseExpCount(stageClearRewardList[i].count);
                else if (stageClearRewardList[i].id == ItemDB.GOLD)
                    BattleData.IncreaseGoldCount(stageClearRewardList[i].count);
            }

            // 이후 슬롯은 비활성화
            else
            {
                _rewardSlots[i].ClearSlot();
            }

           
        }

        // 클리어 UI 호출
        _anim.SetTrigger(STAGE_CLEAR);
    }


    /// <summary>
    /// 플레이어 전멸 시 호출
    /// </summary>
    /// <param name="stageNum"></param>
    /// <param name="themaNum"></param>
    public void PlayStageFail(int stageNum, float time)
    {
        int killCount = BattleData.GetKillCount();
        int hitCount = BattleData.GetPlayerHitCount();
        int hurtCount = BattleData.GetPlayerHurtCount();
        int hour = (int)time / 3600;
        int min = (int)time % 3600 / 60;
        int sec = (int)time % 60;
        string elapseTime = $"{string.Format("{0:00}", hour)}:{string.Format("{0:00}", min)}:{string.Format("{0:00}", sec)}";
        int exp = BattleData.GetBattleExp();
        int gold = BattleData.GetBattleGold();
        int changeCount = BattleData.GetChangeCount();
        int counterCount = BattleData.GetCounterCount();

        _txtLastStage.text = $"{(stageNum / 10) + 1} - {(stageNum % 10) + 1} 스테이지";

        _txtAnswerLeft.text = $"{killCount}\n";
        _txtAnswerLeft.text += $"{hitCount}\n";
        _txtAnswerLeft.text += $"{hurtCount}\n";
        _txtAnswerLeft.text += $"{elapseTime}";
        
        _txtAnswerRight.text = $"{exp} EXP\n";
        _txtAnswerRight.text += $"{gold} G\n";
        _txtAnswerRight.text += $"{changeCount}\n";
        _txtAnswerRight.text += $"{counterCount}";

        _anim.SetTrigger(STAGE_FAIL);

        Invoke(nameof(ShowGiveExpWindow), 3f);
    }

    void ShowGiveExpWindow()
    {
        _applyResult.SetSlots(_unitManager.GetPlayers());
    }


    /// <summary>
    /// 클리어 화면 터치시
    /// </summary>
    public void OnClickStageClearScreen()
    {
        if (!_isTouch)
        {
            _anim.SetTrigger(STAGE_CLEAR_TOUCH);
            _isTouch = true;
        }
    }

    /// <summary>
    /// 버튼 활성화 여부
    /// </summary>
    /// <param name="flag"></param>
    public void SetActiveButton(bool flag)
    {
        for (int i = 0; i < _goButton.Length; i++)
        {
            _goButton[i].SetActive(flag);
        }
    }

    public void OnClickRetryButton()
    {
        StartCoroutine(Loading("Stage"));
    }

    public void OnClickBackHomeBtn()
    {
        StartCoroutine(Loading("Home"));
    }

    /// <summary>
    /// 애니메이션 전부 재생되었는지
    /// </summary>
    /// <returns></returns>
    public bool IsFinishAnimationPlay()
    {
        return _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }

    /// <summary>
    /// 터치 여부
    /// </summary>
    /// <returns></returns>
    public bool IsTouch()
    {
        return _isTouch;
    }

    public void SetCanTouch()
    {
        _isTouch = false;
    }


    IEnumerator Loading(string sceneName)
    {
        ScreenEffect.Instance.FadeOut(1f);

        yield return new WaitUntil(() => ScreenEffect.Instance.IsFinishEffect());

        ScreenEffect.Instance.FadeIn(1f);

        SceneManager.LoadScene(sceneName);
    }
}
