using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

[System.Serializable]
public class Rewards
{
    public Reward[] reward;
}

[System.Serializable]
public class Reward
{
    public int id;
    public int count;
}

public class Attendance : MonoBehaviour
{
    [SerializeField] Rewards[] _rewards;                // 일일차 보상 정보

    [SerializeField] AttendanceSlot[] _slots = null;    // 보상 슬롯

    string loginDate;                                   // 접속 시간
    int _curRewardNum = 0;                              // 출석 체크 차수

    string _url = "http://naver.com";                   // 링크

    Inventory _inven;


    void Awake() => _inven = FindObjectOfType<Inventory>();
    void Start()
    {
        // 슬롯 초기화
        Initialize_AttendanceSlot();

        // 시간 체크
        StartCoroutine(NetworkTimeCheck());
    }

    // 출석 슬롯 초기화
    void Initialize_AttendanceSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetReward(i, _rewards[i], this);   // 슬롯 출석 보상 세팅.
        }
    }


    // 접속 시간 체크
    IEnumerator NetworkTimeCheck()
    {

        UnityWebRequest request = new UnityWebRequest();

        // 자동 Dispose를 위한 using 사용
        using (request = UnityWebRequest.Get(_url))
        {
            yield return request.SendWebRequest();

            // 네트워크 오류 발생
            if (request.isNetworkError)
            {
                Message.instance.ShowMsg(StringData.errorMsgNetwork);
                yield break;
            }

            // 헤더의 date 정보만 Get
            loginDate = request.GetResponseHeader("date");

            // 기존 접속 로그인 기록이 있다면
            if (PlayerPrefs.HasKey("LastAccessTime"))
            {

                _curRewardNum = PlayerPrefs.GetInt("RewardCount");  // 마지막 출석 차수 캐싱
                int getReward = PlayerPrefs.GetInt("GetReward");    // 최신 차수 출석 보상 획득 유무

                // 접속한 날짜 차이 계산
                DateTime dateNow = DateTime.Parse(loginDate);
                DateTime dateLastTime = DateTime.Parse(PlayerPrefs.GetString("LastAccessTime"));
                TimeSpan span = dateNow - dateLastTime;

                if(span.Days == 0 && getReward == 0) // 당일 접속이고 아직 안 받은 상태면 같은 보상 오픈
                    ReOpenReward();

                else if (0 < span.Days && span.Days < 2) // 하루 차이면 다음 일차 보상 오픈
                    GiveReward();

                else if (span.Days >= 2)             // 연속 출석 실패 - 초기화 (1일차 보상 오픈)
                    RewardInitalized();

                else                                // 당일 접속인데 보상을 받은 상태
                    CheckAlreadyGetRewardSlots();
                
            }

            // 최초 실행으로 인한 초기화
            else
            {
                RewardInitalized();
            }  
            
        }
    }
       


    // 지난 번 접속때 보상을 안 받았고, 같은 날 접속한 상태라면 다시 보상 오픈
   void ReOpenReward()
    {
        PlayerPrefs.SetString("LastAccessTime", loginDate);

        _slots[_curRewardNum].SetScreen(false);
        CheckAlreadyGetRewardSlots();
    }



    // 출석 성공으로 인한 다음 차수 보상 오픈
    void GiveReward()
    {
        PlayerPrefs.SetString("LastAccessTime", loginDate);
        PlayerPrefs.SetInt("GetReward", 0);

        _slots[_curRewardNum].SetScreen(false);
        CheckAlreadyGetRewardSlots();
    }



    // 연속 출석 실패로 인한 보상 초기화
    void RewardInitalized()
    {
        PlayerPrefs.SetString("LastAccessTime", loginDate);
        PlayerPrefs.SetInt("RewardCount", 0);
        PlayerPrefs.SetInt("GetReward", 0);

        _curRewardNum = 0;

        _slots[_curRewardNum].SetScreen(false);
        CheckAlreadyGetRewardSlots();
    }



    // 보상 획득 버튼 클릭시
    public void CallSlot(Rewards rewards)
    {
        // 보상 획득
        Reward[] reward = _rewards[_curRewardNum].reward;
        for (int i = 0; i < reward.Length; i++)
        {
            _inven.AcquireItem(reward[i].id, reward[i].count);
        }

        // 차수 증가
        _curRewardNum++;
        AchieveDB.IncreaseAttendanceCount();

        // 저장
        PlayerPrefs.SetInt("RewardCount", _curRewardNum);
        PlayerPrefs.SetInt("GetReward", 1);
    }



    // 이미 획득한 일차는 체크 처리
    void CheckAlreadyGetRewardSlots()
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            if (i < _curRewardNum )
                _slots[i].SetAlreadyGetReward(true);
            else
                _slots[i].SetAlreadyGetReward(false);
        }
    }

}