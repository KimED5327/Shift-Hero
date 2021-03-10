using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // 데이터 블랙보드
        BTBlackBoard blackBoard = new BTBlackBoard();
        blackBoard.SetValueVector2(BTBlackBoard.Dir, Vector2.zero);
        blackBoard.SetValueBool(BTBlackBoard.IsAttack, false);
        blackBoard.SetValueBool(BTBlackBoard.ForceChase, false);

        // 추적 시퀀스
        ChaseRangeNode rangeNode = new ChaseRangeNode(blackBoard, false, this);
        ChaseNode chaseNode = new ChaseNode(blackBoard, this);
        BTSequence chaseSeq = new BTSequence(new List<BTNode> { rangeNode, chaseNode });

        // 공격 시퀀스
        AttackRangeNode attackRangeNode = new AttackRangeNode(blackBoard, this);
        MeleeAttackNode meleeAttackNode = new MeleeAttackNode(blackBoard, this, this);
        BTSequence attackSeq = new BTSequence(new List<BTNode> { attackRangeNode, meleeAttackNode });

        // 추적 or 공격 셀렉터
        BTSelector selector_Atk_Chase = new BTSelector(new List<BTNode> { attackSeq, chaseSeq });

        // 방향 시퀀스
        CalcDirNode dirNode = new CalcDirNode(blackBoard, this);

        _root = new BTSequence(new List<BTNode>{dirNode, selector_Atk_Chase});
    }
}
