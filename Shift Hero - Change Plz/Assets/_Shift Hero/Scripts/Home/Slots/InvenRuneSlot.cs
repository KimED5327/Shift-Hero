using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenRuneSlot : Slot<RuneInvenMenu>
{
    [SerializeField] Image _imgItemSprite = null;
    [SerializeField] Text _txtItemName = null;
    [SerializeField] Text _txtItemDesc = null;
    [SerializeField] Text _txtItemOptionDesc = null;
    [SerializeField] GameObject _goButton = null;

    public override void ClearSlot()
    {
        _imgItemSprite.gameObject.SetActive(false);
        _txtItemName.text = "-";
        _txtItemOptionDesc.text = "-";
        _txtItemDesc.text = "-";
    }

    public override void OnClickSlot()
    {
        _parent.ShowWhichRuneChoiceWindow(_id);
    }

    public override void SetSlot(int id, RuneInvenMenu parent)
    {
        _parent = parent;

        _id = id;
        Item item = ItemDB.GetItem(id);

        _imgItemSprite.gameObject.SetActive(true);
        _imgItemSprite.sprite = item.sprite;
        _txtItemName.text = item.name;
        _txtItemOptionDesc.text = item.optionDesc;
        _txtItemDesc.text = item.desc;
    }

    public void ShowButton()
    {
        _goButton.SetActive(true);
    }

    public void HideButton()
    {
        _goButton.SetActive(false);
    }
}
