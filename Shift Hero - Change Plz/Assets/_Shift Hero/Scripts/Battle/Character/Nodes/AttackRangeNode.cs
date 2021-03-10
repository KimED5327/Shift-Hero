using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeNode : BTNode
{
    BTBlackBoard _blackBoard;
    Enemy _enemy;

    float _range;
    float _rangeHeight;
    Player[] _target;
    Transform _origin;
    Animator _anim;

    public AttackRangeNode(BTBlackBoard blackBoard, Enemy enemy)
    {
        _blackBoard = blackBoard;
        _enemy = enemy;
        _range = _enemy.GetMaxRange();
        _rangeHeight = _enemy.GetMaxAtkDisY();
        _target = _enemy.GetTargets();
        _origin = _enemy.transform;
        _anim = _enemy.GetAnimator();
    }

    public override Result Execute()
    {
        Transform target = _target[PlayerController.s_charChoiceIndex].transform;
        float sqrDistance = Vector3.SqrMagnitude(target.position - _origin.position);
        float myRange = Mathf.Pow(_range, 2);

        // y값 높낮이가 너무 차이나면 Failure 리턴
        if (_rangeHeight != 0 && Mathf.Abs(target.position.y - _origin.position.y) > _rangeHeight)
            return Result.FAILURE;

        // 사정거리 이내에 있으면 Success로 다음 지시 이행
        if (sqrDistance <= myRange && !_blackBoard.GetValueBool(BTBlackBoard.IsAttack))
        {
            return Result.SUCCESS;
        }
        else
        {
            return Result.FAILURE;
        }
    }
}
