using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public int stageID;
    public List<StageMonster> stageMonsterList;
    public List<StageClearReward> stageClearRewardList;
}

[System.Serializable]
public class StageMonster
{
    public int id;
    public int count;
    public string location;
}

[System.Serializable]
public class StageClearReward
{
    public int id;
    public int count;
}

public class StageDB : MonoBehaviour
{
    static Dictionary<int, Stage> _stageDic = new Dictionary<int, Stage>();

    public static void StageAdd(Stage stage)
    {
        _stageDic.Add(stage.stageID, stage);
    }

    public static Stage GetStage(int id)
    {
        if (_stageDic.ContainsKey(id))
        {
            return _stageDic[id];
        }
        else
        {
            Debug.LogError($"{id} 는 없는 키입니다.");
            return null;
        }
    }

}
