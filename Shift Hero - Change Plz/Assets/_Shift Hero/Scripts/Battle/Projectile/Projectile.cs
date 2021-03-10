using UnityEngine;

public class Projectile : MonoBehaviour
{
    Character _attacker;
    Vector3 m_dir = new Vector3();

    string m_targetTag = "";
    [SerializeField] PoolType m_myType = 0;
    [SerializeField] float m_speed = 5f;
    [SerializeField] bool m_isPenetration = false;
    [SerializeField] PoolType m_hitEffect = 0;

    SpriteRenderer _renderer;

    void Awake() => _renderer = GetComponent<SpriteRenderer>();
    

    // 투사체 세팅
    public void SetProjectile(Character attacker)
    {
        _attacker = attacker;

        // 투사체 방향 세팅
        bool filpX = attacker.GetRenderer().flipX;
        if (_renderer != null)
            _renderer.flipX = !filpX;
        m_dir.x = filpX ? -1 : 1;

        // 타겟 세팅
        m_targetTag = attacker.gameObject.CompareTag(StringData.tagPlayer)
                    ? StringData.tagMonster
                    : StringData.tagPlayer;

        gameObject.SetActive(true);
    }


    // 이동
    void Update()
    {
        transform.position += m_dir * m_speed * Time.deltaTime;
    }




    // 적중
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(m_targetTag))
        {
            collision.GetComponent<Character>().Hurt(_attacker);

            if (!m_isPenetration)
            {
                ObjectPoolManager.Instance.GetObjectFromPool(m_hitEffect, transform.position, true);
                ObjectPoolManager.Instance.PushObjectAtPool(m_myType, this.gameObject);
            }
        }
    }
}
