using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achieve
{
    public int id;
    public Sprite sprite;
    public string title;
    public string desc;
    public int clearCount;

    public Achieve(int id, Sprite sprite, string title, string desc, int clearCount)
    {
        this.id = id;
        this.sprite = sprite;
        this.title = title;
        this.desc = desc;
        this.clearCount = clearCount;
    }
}

[System.Serializable]
public class AchieveDetail
{
    public int num;
    public int count;
    public List<AchieveReward> rewards;
}

[System.Serializable]
public class AchieveReward
{
    public int rewardID;
    public int rewardCount;
}

// 업적
[System.Serializable]
public class AchieveContents
{
    public int achieveTotalClearCount;  // 업적 클리어 수
    public int goldCount;               // 골드 누적 획득량
    public int monsterKills;            // 몬스터 누적 킬수
    public int attendanceCount;         // 출석 누적 수
    public int clearMaxStage;           // 최대 스테이지 클리어 수
    public int playerGetCount;          // 캐릭터 획득 수
    public int changeCount;             // 배틀 교대 누적 수
    public int playerAllDieCount;       // 전멸 누적 수
    public int counterCount;            // 카운트 누적 수
    public int battleCount;             // 전투 참여수

    public List<int> achieveClearCountList; // 업적별 클리어 카운트 정보
    public List<int> monsterIDGetList;  // 죽인 몬스터 ID 리스트
    public List<int> itemIDGetList;     // 획득한 아이템 리스트
}





public class AchieveDB : MonoBehaviour
{
    // ----------------------------- 변수 -----------------------------

    static readonly string path = "/Achieve.dat"; // 업적 데이터 저장 

    // 업적 데이터
    static AchieveContents _achieveData = new AchieveContents()
    {
        playerGetCount = 3,
        achieveClearCountList = new List<int>(),
        monsterIDGetList = new List<int>(),
        itemIDGetList = new List<int>()
    };

    // 업적 최대 개수
    static int _achieveMaxCount = 0;

    // 업적 딕셔너리
    static Dictionary<int, Achieve> _achieveDic = new Dictionary<int, Achieve>();

    // 업적별 세부 항목 딕셔너리
    static Dictionary<int, List<AchieveDetail>> _achieveDetailDic = new Dictionary<int, List<AchieveDetail>>();
    
    // ----------------------------- 변수 -----------------------------

    




    // ----------------------------- 함수 -----------------------------
    // 도전과제 추가
    public static void AddAchieve(int id, Achieve achieve)
    {
        _achieveMaxCount++;
        _achieveDic.Add(id, achieve);
    }

    // 도전과제 별 디테일 추가
    public static void AddAchieveDetail(int id, List<AchieveDetail> detailList)
    {
        _achieveDetailDic.Add(id, detailList);
    }

    // 해당 id 값의 도전과제 가져옴
    public static Achieve GetAchieve(int id)
    {
        if (_achieveDic.ContainsKey(id))
        {
            return _achieveDic[id];
        }
        else
        {
            Debug.LogError(id + "는 없는 키입니다.");
            return null;
        }
    }

    /// <param name="id"></param>
    ///     /// <summary>
    /// 도전과제 클리어시 호출. 클리어 횟수를 증가시켜줌
    /// </summary>
    public static void AchieveClear(int id)
    {
        if (_achieveDic.ContainsKey(id))
        {
            _achieveData.achieveTotalClearCount++;
            _achieveDic[id].clearCount++;
            SaveAchieveData();
        }
        else
        {
            Debug.LogError(id + "는 없는 키입니다.");
        }
    }

    /// <summary>
    /// 해당 id를 가진 도전과제 정보의 디테일을 가져옴
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static AchieveDetail GetAchieveDetail(int id, int num)
    {
        if (_achieveDetailDic.ContainsKey(id))
        {
            List<AchieveDetail> detailList = _achieveDetailDic[id];

            for (int i = 0; i < detailList.Count; i++)
            {
                if (detailList[i].num == num)
                {
                    return detailList[i];
                }
            }

            Debug.Log(num + " 값은 세부 디테일 개수를 초과했습니다.");
            return null;
        }
        else
        {
            Debug.LogError(id + "는 없는 키입니다.");
            return null;
        }
    }
    // ----------------------------- 함수 -----------------------------




    // ----------------------------- getter -----------------------------
    public static int GetAchieveClearCount() { return _achieveData.achieveTotalClearCount; }
    public static int GetAchieveMaxCount() { return _achieveMaxCount; }
    public static int GetMonsterKills() { return _achieveData.monsterKills; }
    public static int GetGoldCount() { return _achieveData.goldCount; }
    public static int GetItemGetListCount() { return _achieveData.itemIDGetList.Count; }
    public static int GetMonsterGetListCount() { return _achieveData.monsterIDGetList.Count; }
    public static int GetAttendanceCount() { return _achieveData.attendanceCount; }
    public static int GetClearStageCount() { return _achieveData.clearMaxStage; }
    public static int GetPlayerGetCount() { return _achieveData.playerGetCount; }
    public static int GetCounterTotalCount() { return _achieveData.counterCount; }
    public static int GetChangeTotalCount() { return _achieveData.changeCount; }
    public static int GetBattleCount() { return _achieveData.battleCount; }
    public static int GetPlayerAllDieTotalCount() { return _achieveData.playerAllDieCount; }
    public static MonsterData GetMonsterGetList(int index) { return MonsterDB.GetMonsterData(_achieveData.monsterIDGetList[index]); }
    public static Item GetItemGetList(int value) { return ItemDB.GetItem(_achieveData.itemIDGetList[value]); }
    // ----------------------------- getter -----------------------------




    // ----------------------------- 업적 증가 -----------------------------
    // 교대 횟수 누적
    public static void IncreaseChangeCount(int value)  {
        _achieveData.changeCount += value; 
        SaveAchieveData(); 
    }
    // 전멸 횟수 누적
    public static void IncreaseAllDieCount(int value)  
    {
        _achieveData.playerAllDieCount += value; 
        SaveAchieveData(); 
    }
    // 카운터 횟수 누적
    public static void IncreaseCounterCount(int value)  
    {
        _achieveData.counterCount += value; 
        SaveAchieveData(); 
    }
    // 전투 참여수 누적
    public static void IncreaseBattleCount()
    {
        _achieveData.battleCount++;
        SaveAchieveData();
    }
    // 출석 수 누적
    public static void IncreaseAttendanceCount(int value = 1) 
    { 
        _achieveData.attendanceCount += value;
        SaveAchieveData(); 
    }
    // 골드 누적
    public static void IncreaseGoldCount(int value) 
    { 
        _achieveData.goldCount += value; 
        SaveAchieveData(); 
    }

    // 몬스터 누적 킬 + 새로운 몬스터 기록
    public static void PlusMonsterKill(int id)
    {
        // 처음 잡는 몬스터면 도감 추가
        bool isFirst = true;
        for (int i = 0; i < _achieveData.monsterIDGetList.Count; i++)
        {
            if (_achieveData.monsterIDGetList[i] == id)
            {
                isFirst = false;
                break;
            }
        }
        if(isFirst)
            _achieveData.monsterIDGetList.Add(id);
        
        _achieveData.monsterKills++;        // 몬스터 누적 킬수 증가
        SaveAchieveData();
    }

    public static void SetPlayerGetCount(int value) { _achieveData.playerGetCount = value; }

    // 캐릭터 획득 개수 증가
    public static void IncreasePlayerGetCount(int value = 1) { 
        _achieveData.playerGetCount += value;
        SaveAchieveData();
    }

    // 스테이지 기록
    public static void RecordClearStage(int stageNum)
    {
        if (_achieveData.clearMaxStage < stageNum)
            _achieveData.clearMaxStage = stageNum;

        SaveAchieveData();
    }
    // 새로운 아이템 기록
    public static void AddGetItemList(int id)
    {
        // 이미 획득한 아이템이 있는지 체크
        for (int i = 0; i < _achieveData.itemIDGetList.Count; i++)
        {
            if (_achieveData.itemIDGetList[i] == id)
                return;
        }

        // 처음 획득한 아이템만 푸시
        _achieveData.itemIDGetList.Add(id);

        SaveAchieveData();
    }
    // ----------------------------- 업적 증가 -----------------------------





    // ----------------------------- 데이터 저장 -----------------------------
    // 업적 데이터 저장
    public static void SaveAchieveData()
    {
        _achieveData.achieveClearCountList.Clear();

        for (int i = 0; i < GetAchieveMaxCount(); i++)
        {
            _achieveData.achieveClearCountList.Add(_achieveDic[i].clearCount);
        }

        SaveData<AchieveContents>.DataSave(_achieveData, path);
    }

    // 업적 데이터 로드
    public static void LoadAchieveData()
    {
        AchieveContents achieveData = SaveData<AchieveContents>.DataLoad(path);

        if(achieveData != null)
        {
            for (int i = 0; i < achieveData.achieveClearCountList.Count; i++)
            {
                if (_achieveDic.ContainsKey(i))
                {
                    _achieveDic[i].clearCount = achieveData.achieveClearCountList[i];
                }
            }
            _achieveData = achieveData;
        }
        else
        {
            SaveAchieveData();
        }
    }
}
