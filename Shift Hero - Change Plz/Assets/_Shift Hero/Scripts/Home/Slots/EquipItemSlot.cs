using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemSlot : Slot<EquipInvenMenu>
{
    [SerializeField] Image _imgItem = null;

    public override void ClearSlot()
    {
        _imgItem.gameObject.SetActive(false);
    }

    public override void OnClickSlot()
    {
        ;
    }

    public override void SetSlot(int id, EquipInvenMenu parent = null)
    {
        _parent = parent;

        if(id > 0)
        {
            _imgItem.gameObject.SetActive(true);
            _imgItem.sprite = ItemDB.GetItem(id).sprite;
        }
        else
        {
            _imgItem.gameObject.SetActive(false);
        }

    }
}
