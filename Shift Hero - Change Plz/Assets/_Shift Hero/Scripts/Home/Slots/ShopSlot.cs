using UnityEngine.UI;
using UnityEngine;

public class ShopSlot : Slot<ShopMenu>
{
    [SerializeField] Image _imgItem = null;
    [SerializeField] Text _txtItemName = null;
    [SerializeField] Text _txtItemOption = null;
    [SerializeField] Text _txtItemPrice = null;

    public override void ClearSlot()
    {
        _imgItem.sprite = null;
        _isSet = false;
        _txtItemName.text = "";
        _txtItemOption.text = "";
        _txtItemPrice.text = "";

    }

    public override void OnClickSlot()
    {
        _parent.SlotCall(_id);
    }

    public override void SetSlot(int id, ShopMenu parent)
    {
        _parent = parent;
        _isSet = true;
        _id = id;
        Item item = ItemDB.GetItem(_id);

        _imgItem.sprite = item.sprite;
        _txtItemName.text = item.name;
        _txtItemOption.text = item.optionDesc;
        _txtItemPrice.text = string.Format("{0:###,#}", item.price);
    }
}
