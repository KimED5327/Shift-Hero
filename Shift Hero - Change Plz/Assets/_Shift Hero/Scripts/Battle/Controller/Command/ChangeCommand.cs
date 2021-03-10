using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCommand : ICommand
{

    SpriteRenderer _renderer;

    Player[] _characters;
    Player _curPlayer;
    float _changeTime;

    bool _changeStart = false;

    PoolType _changeEffect;
    PoolType _changePreEffect;

    WaitForSeconds _waitTime;

    BTBlackBoard m_blackBoard;

    public ChangeCommand(MonoBehaviour mono, Player player, Player[] characters, float changeTime, PoolType change, PoolType preChange, BTBlackBoard blackBoard)
    {
        CommandManager.OnChangePlayer += ChangeActor;
        m_blackBoard = blackBoard;
        _changeStart = false;
        _mono = mono;
        _curPlayer = player;
        _characters = characters;
        _changeTime = changeTime;
        _changeEffect = change;
        _changePreEffect = preChange;
        _renderer = _curPlayer.GetRenderer();
        _waitTime = new WaitForSeconds(_changeTime);
    }

    public override void ChangeActor(Player player)
    {
        BattleData.IncreaseChangeCount(1);

        // 위치 방향 저장
        Vector3 t_curPos = _curPlayer.transform.position;
        bool t_flipXStatus = _renderer.flipX;

        // 모든 캐릭터 비활성화
        for (int i = 0; i < _characters.Length; i++)
            _characters[i].gameObject.SetActive(false);


        // 교체된 캐릭터로 컴포넌트와 정보 캐싱
        _renderer = _curPlayer.GetRenderer();
        _curPlayer = player;
        _curPlayer.transform.position = t_curPos;
        _renderer.flipX = t_flipXStatus;

        // 교체한 캐릭터만 활성화
        _curPlayer.gameObject.SetActive(true);
        _changeStart = false;

        // 상태 리셋
        m_blackBoard.SetValueBool(StringData.isAttack, false);
        m_blackBoard.SetValueBool(StringData.isDodge, false);
        GameManager.canPlayerMove = true;
    }



    public override CommandResult Execute()
    {
        // 교체중이 아닌 경우에만 success
        if (!_changeStart)
        {
            // 교체 실행
            _mono.StartCoroutine(ChangeCo());
            return CommandResult.Success;
        }
        else
        {
            return CommandResult.Failure;
        }
    }


    // 교체
    IEnumerator ChangeCo()
    {
        // 교체 시전중
        _changeStart = true;

        // 교체 프리 이펙트
        GameObject effect = ObjectPoolManager.Instance.GetObjectFromPool(_changePreEffect, true);
        effect.GetComponent<AutoFollow>().SetTarget(_curPlayer.transform);

        yield return _waitTime;

        // 교체 될 때까지 반복
        int count = 0;
        while (true)
        {
            // 교체 대상 인덱스 변경
            if (++PlayerController.s_charChoiceIndex >= _characters.Length)
                PlayerController.s_charChoiceIndex = 0;

            // 교체할 대상이 살아 있다면
            if (!_characters[PlayerController.s_charChoiceIndex].IsDead())
            {
                // 교체 완료 이펙트 //
                ObjectPoolManager.Instance.GetObjectFromPool(_changeEffect, _curPlayer.transform.position + Vector3.up * 0.5f, true);

                // 교체한 캐릭터 이벤트 호출;
                CommandManager.CallChangeActorEvent(_characters[PlayerController.s_charChoiceIndex]);
                break;
            }

            // 모든 캐릭터가 죽었다면 반응 X
            if(++count >= _characters.Length)
            {
                _changeStart = false;
                break;
            }
        }
    }
}
