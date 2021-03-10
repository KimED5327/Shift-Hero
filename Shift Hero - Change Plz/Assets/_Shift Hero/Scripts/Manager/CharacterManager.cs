using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int id;
    public int level;
    public int curExp;
    public bool isOpen;
    public OriginStatusBuffer originStatus;
}



public class CharacterManager : MonoBehaviour
{
    static readonly string path = "/player.dat"; // 플레이어 데이터 저장 경로
    const int MAX_STANDBY_CHARACTERS = 3; // 최대 출전 가능 캐릭터

    Player[] _enrolledCharacters;                   // 모든 캐릭터
    int[] _standByCharactersID = null;              // 스탠바이 캐릭터 ID값 저장

    EquipManager _equip;                            // 등록된 모든 캐릭터의 수만큼 장비슬롯 세팅



    // 초기화
    void Awake()
    {
        Initialize_Character();             // 캐릭터 로드 및 데이터 세팅
        Initialize_StandByCharacter();      // 스탠바이 캐릭터 세팅
        Initialize_EquipSlotSetting();      // 캐릭터 수만큼 장비슬롯 세팅
    }

    // 캐릭터 세팅
    void Initialize_Character()
    {
        // 캐릭터 등록
        _enrolledCharacters = GetComponentsInChildren<Player>(true);
        for (int i = 0; i < _enrolledCharacters.Length; i++)
            _enrolledCharacters[i].SetPlayerData(PlayerDB.GetPlayerData(i));

        // 데이터 로드
        LoadPlayerData();
    }

    // 스탠바이 캐릭터 세팅
    void Initialize_StandByCharacter()
    {
        // 스탠바이 수 세팅
        _standByCharactersID = new int[MAX_STANDBY_CHARACTERS];

        // 저장된 값이 있으면 저장된 값으로 세팅
        if (PlayerPrefs.HasKey(StringData.prefStandBy + 0))
            for (int i = 0; i < _standByCharactersID.Length; i++)
                _standByCharactersID[i] = PlayerPrefs.GetInt(StringData.prefStandBy + i);

        // 없으면 기본 캐릭터로 세팅
        else
            for (int i = 0; i < _standByCharactersID.Length; i++)
            {
                _standByCharactersID[i] = i;
                PlayerPrefs.SetInt(StringData.prefStandBy + i, i);
            }

    }

    // 플레이어 수만큼 장비 슬롯 세팅
    void Initialize_EquipSlotSetting()
    {
        _equip = FindObjectOfType<EquipManager>();
        _equip.SetEquipList(_enrolledCharacters.Length);
    }


    /// <summary>
    /// 출전 대기 슬롯에 캐릭터 등록
    /// </summary>
    /// <param name="index"></param>
    /// <param name="id"></param>
    public void SetStandBySlot(int index, int id)
    {
        _standByCharactersID[index] = id;
        PlayerPrefs.SetInt(StringData.prefStandBy + index, id);
    }

    /// <summary>
    /// 출전 대기 슬롯에 등록된 캐릭터들 모두 가져옴
    /// </summary>
    /// <returns></returns>
    public int[] GetStandbyCharactersID() { return _standByCharactersID; }

    /// <summary>
    /// index 슬롯에 등록된 캐릭터 정보 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Player GetStandbyCharacter(int index) {

        int id = _standByCharactersID[index];
        for (int i = 0; i < _enrolledCharacters.Length; i++)
        {
            if (_enrolledCharacters[i].GetID() == id)
            {
                return _enrolledCharacters[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 특정 id값을 가진 캐릭터 가져오기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Player GetPlayerFromID(int id)
    {
        for (int i = 0; i < _enrolledCharacters.Length; i++)
        {
            if (_enrolledCharacters[i].GetID() == id)
            {
                return _enrolledCharacters[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 스탠바이 캐릭터들 가져오기
    /// </summary>
    /// <returns></returns>
    public List<Player> GetStandByPlayers()
    {

        List<Player> standbyList = new List<Player>();

        for (int i = 0; i < _standByCharactersID.Length; i++)
        {
            standbyList.Add(GetStandbyCharacter(_standByCharactersID[i]));
        }

        return standbyList;
    }

    /// <summary>
    /// 전투에 참여한 플레이어의 데이터를 전달 후 저장
    /// </summary>
    /// <param name="player"></param>
    /// <param name="id"></param>
    public void SavePlayer(Player player, int id)
    {
        for (int i = 0; i < _enrolledCharacters.Length; i++)
        {
            if(_enrolledCharacters[i].GetID() == id)
            {
                _enrolledCharacters[i] = player;
            }
        }

        SavePlayerData();
    }

    /// <summary>
    /// 플레이어 데이터 세이브
    /// </summary>
    public void SavePlayerData() {

        PlayerSaveData[] playerSaveData = new PlayerSaveData[_enrolledCharacters.Length];
        for (int i = 0; i < playerSaveData.Length; i++)
        {
            playerSaveData[i] = new PlayerSaveData()
            {
                id = _enrolledCharacters[i].GetID(),
                curExp = _enrolledCharacters[i].GetCurrentExp(),
                level = _enrolledCharacters[i].GetLevel(),
                isOpen = _enrolledCharacters[i].GetIsOpen(),
                originStatus = _enrolledCharacters[i].GetOriginStatus()
            };
        }
        SaveData<PlayerSaveData[]>.DataSave(playerSaveData, path); 
    }


    /// <summary>
    /// 플레이어 데이터 로드
    /// </summary>
    public void LoadPlayerData() {
        PlayerSaveData[] playerSaveData = SaveData<PlayerSaveData[]>.DataLoad(path);

        if(playerSaveData != null)
        {
            for (int i = 0; i < playerSaveData.Length; i++)
            {
                for (int k = 0; k < _enrolledCharacters.Length; k++)
                {
                    if (playerSaveData[i].id == _enrolledCharacters[k].GetID())
                    {
                        _enrolledCharacters[k].SetLevel(playerSaveData[i].level);
                        _enrolledCharacters[k].SetExp(playerSaveData[i].curExp);
                        _enrolledCharacters[k].SetIsOpen(playerSaveData[i].isOpen);
                        _enrolledCharacters[k].SetOriginStatus(playerSaveData[i].originStatus);
                        break;
                    }
                }
            }
        }
        else
        {
            SavePlayerData();
        }
    }

}
