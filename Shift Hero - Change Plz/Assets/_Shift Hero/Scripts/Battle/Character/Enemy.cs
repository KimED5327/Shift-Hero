using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: Character
{
    protected Player[] _targets;

    [Header("Attack Range")]
    [SerializeField] protected float m_viewRadius = 5;      // 몬스터 대상 인지 거리
    [SerializeField] protected float m_minRange = 0.5f;     // 최소 공격 범위
    [SerializeField] protected float m_maxAtkDisY = 0.15f;  // 공격 상하 범위

    [Header("Etc")]
    [SerializeField] protected int m_exp = 5;               // 경험치
    [SerializeField] protected int m_gold = 1;              // 골드

    protected BTSequence _root;                             // Behavior Tree AI




    // 플레이어 타겟 세팅
    public void SetLinkPlayer(Player[] players) => _targets = players;

    // 몬스터 데이터 세팅
    public void SetEnemyStatus(int id, Sprite sprite, string name, int exp, int gold, int hp, 
        int def, int atk, float atkSpd, float atkRange, float viewRange, float criRate, 
        float CriDmg, float avoid, float moveSpd, int recover, float recoverTime, string animPath)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        
        GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(animPath);

        this.m_id = id;
        this.m_name = name;
        this.m_exp = exp;
        this.m_gold = gold;
        this.m_hp = hp;
        this.m_def = def;
        this.m_atk = atk;
        this.m_attackSpeedRate = atkSpd;
        this.m_maxRange = atkRange;
        this.m_viewRadius = viewRange;
        this.m_criRate = criRate;
        this.m_criDmg = CriDmg;
        this.m_avdRate = avoid;
        this.m_moveSpeed = moveSpd;
        this.m_hpRecover = recover;
        this.m_hpRecoverTime = recoverTime;

    }

    // 경험치와 골드 드롭
    protected override void Drop()
    {
        BattleData.IncreaseExpCount(m_exp);
        _targets[PlayerController.s_charChoiceIndex].GetCurrentExp();

        int gold = (int)Random.Range(m_gold * 0.8f, m_gold);
        if (gold <= 0)
            gold = 1;
        BattleData.IncreaseGoldCount(gold);
    }


    // AI 실행
    void Update()
    {
        if (!GameManager.canPlayerMove || m_isDead)
        {
            m_anim.SetBool("Walk", false);
            return;
        }

        _root.Execute();

        // 위치 가두기
        ClampPos();
    }


    // getter
    public float GetViewRange() { return m_viewRadius; }
    public float GetMinRange() { return m_minRange; }
    public float GetMaxAtkDisY() { return m_maxAtkDisY; }
    public Player[] GetTargets() { return _targets; }
}
