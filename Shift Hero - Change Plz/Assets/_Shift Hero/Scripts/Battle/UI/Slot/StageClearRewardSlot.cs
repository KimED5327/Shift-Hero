using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearRewardSlot : Slot<StageResult>
{
    [SerializeField] Image _imgIcon = null;
    [SerializeField] Text _txtCount = null;

    public override void ClearSlot()
    {
        _id = -1;
        _isSet = false;
        gameObject.SetActive(false);
    }

    public override void OnClickSlot()
    {
        ;
    }

    public void SetSlot(int id, int count, StageResult parent)
    {
        _id = id;
        _isSet = true;
        _parent = parent;

        Item item = ItemDB.GetItem(id);
        _imgIcon.sprite = item.sprite;
        _txtCount.text = $"x {string.Format("{0:###,0}", count)}";

        gameObject.SetActive(true);

    }

    public override void SetSlot(int id, StageResult parent)
    {

    }
}
