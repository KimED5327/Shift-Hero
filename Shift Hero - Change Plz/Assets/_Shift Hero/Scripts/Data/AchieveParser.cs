using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0     학살에도 정도가 있나요	        몬스터 count마리 처치
//1	    티끌 모아 태산	                누적 골드 획득량 count G 달성
//2	    몬스터 박사	                    수집한 몬스터 도감 count 달성
//3	    수집가	                        수집한 아이템 도감 count 달성
//4	    동료의 소중함                 	동료의 수 count 달성
//5	    저승 체험의 즐거움	            전멸 횟수 count 달성
//6	    스릴의 전율	                    회피 카운터 count 성공
//7	    체인지 플리즈	                    교대 횟수 count 달성
//8	    성실왕	                        출석 횟수 count 달성
//9	    정상까지 코앞이다	                클리어한 스테이지 수 count 달성
//10    무한 도전	                    도전 과제 count 개 달성

public class AchieveParser
{

    const int KILL_COUNT = 0, GOLD_COUNT = 1, MONSTER_COUNT = 2,
              ITEM_COUNT = 3, PLAYER_COUNT = 4, DIE_COUNT = 5,
              COUNTER_COUNT = 6, CHANGE_COUNT = 7, ATTENDANCE_COUNT = 8,
              CLEAR_COUNT = 9, CHALLENGE_COUNT = 10;


    public static int GetAchieveCount(int achieveID)
    {
        switch (achieveID)
        {
            case KILL_COUNT: return AchieveDB.GetMonsterKills();
            case GOLD_COUNT: return AchieveDB.GetGoldCount();
            case MONSTER_COUNT: return AchieveDB.GetMonsterGetListCount();
            case ITEM_COUNT: return AchieveDB.GetItemGetListCount();
            case PLAYER_COUNT: return AchieveDB.GetPlayerGetCount();
            case DIE_COUNT: return AchieveDB.GetPlayerAllDieTotalCount();
            case COUNTER_COUNT:return AchieveDB.GetCounterTotalCount();
            case CHANGE_COUNT: return AchieveDB.GetChangeTotalCount();
            case ATTENDANCE_COUNT: return AchieveDB.GetAttendanceCount();
            case CLEAR_COUNT: return AchieveDB.GetClearStageCount();
            case CHALLENGE_COUNT: return AchieveDB.GetAchieveClearCount();
            default: return 0;
        }
    }
}
