[System.Serializable]
public class OriginStatusBuffer
{
    public int _bufferHp;
    public int _bufferAtk;
    public int _bufferDef;
    float _bufferMoveSpeed;
    float _bufferAttackSpeed;
    float _bufferCriticalRate;
    float _bufferCriticalDamage;
    float _bufferAvoidanceRate;
    float _bufferRecoverHp;
    float _bufferRecoverTime;

    public OriginStatusBuffer()
    {
        _bufferHp = 0;
        _bufferAtk = 0;
        _bufferDef = 0;
        _bufferMoveSpeed = 0;
        _bufferAttackSpeed = 0;
        _bufferCriticalRate = 0;
        _bufferCriticalDamage = 0;
        _bufferAvoidanceRate = 0;
        _bufferRecoverHp = 0;
        _bufferRecoverTime = 0;
    }

    public OriginStatusBuffer(int bufferHp, int bufferAtk, int bufferDef, float bufferMoveSpeed, float bufferAttackSpeed, float bufferCriticalRate, float bufferCriticalDamage, float bufferAvoidanceRate, float bufferRecoverHp, float bufferRecoverTime)
    {
        _bufferHp = bufferHp;
        _bufferAtk = bufferAtk;
        _bufferDef = bufferDef;
        _bufferMoveSpeed = bufferMoveSpeed;
        _bufferAttackSpeed = bufferAttackSpeed;
        _bufferCriticalRate = bufferCriticalRate;
        _bufferCriticalDamage = bufferCriticalDamage;
        _bufferAvoidanceRate = bufferAvoidanceRate;
        _bufferRecoverHp = bufferRecoverHp;
        _bufferRecoverTime = bufferRecoverTime;
    }

    public int BufferHp { get => _bufferHp; set => _bufferHp = value; }
    public int BufferAtk { get => _bufferAtk; set => _bufferAtk = value; }
    public int BufferDef { get => _bufferDef; set => _bufferDef = value; }
    public float BufferMoveSpeed { get => _bufferMoveSpeed; set => _bufferMoveSpeed = value; }
    public float BufferAttackSpeed { get => _bufferAttackSpeed; set => _bufferAttackSpeed = value; }
    public float BufferCriticalRate { get => _bufferCriticalRate; set => _bufferCriticalRate = value; }
    public float BufferCriDmg { get => _bufferCriticalDamage; set => _bufferCriticalDamage = value; }
    public float BufferAvoidanceRate { get => _bufferAvoidanceRate; set => _bufferAvoidanceRate = value; }
    public float BufferRecoverHp { get => _bufferRecoverHp; set => _bufferRecoverHp = value; }
    public float BufferRecoverTime { get => _bufferRecoverTime; set => _bufferRecoverTime = value; }
}
