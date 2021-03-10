using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackNode : BTNode
{
    BTBlackBoard _blackBoard;
    MonoBehaviour _mono;
    Enemy _enemy;

    // 공격 정보
    float m_attackDamageDelay;
    float m_attackDelay;
    float m_maxRange;
    float m_attackHeight;

    LayerMask m_layerMask;
    PoolType m_attackEffect;
    Vector2 m_dir;


    Rigidbody2D m_myRigid;
    Animator m_anim;


    public MeleeAttackNode(BTBlackBoard blackBoard, Enemy enemy, MonoBehaviour mono)
    {
        _blackBoard = blackBoard;
        _enemy = enemy;
        m_attackDamageDelay = _enemy.GetAtkDmgDelay();
        m_attackDelay = _enemy.GetAtkDelay();
        m_maxRange = _enemy.GetMaxRange();
        m_attackHeight = _enemy.GetMaxAtkDisY();
        m_layerMask = _enemy.GetLayerMask();
        _mono = mono;
        m_myRigid = _enemy.GetRigid();
        m_anim = _enemy.GetAnimator();
        m_attackEffect = _enemy.GetAttackEffect();
        _blackBoard.SetValueBool(BTBlackBoard.IsAttack, false);
    }


    public override Result Execute()
    {

        if (!_blackBoard.GetValueBool(BTBlackBoard.IsAttack))
        {
            _blackBoard.SetValueBool(BTBlackBoard.IsAttack, true);
            _mono.StartCoroutine(AttackCo());
            return Result.RUNNING;
        }
        else
        {
            return Result.FAILURE;
        }
        

    }

    IEnumerator AttackCo()
    {
        m_anim.SetBool("Walk", false);
        m_anim.SetTrigger("Attack");

        yield return new WaitForSeconds(m_attackDamageDelay);
        TryHit();

        yield return new WaitForSeconds(m_attackDelay - m_attackDamageDelay);
        yield return new WaitForSeconds(0.1f); // 안정 대기

        _blackBoard.SetValueBool(BTBlackBoard.IsAttack, false);
    }

    void TryHit()
    {
        m_dir = _blackBoard.GetValueVector2(BTBlackBoard.Dir).normalized;
        Vector2 t_originPos = m_myRigid.position;
        Vector2 t_size = new Vector2(0.1f, m_attackHeight);
        RaycastHit2D t_hitInfo = Physics2D.BoxCast(t_originPos, t_size, 0, m_dir, m_maxRange, m_layerMask);

        if (t_hitInfo.collider != null)
        {
            Collider2D t_col = t_hitInfo.collider;

            if (t_col.CompareTag(StringData.tagPlayer))
            {
                // 타격 이펙트
                ObjectPoolManager.Instance.GetObjectFromPool(m_attackEffect, t_col.transform.position, true);
                t_col.GetComponent<Character>().Hurt(_enemy);
            }
        }
    }
}
