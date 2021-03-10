using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcDirNode : BTNode
{
    BTBlackBoard _blackBoard;
    Enemy _enemy;

    float _range;

    Player[] _target;
    Transform _origin;
    Animator _anim;
    SpriteRenderer _renderer;

    public CalcDirNode(BTBlackBoard blackBoard, Enemy enemy)
    {
        _blackBoard = blackBoard;
        _enemy = enemy;
        _target = _enemy.GetTargets();
        _origin = _enemy.transform;
        _range = _enemy.GetViewRange();
        _renderer = _enemy.GetRenderer();
        _anim = _enemy.GetAnimator();
    }

    public override Result Execute()
    {
        Transform target = _target[PlayerController.s_charChoiceIndex].transform;
        float sqrDistance = Vector3.SqrMagnitude(target.position - _origin.position);
        float myRange = Mathf.Pow(_range, 2);

        // 방향 산출
        if (!_blackBoard.GetValueBool(BTBlackBoard.IsAttack))
        {
            Vector3 dir = (target.position - _origin.position);

            if(sqrDistance <= myRange)
            {
                _blackBoard.SetValueVector2(BTBlackBoard.Dir, dir);
                _renderer.flipX = dir.x <= 0;
                return Result.SUCCESS;
            }
            else if (_blackBoard.GetValueBool(BTBlackBoard.ForceChase))
            {
                _blackBoard.SetValueVector2(BTBlackBoard.Dir, dir);
                _renderer.flipX = dir.x <= 0;
                return Result.SUCCESS;
            }
        }
        return Result.RUNNING;
    }
}
