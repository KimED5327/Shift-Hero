using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static readonly string WALK = "Walk";              // 걷기
    static readonly string ATTACK = "Attack";          // 공격
    static readonly string DODGE = "Dodge";            // 회피

    public static int s_charChoiceIndex = 0;           // 현재 선택된 캐릭터 인덱스
    int _survivalPlayerCount;                          // 현재까지 살아남은 캐릭터 수
    
    Player[] _players;                                 // 스탠 바이 캐릭터들
    Player _curPlayer;                                 // 조종중인 캐릭터
    Animator _curPlayerAnim;                           // 조종중인 캐릭터의 애니메이터

    WaitForSeconds waitTime =  new WaitForSeconds(2f); // 캐릭터 사망 체크 타임

    // 캐릭터 교체 정보
    [Header("Change Unit")]
    [SerializeField] float _changeTime = 0.1f;         // 교체 소요 시간
    [SerializeField] PoolType _changeEffect = 0;       // 교체 이펙트
    [SerializeField] PoolType _changePreEffect = 0;    // 교체 시전 이펙트

    [Header("Counter")]
    [SerializeField] float _counterTiming = 0.1f;       // 카운터 타이밍

    // 컨트롤 유닛 캐싱 컴포넌트
    HpBarManager _hpBarManager;                        // HP바 
    PlayerSlotStatus _playerSlotStatus;                // 슬롯 UI
    CameraController _cameraController;                // 카메라

    CommandManager _commandManager;                    // 커맨드 매니저
    StageManager _stageManager;                        // 스테이지 매니저

    // 컴포넌트 캐싱
    void Awake()
    {
        _hpBarManager = FindObjectOfType<HpBarManager>();
        _playerSlotStatus = FindObjectOfType<PlayerSlotStatus>();
        _cameraController = FindObjectOfType<CameraController>();
        _stageManager = FindObjectOfType<StageManager>();
    }

    public void Initialized(Player[] players)
    {
        // 시작 캐릭터 캐싱
        _players = players;
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].Initialized();
        }
        _survivalPlayerCount = _players.Length;
        // 컴포넌트에 현재 캐릭터 정보 전달
        s_charChoiceIndex = 0;
        SetLinkPlayerToComponents(_players[s_charChoiceIndex]);

        // 커맨드 등록
        BTBlackBoard blackBoard = new BTBlackBoard();
        blackBoard.SetValueBool(StringData.isDead, false);
        blackBoard.SetValueBool(StringData.isAttack, false);
        blackBoard.SetValueBool(StringData.isDodge, false);

        _commandManager = new CommandManager();
        MoveCommand cmdMove = new MoveCommand(this, _curPlayer, blackBoard);
        AttackCommand cmdAttack = new AttackCommand(this, _curPlayer, blackBoard);
        ChangeCommand cmdChange = new ChangeCommand(this, _curPlayer, _players, _changeTime, _changeEffect, _changePreEffect, blackBoard);
        DodgeCommand cmdDodge = new DodgeCommand(this, _curPlayer, _counterTiming, blackBoard);
        _commandManager.AddCommand(PlayerCommand.Move, cmdMove);
        _commandManager.AddCommand(PlayerCommand.Attack, cmdAttack);
        _commandManager.AddCommand(PlayerCommand.Change, cmdChange);
        _commandManager.AddCommand(PlayerCommand.Dodge, cmdDodge);

        // 캐릭 교체시 교체한 캐릭터의 컴포넌트를 캐싱
        CommandManager.OnChangePlayer += SetLinkPlayerToComponents;
    }

    void Update()
    {
        if (_curPlayer == null) return;
        if (GameManager.canPlayerMove == false) return;

        Change();
        Attack();
        Move();
        Dodge();
    }

    // 이동
    void Move()
    {
        // 방향키 눌리면 커맨드 실행
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // 커맨드 실행 성공하면 걷기 애니메이션.
            if (_commandManager.InvokeExecute(PlayerCommand.Move) == CommandResult.Success)
                _curPlayerAnim.SetBool(WALK, true);
            else
                _curPlayerAnim.SetBool(WALK, false);
        }
        else
            _curPlayerAnim.SetBool(WALK, false);
        
    }

    // 공격
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 공격 커맨드 성공하면 공격 애니메이션
            if (_commandManager.InvokeExecute(PlayerCommand.Attack) == CommandResult.Success)
                _curPlayerAnim.SetTrigger(ATTACK);
            
        }
    }


    // 교체
    void Change()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            _commandManager.InvokeExecute(PlayerCommand.Change);
        }
    }

    // 회피
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)){

            if(_commandManager.InvokeExecute(PlayerCommand.Dodge) == CommandResult.Success)
                _curPlayerAnim.SetTrigger(DODGE);
        }

    }


    // 플레이어가 교체되면 이벤트 호출을 통해 다시 링크.
    public void SetLinkPlayerToComponents(Player player)
    {
        // 캐릭터 캐싱
        _curPlayer = player;
        _curPlayer.gameObject.SetActive(true);
        _curPlayerAnim = _curPlayer.GetAnimator();

        _hpBarManager.SetCurPlayerLink(_curPlayer.transform);      // 체력바 교체
        _playerSlotStatus.SetChoiceCharacter(s_charChoiceIndex);     // 선택된 슬롯 변경
        _cameraController.SetTargetLink(_curPlayer.transform);     // 카메라 타겟팅 변경
        
        // 바뀐 캐릭터, 언제 죽는지 체크 시작
        StopAllCoroutines();
        StartCoroutine(CheckPlayerDead());
    }


    // 조종중인 플레이어가 죽으면 다음 캐릭터로 교체
    IEnumerator CheckPlayerDead()
    {
        while (true)
        {
            yield return waitTime;
            // 플레이어 사망 확인
            if (_curPlayer.IsDead())
            {
                // 생존 수 감소
                _survivalPlayerCount--;

                // 생존한 캐릭터가 1 이상이면 교체
                if(_survivalPlayerCount > 0)
                    _commandManager.InvokeExecute(PlayerCommand.Change);


                // 생존한 캐릭터가 없으면 게임 실패
                else
                {
                    _stageManager.StageFail();
                    yield break;
                }
                
            }
        }
    }
}