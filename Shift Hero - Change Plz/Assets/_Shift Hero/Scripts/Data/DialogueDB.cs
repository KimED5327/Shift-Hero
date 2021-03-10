using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDB 
{
    static readonly string path = "/EventData.dat";
    static readonly int GAME_START = 0, STAGE_START = 1, STAGE_FINISH = 2;

    static Dictionary<int, Dialogue> _dialogueDic = new Dictionary<int, Dialogue>();


    static List<DialogueEvent> _eventList = new List<DialogueEvent>();


    // 대사 추가
    public static void AddDialogue(Dialogue dialogue)
    {
        _dialogueDic.Add(dialogue.id, dialogue);
    }

    // 대화 이벤트 추가
    public static void AddDialogueEvent(DialogueEvent dialogueEvent)
    {
        _eventList.Add(dialogueEvent);
    }

    // 대사 정보 가져오기
    public static Dialogue GetDialogue(int id)
    {
        if (_dialogueDic.ContainsKey(id))
        {
            return _dialogueDic[id];
        }
        else
        {
            Debug.LogError(id + " 는 없는 key값 입니다.");
            return null;
        }
    }

    // 이벤트 정보 가져오기
    public static DialogueEvent GetDialogueEvent(int id)
    {
        if (_eventList.Count > id)
        {
            return _eventList[id];
        }
        else
        {
            Debug.LogError(id + " 는 없는 key값 입니다.");
            return null;
        }
    }

    // 게임 시작시 호출. 조건에 매칭되는 이벤트 가져오기
    public static DialogueEvent GetMatchedGameStartEvent()
    {
        for (int i = 0; i < _eventList.Count; i++)
        {
            if (_eventList[i].isShow) continue;
            if (_eventList[i].condition.type == GAME_START)
            {
                if (_eventList[i].condition.option == AchieveDB.GetBattleCount())
                {
                    _eventList[i].isShow = true;
                    SaveEventData();
                    return _eventList[i];
                }

            }
        }
        return null;
    }

    // 스테이지 시작시 호출. 조건에 매칭되는 이벤트 가져오기
    public static DialogueEvent GetMatchedStageStartEvent()
    {
        for (int i = 0; i < _eventList.Count; i++)
        {
            if (_eventList[i].isShow) continue;

            if (_eventList[i].condition.type == STAGE_START)
            {
                if (_eventList[i].condition.option == GameManager.stageNum)
                {
                    _eventList[i].isShow = true;
                    SaveEventData();
                    return _eventList[i];
                }
            }
        }
        return null;
    }


    // 스테이지 종료시 호출. 조건에 매칭되는 이벤트 가져오기
    public static DialogueEvent GetMatchedStageFinishEvent()
    {
        for (int i = 0; i < _eventList.Count; i++)
        {
            if (_eventList[i].isShow) continue;

            if (_eventList[i].condition.type == STAGE_FINISH)
            {
                if (_eventList[i].condition.option == GameManager.stageNum)
                {
                    _eventList[i].isShow = true;
                    SaveEventData();
                    return _eventList[i];
                }
            }
        }
        return null;
    }


    // 이벤트 정보 세이브
    public static void SaveEventData()
    {
        bool[] isShows = new bool[_eventList.Count];
        for (int i = 0; i < isShows.Length; i++)
        {
            isShows[i] = _eventList[i].isShow;
        }

        SaveData<bool[]>.DataSave(isShows, path);
    }

    // 이벤트 정보 로드
    public static void LoadEventData()
    {
        bool[] isShows = SaveData<bool[]>.DataLoad(path);

        if (isShows == null)
            SaveEventData();
        else
        {
            for (int i = 0; i < _eventList.Count; i++)
            {
                if (i >= isShows.Length)
                    _eventList[i].isShow = false;
                else
                    _eventList[i].isShow = isShows[i];
            }
        }
    }

}
