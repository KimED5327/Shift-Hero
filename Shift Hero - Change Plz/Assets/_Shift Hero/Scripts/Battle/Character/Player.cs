using UnityEngine;
using System.Collections;

public enum DodgeType
{
    TELEPORT,
    BACK_STEP,
    DEFENCE_STEP,
}

public class Player : Character
{
    [Header("Player Setting")]
    [SerializeField] string m_job = "아쳐";             // 직업 이름
    int m_currentLevel = 1;                             // 현재 레벨
    int m_currentExp = 0;                               // 현재 EXP
    bool m_isOpen = true;                               // 캐릭터 잠금 상태

    [Header("Projectile")]                              
    [SerializeField] bool m_isMelee = true;             // 근접인지 원거리인지
    [SerializeField] PoolType m_projectileType = 0;     // 투사체
    [SerializeField] Transform m_tfFirePos = null;      // 투사체 발사 위치
    [SerializeField] float m_fireWait = 0f;             // 투사체 발사 대기 타임

    [Header("Dodge")]
    [SerializeField] DodgeType m_dodgeType = 0;         // 회피 유형
    [SerializeField] PoolType m_dodgeEffect = 0;        // 회피 이펙트
    [SerializeField] float m_dodgeSpeed = 6f;           // 순간 회피 스피드
    [SerializeField] float m_dodgeDuration = 0.5f;      // 회피 지속 시간
    [SerializeField] bool m_canCounter = false;         // 카운터 여부

    [Header("Level Status")]
    [SerializeField] int m_levelUpHp = 5;               // 레밸업 증가 스테이터스
    [SerializeField] int m_levelUpAtk = 1;
    [SerializeField] int m_levelUpDef = 1;

    [Header("Origin Status")]
    [SerializeField] OriginStatusBuffer _originStatus = null;

    // 위치 가두기
    void Update()
    {
        if (!GameManager.canPlayerMove || m_isDead) return;

        ClampPos();
    }

    // 데미지 받음
    override public void Hurt(Character attacker)
    {
        if (m_isDead) return;                               // 사망 무시
        if (m_canCounter) { Counter(attacker); return; }    // 카운터 발동
        if (TryReflectDamage(attacker)) return;             // 데미지 반사
        

        base.Hurt(attacker);                    // 피격처리
    }

    // 반격 발동
    void Counter(Character attacker)
    {
        m_canCounter = false;
        attacker.Hurt(this);
        ObjectPoolManager.Instance.GetObjectFromPool(PoolType.Counter_Effect, transform.position + Vector3.up * 0.5f ,true);
        AchieveDB.IncreaseCounterCount(1);
        StopCoroutine(nameof(CounterEffect));
        StartCoroutine(nameof(CounterEffect));
    }

    // 반격 연출
    IEnumerator CounterEffect()
    {
        Time.timeScale = 0.35f;
        while(Time.timeScale < 1.0f)
        {
            Time.timeScale += 0.0075f;
            yield return null;
        }
        Time.timeScale = 1.0f;
    }

    // 데미지 반사
    bool TryReflectDamage(Character attacker)
    {
        float random = Random.Range(0f, 1f);
        if (random < BonusAbility.mirrorRate)
        {
            attacker.Hurt(attacker);
            return true;
        }
        return false;
    }



    // 경험치 증가
    public void IncreaseExp(int value)
    {
        m_currentExp += value;

        if (CanLevelUp()) 
            LevelUp();         
    }

    // 체밸업 체크
    bool CanLevelUp()
    {
        int needExp = PlayerDB.GetLevelUpNeedExp(m_currentLevel);
        return (m_currentExp >= needExp);
    }

    // 레밸업
    void LevelUp()
    {
        int needExp = PlayerDB.GetLevelUpNeedExp(m_currentLevel);
        m_currentExp = needExp - m_currentExp;
        m_currentLevel++;
        _originStatus.BufferHp +=  m_levelUpHp;
        _originStatus.BufferAtk += m_levelUpAtk;
        _originStatus.BufferDef += m_levelUpDef;
    }


    // setter
    public void SetMoveSpeed(float value) => m_moveSpeed = value;
    public void SetHp(int value) => m_hp = value;
    public void SetAtk(int value) => m_atk = value;
    public void SetDef(int value) => m_def = value;
    public void SetAttackSpeed(float value) => m_attackDelay = value;
    public void SetCriRate(float value) => m_criRate = value;
    public void SetCriDamage(float value) => m_criDmg = value;
    public void SetRecoverHp(int value) => m_hpRecover = value;
    public void SetRecoverTime(float value) => m_hpRecoverTime = value;
    public void SetAvoidance(float value) => m_avdRate = value;
    public void SetLevel(int value) => m_currentLevel = value;
    public void SetExp(int value) => m_currentExp = value;
    public void SetOriginStatus(OriginStatusBuffer originStatus) => _originStatus = originStatus;
    public void SetIsOpen(bool flag) { m_isOpen = flag; }
    public void SetCounter(bool flag) { m_canCounter = flag; }


    // 플레이어 기본 데이터 세팅
    public void SetPlayerData(PlayerData data)
    {
        m_id = data.id;
        m_name = data.name;
        m_job = data.job;
        m_desc = data.desc;
        m_isOpen = data.isOpen;
        m_currentLevel = data.level;
        m_currentExp = data.exp;
        m_hp = data.hp;
        m_def = data.def;
        m_atk = data.atk;
        m_attackSpeedRate = data.attackSpeed;
        m_maxRange = data.attackRange;
        m_criRate = data.criRate;
        m_criDmg = data.criMultiply;
        m_avdRate = data.avoidanceRate;
        m_moveSpeed = data.moveSpeed;
        m_hpRecover = data.recoverHp;
        m_hpRecoverTime = data.recoverTime;
        m_levelUpHp = data.levelupHp;
        m_levelUpAtk = data.levelupAtk;
        m_levelUpDef = data.levelupDef;
        _originStatus = new OriginStatusBuffer(m_hp, m_atk, m_def, m_moveSpeed, m_attackSpeedRate, m_criRate, m_criDmg, m_avdRate, m_hpRecover, m_hpRecoverTime);
    }

    // getter
    public DodgeType GetDodgeType() { return m_dodgeType; }
    public PoolType GetDodgeEffect() { return m_dodgeEffect; }
    public OriginStatusBuffer GetOriginStatus() { return _originStatus; }
    public PoolType GetProjectileType() { return m_projectileType; }
    public Vector3 GetFirePos() { return m_tfFirePos.position; }

    public int GetLevel() { return m_currentLevel; }
    public string GetClass() { return m_job; }
    public int GetCurrentExp() { return m_currentExp; }
    override public int GetMaxHp() { return m_hp + (int)(BonusAbility.hpRate * _originStatus.BufferHp); }
    override public float GetAvdRate() { return m_avdRate + BonusAbility.avdRate; }
    override public float GetMoveSpeed() { return m_moveSpeed + (int)(BonusAbility.moveSpdRate * _originStatus.BufferMoveSpeed); }
    override public float GetAtkDelay() { return m_attackDelay; }
    override public float GetAtkDmgDelay() { return m_attackDamageDelay; }
    public float GetFireWait() { return m_fireWait; }
    override public float GetCriDmg() { return m_criDmg + (int)(BonusAbility.criDmg * _originStatus.BufferCriDmg); }
    override public float GetCriRate() { return m_criRate + BonusAbility.criRate; }
    override public int GetRecoverHp() { return m_hpRecover + (int)(BonusAbility.recoverHpRate * _originStatus.BufferRecoverHp); }
    override public float GetRecoverTime() { return m_hpRecoverTime; }
    override public int GetAtk() { return m_atk + (int)(BonusAbility.atkRate * _originStatus.BufferAtk); }
    override public int GetDef() { return m_def + (int)(BonusAbility.defRate * _originStatus.BufferDef); }
    public float GetDodgeSpeed() { return m_dodgeSpeed; }
    public float GetDodgeDuration() { return m_dodgeDuration; }
    public int GetLevelUpHp() { return m_levelUpHp; }
    public int GetLevelUpAtk() { return m_levelUpAtk; }
    public int GetLevelUpDef() { return m_levelUpDef; }
    public bool GetIsMelee() { return m_isMelee; }
    public bool GetIsOpen() { return m_isOpen; }
    public bool GetCanCounter() { return m_canCounter; }
}
