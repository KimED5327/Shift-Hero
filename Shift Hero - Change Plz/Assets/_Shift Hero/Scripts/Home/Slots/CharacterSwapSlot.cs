using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwapSlot : Slot<CharacterShopMenu>
{
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtClass = null;
    [SerializeField] Text _txtDesc = null;
    [SerializeField] Text _txtPrice = null;
    [SerializeField] Image _imgCharacter = null;

    [SerializeField] GameObject[] _goBtns = null;
    [SerializeField] GameObject _goIsStandBy = null;


    bool isNeedBuy;

    public override void ClearSlot()
    {
        _txtName.text = "-";
        _txtClass.text = "-";
        _txtDesc.text = "-";
        _txtPrice.text = "- G";
        _imgCharacter.gameObject.SetActive(false);
        _id = -1;
        _isSet = false;
        _goBtns[0].gameObject.SetActive(false);
        _goBtns[1].gameObject.SetActive(false);
    }

    public override void OnClickSlot()
    {
        _parent.CallSlot(_id, isNeedBuy);
    }

    public override void SetSlot(int id, CharacterShopMenu parent)
    {
        _parent = parent;
        _id = id;

        PlayerData player = PlayerDB.GetPlayerData(_id);
        _txtName.text = player.name;
        _txtClass.text = player.job;
        _txtDesc.text = player.desc.Replace("#", "\n");
        _txtPrice.text = $"{string.Format("{0:###,0}", player.price)} G\n구입";
        _imgCharacter.gameObject.SetActive(true);
        _imgCharacter.sprite = player.sprite;

        isNeedBuy = !player.isOpen;

        _goIsStandBy.SetActive(false);

        _goBtns[0].gameObject.SetActive(!isNeedBuy);        // 교체 가능
        _goBtns[1].gameObject.SetActive(isNeedBuy);         // 구입 필요
        _isSet = true;
    }


    public void StandbyCharacter()
    {
        _goIsStandBy.SetActive(true);
    }
}
