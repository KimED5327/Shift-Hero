using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] protected int m_id = 0;                    // 유닛 고유 ID
    [SerializeField] protected string m_name = "";              // 유닛 이름
    [SerializeField] protected string m_desc = "";              // 유닛 설명
    [SerializeField] protected int m_hp = 50;                   // 최대 체력
    protected int m_currentHp;                                  // 현재 체력

    [SerializeField] protected int m_atk;                       // 공격력
    [SerializeField] protected int m_def;                       // 방어력
    [Range(0f, 1f)]
    [SerializeField] protected float m_criRate = 0.05f;         // 치명률
    [SerializeField] protected float m_criDmg = 1.5f;           // 치명타 데미지
    [Range(0f, 1f)]
    [SerializeField] protected float m_avdRate;                 // 회피율
    [SerializeField] protected float m_hpRecoverTime = 5;       // 체력 회복시간 5초당 
    [SerializeField] protected int m_hpRecover = 1;             // 체력 회복량 1


    [Header("Move Speed")]
    [SerializeField] protected float m_moveSpeed = 3;           // 이동 스피드
    [SerializeField] protected float m_moveSpeedRate = 1f;      // 이동 속도 %

    [Header("Attack")]
    [SerializeField] protected float m_maxRange = 2;            // 최대 사거리
    [SerializeField] protected float m_attackDelay = 1.5f;      // 공격 인터벌
    [SerializeField] protected float m_attackDamageDelay = 1.5f;// 공격 적용 딜레이
    [SerializeField] protected float m_attackSpeedRate = 1f;    // 공격 속도
    [SerializeField] protected PoolType m_attackEffect = 0;     // 타격 이펙트
    [SerializeField] protected LayerMask m_layerMask = 0;       // 공격 대상 레이어 마스크

    [Header("Hurt")]
    [SerializeField] Transform m_tfHpBar = null;                // hp바
    [SerializeField] protected PoolType m_hurtEffect = 0;       // 피격 이펙트
    [SerializeField] protected float m_knockBackPower = 0.3f;   // 넉백 파워
    [SerializeField] protected float m_knockBackDef = 0f;       // 넉백 저항력
    [SerializeField] protected float m_deadDestroyDelay = 1.5f; // 죽은 뒤 파괴 대기 시간
    float m_knockBackError = 0.025f;                            // 넉백 오류 보정치

    protected bool m_isAttack = false;                          // 공격중
    protected bool m_isDead = false;                            // 사망
    protected bool m_canAttack = false;                         // 공격 가능한지
    protected bool m_isKnockBack = false;                       // 넉백중


    protected Animator m_anim = null;
    protected Rigidbody2D m_myRigid = null;
    protected SpriteRenderer m_render = null;
    protected Collider2D m_col = null;

    Coroutine m_knockBackCo;


    virtual protected void Start()
    {
        m_anim = GetComponent<Animator>();
        m_myRigid = GetComponent<Rigidbody2D>();
        m_render = GetComponent<SpriteRenderer>();
        m_col = GetComponent<Collider2D>();

        Initialized();
    }

    // 초기화
    virtual public void Initialized()
    {
        m_currentHp = m_hp;
        m_isDead = false;
        if (m_col == null) m_col = GetComponent<Collider2D>();
        m_col.enabled = true;
    }



    // 피격
    virtual public void Hurt(Character attacker)
    {
        if (IsDead()) return;   // 사망시 무시
        if (IsMiss()) return;   // 회피시 무시

        ApplyDamage(attacker);  // 데미지 적용

        if (!IsDead())
            CalcKnockBack(attacker); // 생존시 넉백
    }



    // 회피 여부 계산
    virtual protected bool IsMiss()
    {
        float t_random = Random.Range(0f, 1f);
        if (t_random <= GetAvdRate())
        {
            Miss();
            return true;
        }
        return false;
    }



    // 회피 성공
    protected void Miss()
    {
        FloatingText text = ObjectPoolManager.Instance.GetObjectFromPool(PoolType.FloatingText, false).GetComponent<FloatingText>();
        text.MissSetup(transform.position + Vector3.up * 2f);
    }




    // 데미지 적용
    void ApplyDamage(Character attacker)
    {
        // 데미지 계산
        int damage = CalcDamage(attacker.GetAtk());

        // 크리티컬 계산
        bool isCritical = IsCritical(attacker.GetCriRate());
        if (isCritical)
            damage = (int)(damage * attacker.GetCriDmg());
        
        // 체력 감소
        m_currentHp -= damage;
        if (m_currentHp < 0)
            m_currentHp = 0;
        
        // 데미지 연출
        HurtEffect(attacker, damage, isCritical);

        // 체력 0 이하면 사망 판정
        if (m_currentHp <= 0)
            Dead();
    }


    // 데미지 계산
    protected int CalcDamage(int pDamage)
    {
        pDamage -= m_def;
        if (pDamage <= 0)
            pDamage = 1;

        return pDamage;
    }


    // 크리티컬 확률 계산
    bool IsCritical(float criRate)
    {
        float random = Random.Range(0f, 1f);
        return random < criRate;
    }




    // 피격 연출
    protected void HurtEffect(Character attacker, int p_damage, bool isCritical = false)
    {
        // 피격 연출
        ObjectPoolManager.Instance.GetObjectFromPool(m_hurtEffect, m_col.bounds.center, true);

        // 데미지 플로팅 텍스트 출력
        FloatingText text = ObjectPoolManager.Instance.GetObjectFromPool(PoolType.FloatingText, false).GetComponent<FloatingText>();
        if (attacker.gameObject.CompareTag(StringData.tagPlayer))
        {
            BattleData.IncreaseHitCount(1);

            text.Setup(transform.position + Vector3.up * 2f, p_damage, FloatingTextType.WHITE, isCritical);
            // HP바 조절
            if (m_tfHpBar != null)
                m_tfHpBar.localScale = new Vector3((float)m_currentHp / m_hp, 0.8f, 1);
        }
        else
        {
            BattleData.IncreaseHurtCount(1);
            text.Setup(transform.position + Vector3.up * 2f, p_damage, FloatingTextType.RED, isCritical);
        }
    }



    // 넉백 가능한지 계산
    void CalcKnockBack(Character attacker)
    {

        // 넉백 저항 계산 (0 이하면 넉백 무시)
        float knockBackDistance = attacker.GetKnockBackPower() - m_knockBackDef;
        if (knockBackDistance <= 0)
            return;

        // 넉백 방향 및 위치 세팅
        float dirX = attacker.GetRenderer().flipX ? -1 : 1;
        Vector2 knockBackPos = m_myRigid.position;
        knockBackPos.x += dirX > 0 ? knockBackDistance : -knockBackDistance;

        // 넉백 실행
        if (m_knockBackCo != null)
            StopCoroutine(m_knockBackCo);
        m_knockBackCo = StartCoroutine(KnockBack(knockBackPos));
    }


    // 넉백
    IEnumerator KnockBack(Vector2 knockBackPos)
    {
        // 넉백 실행
        m_isKnockBack = true;
        while (Vector2.SqrMagnitude(m_myRigid.position - knockBackPos) >= Mathf.Pow(m_knockBackError, 2))
        {
            yield return null;
            m_myRigid.MovePosition(Vector2.Lerp(m_myRigid.position, knockBackPos, 0.1f));
        }
        m_isKnockBack = false;
    }

    // 이동 공간 제한
    protected void ClampPos()
    {
        Vector3 myPos = transform.position;
        myPos.x = Mathf.Clamp(myPos.x, StageManager.limitPos.x, StageManager.limitPos.y);
        myPos.y = Mathf.Clamp(myPos.y, StageManager.limitPos.z, StageManager.limitPos.w);
        transform.position = myPos;
    }


    // 사망 연출
    protected void Dead()
    {
        m_anim.SetTrigger("Dead");

        // 불필요한 것들 비활성화
        m_col.enabled = false;
        m_isDead = true;
        
        Drop(); // 죽으면서 남길 게 있다면 드롭.

        // 플레이어가 죽은 경우 움직임 제한
        if (gameObject.CompareTag(StringData.tagPlayer))
            GameManager.canPlayerMove = false;
        // 몬스터가 죽은 경우 1킬 증가
        else
        {
            AchieveDB.PlusMonsterKill(m_id);
            BattleData.IncreaseKillCount(1);
        }

        // 시체 파괴
        Invoke(nameof(DestroyDeadBody), m_deadDestroyDelay);
    }



    // 죽으면서 드롭할 게 있다면, 자식 클래스에서 구현.
    virtual protected void Drop()
    {
        ;
    }



    // 체력 회복
    public void RecoverHp(int p_num)
    {
        m_currentHp += p_num;
        if (m_currentHp >= m_hp)
            m_currentHp = m_hp;
    }




    // 시체 삭제
    protected void DestroyDeadBody()
    {
        StopAllCoroutines();

        // 몬스터가 죽으면 제거
        if (gameObject.CompareTag(StringData.tagMonster))
        {
            UnitManager.s_curEnemyCount--;
            Destroy(gameObject);
        }
    }



    // getter
    public PoolType GetAttackEffect() { return m_attackEffect; }
    public LayerMask GetLayerMask() { return m_layerMask; }
    public Animator GetAnimator()
    {
        if (m_anim == null) m_anim = GetComponent<Animator>();
        return m_anim;
    }
    public Rigidbody2D GetRigid()
    {
        if (m_myRigid == null) m_myRigid = GetComponent<Rigidbody2D>();
        return m_myRigid;
    }
    public SpriteRenderer GetRenderer()
    {
        if (m_render == null) m_render = GetComponent<SpriteRenderer>();
        return m_render;
    }
    public int GetID() { return m_id; }
    public string GetName() { return m_name; }
    public bool IsDead() { return m_isDead; }
    virtual public float GetMoveSpeed() { return m_moveSpeed; }
    virtual public int GetMaxHp() { return m_hp; }
    public int GetCurrentHP() { return m_currentHp; }
    virtual public int GetAtk() { return m_atk; }
    virtual public int GetDef() { return m_def; }
    virtual public float GetCriDmg() { return m_criDmg; }
    virtual public float GetCriRate() { return m_criRate; }
    virtual public int GetRecoverHp() { return m_hpRecover; }
    virtual public float GetRecoverTime() { return m_hpRecoverTime; }
    virtual public float GetAvdRate() { return m_avdRate; }
    virtual public float GetAtkDelay() { return m_attackDelay; }
    virtual public float GetAtkDmgDelay() { return m_attackDamageDelay; }
    public float GetKnockBackPower() { return m_knockBackPower; }
    public float GetMaxRange() { return m_maxRange; }
   
    
}
