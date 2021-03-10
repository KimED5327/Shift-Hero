using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    static readonly string LEFT_TOP = "LEFT_TOP", LEFT_CENTER = "LEFT_CENTER", LEFT_BOTTOM = "LEFT_BOTTOM";
    static readonly string MIDDLE_TOP = "MIDDLE_TOP", MIDDLE_CENTER = "MIDDLE_CENTER", MIDDLE_BOTTOM = "MIDDLE_BOTTOM";
    static readonly string RIGHT_TOP = "RIGHT_TOP", RIGHT_CENTER = "RIGHT_CENTER", RIGHT_BOTTOM = "RIGHT_BOTTOM ";

    public static int s_curEnemyCount;  // 몬스터 개체 수


    [Header("Player Pos")]
    [SerializeField] Transform _tfStartingPoint = null;
    [SerializeField] Transform _tfPlayerParent = null;
    Player[] _players;


    [Header("Enemy Base")]
    [SerializeField] GameObject _goEnemyBase = null;    // 일반형


    // 스폰 위치 세팅
    [Header("Enemy Pos")]
    [SerializeField] Vector2 _spawnRange = Vector2.zero;
    [SerializeField] Transform[] _tfEnemyStartingPoints = null;
    [SerializeField] Transform _tfEnemyParent = null;

    CharacterManager _characterManager;
    PlayerController _playerController;
    PlayerSlotStatus _playerStatusHud;

    // Start is called before the first frame update
    void Awake()
    {
        // 컴포넌트 캐싱
        _playerController = FindObjectOfType<PlayerController>();
        _characterManager = FindObjectOfType<CharacterManager>();
        _playerStatusHud = FindObjectOfType<PlayerSlotStatus>();

    }




    // 플레이어 생성 및 플레이어 연결
    public void SetPlayer()
    {

        // 출전 캐릭터 3명 생성
        List<Player> standbyPlayerList = _characterManager.GetStandByPlayers();


        for (int i = 0; i < standbyPlayerList.Count; i++)
        {
            standbyPlayerList[i].transform.SetParent(_tfPlayerParent);
            standbyPlayerList[i].transform.position = _tfStartingPoint.position;
        }

        // 플레이어 컨트롤러에 출전 캐릭터들 전달
        _players = standbyPlayerList.ToArray();
        _playerController.Initialized(_players);
        _playerStatusHud.SetPlayerLink(_players);
    }

    public void SetEnemy(Stage stage)
    {
        s_curEnemyCount = 0;

        // 스폰 몬스터 리스트 개수만큼 반복
        for (int i = 0; i < stage.stageMonsterList.Count; i++)
        {
            // 몬스터 스폰 수만큼 반복
            for(int k = 0; k < stage.stageMonsterList[i].count; k++)
            {
                
                Vector3 pos = GetSpawnPos(stage.stageMonsterList[i].location);

                // 위치 랜덤 세팅 (처음 한 마리만 정확한 위치에, 이후는 랜덤 배치)
                if (k >= 1)
                {
                    pos.x = Random.Range(pos.x, pos.x + _spawnRange.x);
                    pos.y = Random.Range(pos.y, pos.y + _spawnRange.y);
                }

                // 몬스터 생성
                GameObject clone = Instantiate(_goEnemyBase, pos, Quaternion.identity, _tfEnemyParent);

                // 데이터 연결
                MonsterData monsterData = MonsterDB.GetMonsterData(stage.stageMonsterList[i].id);
                Enemy enemy = clone.GetComponent<Enemy>();
                enemy.SetLinkPlayer(_players);
                enemy.SetEnemyStatus(monsterData.id, monsterData.sprite, monsterData.name, monsterData.exp, monsterData.gold, monsterData.hp,
                    monsterData.def, monsterData.atk, monsterData.attackSpeed, monsterData.attackRange, monsterData.viewRange,
                    monsterData.criRate, monsterData.criMultiply, monsterData.avoidanceRate, monsterData.moveSpeed,
                    monsterData.recoverHp, monsterData.recoverTime, monsterData.animPath);
                s_curEnemyCount++;
            }
        }
    }

    Vector3 GetSpawnPos(string loc)
    {
        // <좌측,가운데,우측> 스폰 위치 설정
        if (string.Equals(loc, LEFT_TOP))
            return _tfEnemyStartingPoints[0].position;
        else if (string.Equals(loc, LEFT_CENTER))
            return _tfEnemyStartingPoints[1].position;
        else if (string.Equals(loc, LEFT_BOTTOM))
            return _tfEnemyStartingPoints[2].position;
        else if (string.Equals(loc, MIDDLE_TOP))
            return _tfEnemyStartingPoints[3].position;
        else if (string.Equals(loc, MIDDLE_CENTER))
            return _tfEnemyStartingPoints[4].position;
        else if (string.Equals(loc, MIDDLE_BOTTOM))
            return _tfEnemyStartingPoints[5].position;
        else if (string.Equals(loc, RIGHT_TOP))
            return _tfEnemyStartingPoints[6].position;
        else if (string.Equals(loc, RIGHT_CENTER))
            return _tfEnemyStartingPoints[7].position;
        else if (string.Equals(loc, RIGHT_BOTTOM))
            return _tfEnemyStartingPoints[8].position;
        else
            Debug.LogError("이상한 스폰 장소 = " + loc);

        return _tfEnemyStartingPoints[1].position;
    }

    /// <summary>
    /// 플레이어 위치 초기화
    /// </summary>
    public void ResetPlayerPos()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].transform.position = _tfStartingPoint.position;
        }
    }

    /// <summary>
    /// 적 남아있는 개수 가져오기
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentEnemyCount()
    {
        return s_curEnemyCount;
    }

    /// <summary>
    /// 게임 출전중인 플레이어들 가져오기
    /// </summary>
    /// <returns></returns>
    public Player[] GetPlayers()
    {
        return _players;
    }

    /// <summary>
    /// 게임 종료 후, 캐릭터 매니저에게 다시 반납
    /// </summary>
    public void SavePlayerResult()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].gameObject.SetActive(false);
            _players[i].Initialized();
            _players[i].transform.SetParent(_characterManager.transform);
        }
    }
}
