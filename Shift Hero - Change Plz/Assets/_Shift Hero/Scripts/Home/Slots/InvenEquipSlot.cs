using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenEquipSlot : Slot<EquipInvenMenu>
{
    [SerializeField] Image _imgItemSprite = null;
    [SerializeField] Text _txtItemName = null;
    [SerializeField] Text _txtItemDesc = null;
    [SerializeField] Text _txtItemOptionDesc = null;

    public override void ClearSlot()
    {
        _id = -1;
        _isSet = false;
        _imgItemSprite.gameObject.SetActive(false);
        _txtItemName.text = "-";
        _txtItemOptionDesc.text = "-";
        _txtItemDesc.text = "-";
    }

    public override void OnClickSlot()
    {
        if(_id > 0)
            _parent.OnClickEquipButton(_id);
    }

    public override void SetSlot(int id, EquipInvenMenu parent)
    {
        _parent = parent;

        _id = id;
        _isSet = true;
        Item item = ItemDB.GetItem(_id);

        _imgItemSprite.gameObject.SetActive(true);
        _imgItemSprite.sprite = item.sprite;
        _txtItemName.text = item.name;
        _txtItemOptionDesc.text = item.optionDesc;
        _txtItemDesc.text = item.desc;
    }
}
