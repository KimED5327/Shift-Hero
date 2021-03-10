using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData
{
    static int battleGetGold;
    static int battleGetExp;


    static int battleCounterCount;
    static int battleKillCount;
    static int battleChangeCount;
    static int battlePlayerHurtCount;
    static int battlePlayerHitCount;
    static int battlePlayerAllDieCount;

    public static void RecordBattle()
    {
        AchieveDB.IncreaseChangeCount(battleChangeCount);
        AchieveDB.IncreaseAllDieCount(battlePlayerAllDieCount);
        AchieveDB.IncreaseCounterCount(battleCounterCount);
    }

    public static int GetKillCount() { return battleKillCount; }
    public static int GetCounterCount() { return battleCounterCount; }
    public static int GetChangeCount() { return battleChangeCount; }
    public static int GetPlayerHurtCount() { return battlePlayerHurtCount; }
    public static int GetPlayerHitCount() { return battlePlayerHitCount; }
    public static int GetPlayerAllDieCount() { return battlePlayerAllDieCount; }
    public static int GetBattleGold() { return battleGetGold; }
    public static int GetBattleExp() { return battleGetExp; }



    public static void IncreaseKillCount(int value = 1)
    {
        battleKillCount += value;
    }
    public static void IncreaseHitCount(int value = 1)
    {
        battlePlayerHitCount += value;
    }
    public static void IncreaseHurtCount(int value = 1)
    {
        battlePlayerHurtCount += value;
    }
    public static void IncreaseAllDieCount(int value = 1)
    {
        battlePlayerHitCount += value;
    }
    public static void IncreaseChangeCount(int value = 1)
    {
        battleChangeCount += value;
    }
    public static void IncreaseCounterCount(int value = 1)
    {
        battleCounterCount += value;
    }
    public static void IncreaseGoldCount(int value = 1)
    {
        battleGetGold += value;
    }
    public static void IncreaseExpCount(int value = 1)
    {
        battleGetExp += value;
    }




    public static void ClearBattleRecord()
    {
        battleCounterCount = 0;
        battleChangeCount = 0;
        battlePlayerHurtCount = 0;
        battlePlayerHitCount = 0;
        battlePlayerAllDieCount = 0;
        battleGetGold = 0;
        battleGetExp = 0;
        battleKillCount = 0;
    }
}
