using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNumInfo : MonoBehaviour
{
    [SerializeField] Text _txtCurrentStage1 = null;
    [SerializeField] Text _txtCurrentStage2 = null;

    public void SetStageInfo(int stageNum)
    {
        _txtCurrentStage1.text = (stageNum + 1).ToString(); 
        _txtCurrentStage2.text = (stageNum + 1).ToString();
    }
}
