using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    Player _player;
    Rigidbody2D _rigid;
    SpriteRenderer _renderer;
    float _moveSpeed;
    Vector2 _dir;
    BTBlackBoard m_blackBoard;

    public MoveCommand(MonoBehaviour mono, Player player, BTBlackBoard blackBoard)
    {
        CommandManager.OnChangePlayer += ChangeActor;
        m_blackBoard = blackBoard;
        _mono = mono;
        _player = player;
        SetStatus();
    }

    public override void ChangeActor(Player player)
    {
        _player = player;
        SetStatus();
    }

    void SetStatus()
    {
        _moveSpeed = _player.GetMoveSpeed();
        _rigid = _player.GetRigid();
        _renderer = _player.GetRenderer();
    }

    public override CommandResult Execute()
    {
        // 닷지, 공격중은 Fail
        if (m_blackBoard.GetValueBool(StringData.isDodge) || m_blackBoard.GetValueBool(StringData.isAttack))
            return CommandResult.Failure;

        // 올바른 방향이면 해당 방향으로 움직임
        _dir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_dir.x != 0 || _dir.y != 0)
        {
            _dir.Normalize();

            _rigid.MovePosition(_rigid.position + _dir * _moveSpeed * Time.fixedDeltaTime);

            if (_dir.x > 0)
                _renderer.flipX = false;
            else if (_dir.x < 0)
                _renderer.flipX = true;

            return CommandResult.Success;
        }
        else
            return CommandResult.Failure;
    }


}
