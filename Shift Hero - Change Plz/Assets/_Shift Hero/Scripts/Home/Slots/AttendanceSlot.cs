using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceSlot : Slot<Attendance>
{
    [SerializeField] GameObject _goScreen = null;
    [SerializeField] GameObject[] _goItems = null;
    [SerializeField] Image[] _imgItem = null;
    [SerializeField] Text[] _txtItemCount = null;
    [SerializeField] Text _txtSlotName = null;
    [SerializeField] GameObject _goGetReward = null;

    Rewards _rewards;

    /// <summary>
    /// 출석 보상 처리를 위한 데이터 전달
    /// </summary>
    public override void OnClickSlot()
    {
        _parent.CallSlot(_rewards);
        _goScreen.SetActive(true);
        _goGetReward.SetActive(true);
    }


    public void SetAlreadyGetReward(bool flag)
    {
        _goGetReward.SetActive(flag);
    }

    /// <summary>
    /// 보상 rewards 세팅
    /// </summary>
    /// <param name="rewards"></param>
    public void SetReward(int id, Rewards rewards, Attendance parent)
    {
        _id = id;
        _parent = parent;
        _txtSlotName.text = $"{_id + 1} 일차";
        _goScreen.SetActive(true);

        _rewards = rewards;

        for(int i = 0; i < _goItems.Length; i++)
        {
            if(i < _rewards.reward.Length)
            {
                Item item = ItemDB.GetItem(_rewards.reward[i].id);
                _imgItem[i].sprite = item.sprite;
                _txtItemCount[i].text = string.Format("{0:###,0}", _rewards.reward[i].count);
                _goItems[i].SetActive(true);
            }
            else
            {
                _goItems[i].SetActive(false);
            }
        }
    }

    public void SetScreen(bool flag)
    {
        _goScreen.SetActive(flag);
    }


    public override void ClearSlot()
    {

    }
    public override void SetSlot(int id, Attendance parent)
    {
        ;
    }
}
