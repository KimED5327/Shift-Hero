using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICommand
{
    Player m_player;
    Rigidbody2D m_myRigid;
    SpriteRenderer m_renderer;
    int m_attack;
    float m_attackDamageDelay;
    float m_attackDelay;
    float m_fireWait;
    float m_knockBackPower;
    float m_maxRange;
    float m_cri;
    float m_criRate;
    LayerMask m_layerMask;
    BTBlackBoard m_blackBoard;

    public AttackCommand(MonoBehaviour mono, Player player, BTBlackBoard blackBoard)
    {
        CommandManager.OnChangePlayer += ChangeActor;
        _mono = mono;
        m_blackBoard = blackBoard;
        m_player = player;
        SetStatus();
    }

    public override void ChangeActor(Player player)
    {
        m_player = player;
        SetStatus();
    }

    void SetStatus()
    {
        m_myRigid = m_player.GetRigid();
        m_renderer = m_player.GetRenderer();
        m_attack = m_player.GetAtk();
        m_attackDamageDelay = m_player.GetAtkDmgDelay();
        m_attackDelay = m_player.GetAtkDelay();
        m_knockBackPower = m_player.GetKnockBackPower();
        m_maxRange = m_player.GetMaxRange();
        m_cri = m_player.GetCriDmg();
        m_criRate = m_player.GetCriRate();
        m_layerMask = m_player.GetLayerMask();
        m_fireWait = m_player.GetFireWait();
    }

    public override CommandResult Execute()
    {
        // 회피, 이미 공격중이면 Fail
        if (m_blackBoard.GetValueBool(StringData.isDodge) || m_blackBoard.GetValueBool(StringData.isAttack))
            return CommandResult.Failure;

        // 공격 실행
        _mono.StartCoroutine(AttackCo());
        return CommandResult.Success;
    }

    IEnumerator AttackCo()
    {
        m_blackBoard.SetValueBool(StringData.isAttack, true);
        yield return new WaitForSeconds(m_attackDamageDelay);

        PoolType attackEffect = m_player.GetAttackEffect();
        Vector2 t_dir = Vector2.zero;
        t_dir.x = m_renderer.flipX ? -1 : 1;

        // 근접 공격 캐릭터-
        if (m_player.GetIsMelee())
        {
            // 몬스터 검출 시도
            Vector2 t_startCast = m_myRigid.position;
            Vector2 t_size = new Vector2(0.1f, 0.25f);
            RaycastHit2D hitInfo = Physics2D.BoxCast(t_startCast, t_size, 0, t_dir, m_maxRange, m_layerMask);

            // 몬스터 검출시-
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag(StringData.tagMonster))
                {
                    // 타격 이펙트
                    ObjectPoolManager.Instance.GetObjectFromPool(attackEffect, hitInfo.transform.position, true);
                    // 데미지 적용
                    Character t_monster = hitInfo.collider.GetComponent<Character>();
                    t_monster.Hurt(m_player);
                }
            }
        }

        // 원거리 공격 캐릭터-
        else
        {
            PoolType type = m_player.GetProjectileType();
            Vector3 firePos = m_player.GetFirePos();

            yield return new WaitForSeconds(m_fireWait);

            // 발사 이펙트
            ObjectPoolManager.Instance.GetObjectFromPool(attackEffect, firePos, true);
            // 투사체 셋팅
            Projectile t_projectile = ObjectPoolManager.Instance.GetObjectFromPool(type, firePos, false)
                .GetComponent<Projectile>();
            t_projectile.SetProjectile(m_player);
        }
        
        yield return new WaitForSeconds(m_attackDelay - m_attackDamageDelay - m_fireWait);
        m_blackBoard.SetValueBool(StringData.isAttack, false);
    }


}
