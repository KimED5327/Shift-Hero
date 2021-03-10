using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _bgRenderer = null;

    [SerializeField] Transform _limitLeftUp = null;
    [SerializeField] Transform _limitRightDown = null;

    public static Vector4 limitPos;

    UnitManager _unitManager;
    StageResult _stageEffect;
    DialogueManager _dialogueManager;
    StageNumInfo _stageInfo;
    float _elapseTime;

    // 캐싱
    void Awake()
    {
        limitPos = new Vector4(_limitLeftUp.position.x, _limitRightDown.position.x,
                                _limitRightDown.position.y, _limitLeftUp.position.y);

        for (int i = 0; i < _bgRenderer.Length; i++)
        {
            _bgRenderer[i].sprite = GameManager.spriteThema;
        }

        _unitManager = FindObjectOfType<UnitManager>();
        _stageEffect = FindObjectOfType<StageResult>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _stageInfo = FindObjectOfType<StageNumInfo>();
        AchieveDB.IncreaseBattleCount();
    }




    void Start()
    {
        // 플레이어 생성
        _unitManager.SetPlayer();

        // 스테이지 시작
        StartCoroutine(StageStartEffectCoroutine());
    }

    


    // 해당 스테이지 세팅 및 시작
    IEnumerator StageStartEffectCoroutine()
    {
        GameManager.canPlayerMove = false;

        // 몬스터 배치
        _unitManager.SetEnemy(StageDB.GetStage(GameManager.stageNum));

        // 스테이지 시작 연출
        _stageEffect.PlayStageStart(GameManager.stageNum);
        _stageInfo.SetStageInfo(GameManager.stageNum);
        // 애니메이션 종료까지 대기
        yield return null;
        yield return new WaitUntil(()=>_stageEffect.IsFinishAnimationPlay());

        // 스테이지 시작 이벤트가 있다면 대화이벤트 시작.
        DialogueEvent dialogueEvent = DialogueDB.GetMatchedStageStartEvent();
        if(dialogueEvent != null)
        {
            _dialogueManager.ShowDialogue(dialogueEvent);
        }
        // 대화 이벤트 종료까지 대기
        yield return new WaitUntil(() => !_dialogueManager.IsStart());

        // 움직임 허용
        GameManager.canPlayerMove = true;
        StartCoroutine(CheckStageClearCoroutine());
    }


    // 스테이지 Clear 조건 체크
    IEnumerator CheckStageClearCoroutine()
    {
        // 남아있는 몬스터가 없으면
        yield return new WaitUntil(() => UnitManager.GetCurrentEnemyCount() <= 0);

        // 스테이지 클리어
        GameManager.canPlayerMove = false;
        _stageEffect.PlayStageClear(GameManager.stageNum);
        
        // 클리어 애니메이션 완료 대기
        yield return new WaitUntil(() => _stageEffect.IsFinishAnimationPlay());


        // 터치 대기
        _stageEffect.SetCanTouch();
        yield return new WaitUntil(() => _stageEffect.IsTouch());
        yield return new WaitForSeconds(1f);


        DialogueEvent dialogueEvent = DialogueDB.GetMatchedStageFinishEvent();
        if (dialogueEvent != null)
        {
            _dialogueManager.ShowDialogue(dialogueEvent);
        }
        // 대화 이벤트 종료까지 대기
        yield return new WaitUntil(() => !_dialogueManager.IsStart());


        // 플레이어 원위치 및 다음 스테이지 시작
        GameManager.stageNum++;
        _unitManager.ResetPlayerPos();
        StartCoroutine(StageStartEffectCoroutine());
    }

    /// <summary>
    /// 스테이지 클리어 실패
    /// </summary>
    public void StageFail()
    {
        _unitManager.SavePlayerResult();
        GameManager.canPlayerMove = false;
        AchieveDB.RecordClearStage(GameManager.stageNum);
        AchieveDB.IncreaseAllDieCount(1);
        _stageEffect.PlayStageFail(GameManager.stageNum, _elapseTime);

    }

    private void Update()
    {
        if (GameManager.canPlayerMove)
        {
            _elapseTime += Time.deltaTime;
        }
    }
}
