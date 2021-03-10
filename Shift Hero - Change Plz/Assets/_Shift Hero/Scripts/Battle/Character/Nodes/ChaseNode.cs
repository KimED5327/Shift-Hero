using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : BTNode
{
    BTBlackBoard _blackBoard;
    Enemy _enemy;

    float _moveSpeed;
    float _canAttackDistance;
    float _minY;

    Player[] _target;
    Rigidbody2D _myRigid;
    Animator _anim;

    public ChaseNode(BTBlackBoard blackBoard, Enemy enemy)
    {
        _blackBoard = blackBoard;
        _enemy = enemy;
        _target = _enemy.GetTargets();
        _anim = _enemy.GetAnimator();
        _myRigid = _enemy.GetRigid();
        _moveSpeed = _enemy.GetMoveSpeed();
        _canAttackDistance = _enemy.GetMinRange();
        _minY = _enemy.GetMaxAtkDisY();
    }

    public override Result Execute()
    {
        Transform target = _target[PlayerController.s_charChoiceIndex].transform;

        float distance = Vector3.SqrMagnitude(target.position - _myRigid.transform.position);
        float canAtkDistance = Mathf.Pow(_canAttackDistance, 2);
        bool canAtkInnerHeight = Mathf.Abs(target.position.y - _myRigid.transform.position.y) <= _minY;

        Vector3 dir = CalcDestinationPos();

        // 거리가 멀거나, 높이값이 큰 차이가 나는 경우
        if (canAtkDistance <= distance || !canAtkInnerHeight)
        {
            // 추적 실행
            _anim.SetBool("Walk", true);
            _myRigid.MovePosition(_myRigid.transform.position + dir * _moveSpeed * Time.fixedDeltaTime);
            return Result.RUNNING;
        }
        else
        {
            // 추적 중단
            _anim.SetBool("Walk", false);
            return Result.FAILURE;
        }
    }


    Vector3 CalcDestinationPos()
    {

        Vector3 dir = _blackBoard.GetValueVector2(BTBlackBoard.Dir);

        // 타겟이 오른쪽에 있으면, 타겟 왼쪽에 붙음
        if (dir.x > 0)
            dir.x = Mathf.Abs(dir.x) - _canAttackDistance;
        else
            dir.x = -Mathf.Abs(dir.x) + _canAttackDistance;

        return dir.normalized;
    }
}
