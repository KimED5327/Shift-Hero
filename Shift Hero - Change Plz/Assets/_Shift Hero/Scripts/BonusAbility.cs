using UnityEngine;

[System.Serializable]
public class AbilitySaveData
{
    public float hpRate;
    public float atkRate;
    public float atkSpeedRate;
    public float defRate;
    public float ignoreDefRate;
    public float recoverHpRate;
    public float criRate;
    public float criDmg;
    public float avdRate;
    public float moveSpdRate;
    public  float goldRate;
    public  float expRate;
    public  float resurRate;
    public  float mirrorRate;
}

[System.Serializable]
public class BonusAbility
{
    static readonly string path = "/Ability.Dat";

    public static void ApplyBonusAbility(string option, float optionNum)
    {
        switch (option)
        {
            case ItemDB.HP_RATE: hpRate += optionNum; break;
            case ItemDB.ATK_RATE: atkRate += optionNum; break;
            case ItemDB.ATK_SPD_RATE: atkSpeedRate += optionNum; break;
            case ItemDB.DEF_RATE: defRate += optionNum; break;
            case ItemDB.DEF_IGNORE: ignoreDefRate += optionNum; break;
            case ItemDB.CRI_RATE: criRate += optionNum; break;
            case ItemDB.CRI_DMG: criDmg += optionNum; break;
            case ItemDB.AVD_RATE: avdRate += optionNum; break;
            case ItemDB.REC_RATE: recoverHpRate += optionNum; break;
            case ItemDB.EXP_RATE: expRate += optionNum; break;
            case ItemDB.GOLD_RATE: goldRate += optionNum; break;
            case ItemDB.BONUS_RATE: expRate += optionNum; goldRate += optionNum; break;
            case ItemDB.RESUR_RATE: resurRate += optionNum; break;
            case ItemDB.MIRROR_RATE: mirrorRate += optionNum; break;
            case ItemDB.MOVE_SPD_RATE: moveSpdRate += optionNum; break;
        }

        SaveAbility();
    }

    public static float hpRate;

    public static float atkRate;
    public static float atkSpeedRate;

    public static float defRate;
    public static float ignoreDefRate;
    public static float recoverHpRate;

    public static float criRate;
    public static float criDmg;

    public static float avdRate;
    public static float moveSpdRate;

    public static float goldRate;
    public static float expRate;
    public static float resurRate;
    public static float mirrorRate;

    public static void SaveAbility()
    {
        AbilitySaveData ability = new AbilitySaveData()
        {
            hpRate = hpRate,
            atkRate = atkRate,
            atkSpeedRate = atkSpeedRate,
            defRate = defRate,
            ignoreDefRate = ignoreDefRate,
            recoverHpRate = recoverHpRate,
            criRate = criRate,
            criDmg = criDmg,
            avdRate = avdRate,
            moveSpdRate = moveSpdRate,
            goldRate = goldRate,
            expRate = expRate,
            resurRate = resurRate,
            mirrorRate = mirrorRate
        };

        SaveData<AbilitySaveData>.DataSave(ability, path);
    }

    public static void LoadAbility()
    {
        AbilitySaveData ability = SaveData<AbilitySaveData>.DataLoad(path);

        if(ability != null)
        {
            hpRate = ability.hpRate;
            atkRate = ability.atkRate;
            atkSpeedRate = ability.atkSpeedRate;
            defRate = ability.defRate;
            ignoreDefRate = ability.ignoreDefRate;
            recoverHpRate = ability.recoverHpRate;
            criRate = ability.criRate;
            criDmg = ability.criDmg;
            avdRate = ability.avdRate;
            moveSpdRate = ability.moveSpdRate;
            goldRate = ability.goldRate;
            expRate = ability.expRate;
            resurRate = ability.resurRate;
            mirrorRate = ability.mirrorRate;
        }
        else
        {
            SaveAbility();
        }


    }

}
