using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeCommand : ICommand
{
    DodgeType _dodgeType;
    float _dodgeSpeed;
    float _dodgeDuration;
    Vector2 _dir;

    Player _player;
    Rigidbody2D _rigid;
    SpriteRenderer _renderer;
    BTBlackBoard m_blackBoard;
    bool _isDir;
    float _counterTiming;
    
    public DodgeCommand(MonoBehaviour mono, Player player, float counterTiming, BTBlackBoard blackBoard)
    {
        CommandManager.OnChangePlayer += ChangeActor;
        _mono = mono;
        m_blackBoard = blackBoard;
        _counterTiming = counterTiming;
        _player = player;
        SetStatus();
    }

    public override void ChangeActor(Player player)
    {
        _player = player;
        SetStatus();
        _mono.StopAllCoroutines();

        m_blackBoard.SetValueBool(StringData.isDodge, false);
        _renderer.enabled = true;
    }


    void SetStatus()
    {
        _dodgeSpeed = _player.GetDodgeSpeed();
        _dodgeDuration = _player.GetDodgeDuration();
        _rigid = _player.GetRigid();
        _dodgeType = _player.GetDodgeType();
        _renderer = _player.GetRenderer();
    }


    public override CommandResult Execute()
    {

        if (m_blackBoard.GetValueBool(StringData.isDodge) || m_blackBoard.GetValueBool(StringData.isAttack))
            return CommandResult.Failure;

        _dir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_dir.x != 0 || _dir.y != 0)
        {
            _isDir = true;
            _mono.StartCoroutine(DodgeCoroutine());
            return CommandResult.Success;
        }
        else
        {
            _isDir = false;
            _mono.StartCoroutine(DodgeCoroutine());
            return CommandResult.Success;
        }
    }

    
    // 회피 시작 처리
    void DodgeStart()
    {
        m_blackBoard.SetValueBool(StringData.isDodge, true);
        _dir.Normalize();

        // 회피 이펙트 연출
        switch (_dodgeType)
        {
            // 텔레포트
            case DodgeType.TELEPORT:
                ObjectPoolManager.Instance.GetObjectFromPool(_player.GetDodgeEffect(), _rigid.transform.position + Vector3.up, true);
                if (!_isDir)
                    _dir = _renderer.flipX ? Vector2.left : Vector2.right;
                break;
           // 백스텝
            case DodgeType.BACK_STEP:
                ObjectPoolManager.Instance.GetObjectFromPool(_player.GetDodgeEffect(), _rigid.transform.position, true);
                if (_isDir)
                    _renderer.flipX = !_renderer.flipX;
                else
                    _dir = !_renderer.flipX ? Vector2.left : Vector2.right;
                break;
            // 디펜스 스텝
            case DodgeType.DEFENCE_STEP:
                _dir = !_renderer.flipX ? Vector2.left : Vector2.right;
                ObjectPoolManager.Instance.GetObjectFromPool(_player.GetDodgeEffect(), _rigid.transform.position - (Vector3)_dir + Vector3.up * 0.5f, true);
                break;
        }
    }


    // 회피 처리
    IEnumerator DodgeCoroutine()
    {
        // 회피 시작
        DodgeStart();


        // 회피 동작
        float time = 0f;
        _player.SetCounter(true);
        _mono.Invoke(nameof(CounterEnd), _counterTiming);
        while (_dodgeDuration >= time)
        {
            switch (_dodgeType)
            {
                // 텔레포트
                case DodgeType.TELEPORT:
                    _rigid.position += _dir * _dodgeSpeed;
                    time = _dodgeDuration;
                    break;
                // 백 스탭
                case DodgeType.BACK_STEP:
                    _rigid.MovePosition(_rigid.position + _dir * _dodgeSpeed * Time.fixedDeltaTime);
                    break;
                // 디펜스 스탭
                case DodgeType.DEFENCE_STEP:
                    _rigid.MovePosition(_rigid.position - _dir * _dodgeSpeed * Time.fixedDeltaTime);
                    break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        // 회피 마무리
        DodgeEnd();

        if(_dodgeType == DodgeType.TELEPORT)
            yield return new WaitForSeconds(_dodgeDuration);

        m_blackBoard.SetValueBool(StringData.isDodge, false);
    }

    // 회피 종료 처리
    void DodgeEnd()
    {
        switch (_dodgeType)
        {
            // 텔레포트
            case DodgeType.TELEPORT:
                ObjectPoolManager.Instance.GetObjectFromPool(_player.GetDodgeEffect(), _rigid.position + Vector2.up, true);
                break;
            // 백 스텝
            case DodgeType.BACK_STEP:
                break;
            // 디펜스 스텝
            case DodgeType.DEFENCE_STEP:
                break;
        }
    }

    void CounterEnd()
    {
        _player.SetCounter(false);
    }
}
