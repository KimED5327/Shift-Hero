using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRangeNode : BTNode
{
    BTBlackBoard _blackBoard;
    Enemy _enemy;

    float _range;
    bool _forceChase = false;
    Player[] _target;
    Transform _origin;
    Animator _anim;

    public ChaseRangeNode(BTBlackBoard blackBoard, bool forceChase, Enemy enemy)
    {
        _blackBoard = blackBoard;
        _enemy = enemy;
        _forceChase = forceChase;
        _range = _enemy.GetViewRange();
        _target = _enemy.GetTargets();
        _origin = _enemy.transform;
        _anim = _enemy.GetAnimator();
    }

    public override Result Execute()
    {
        Transform target = _target[PlayerController.s_charChoiceIndex].transform;
        float sqrDistance = Vector3.SqrMagnitude(target.position - _origin.position);
        float myRange = Mathf.Pow(_range, 2);

        // 사정거리 이내에 있으면 Success로 다음 지시 이행
        if (sqrDistance <= myRange && !_blackBoard.GetValueBool(BTBlackBoard.IsAttack))
        {
            if(_forceChase)
                _blackBoard.SetValueBool(BTBlackBoard.ForceChase, true);
            
            return Result.SUCCESS;
        }
        // Success로 다음 지시 이행
        else if (_blackBoard.GetValueBool(BTBlackBoard.ForceChase))
        {
            return Result.SUCCESS;
        }
        else {
            _anim.SetBool("Walk", false);
            return Result.FAILURE; 
        }
    }

}
