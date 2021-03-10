using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : Slot<CharacterMenu>
{
    [SerializeField] Image _imgCharacterSprite = null;

    public override void ClearSlot()
    {
        _id = -1;
        _imgCharacterSprite.gameObject.SetActive(false);
    }

    public override void OnClickSlot()
    {
        ;
    }

    public override void SetSlot(int id, CharacterMenu parent)
    {
        _id = id;
        _imgCharacterSprite.sprite = SpriteDB.GetPlayerSprite(_id);
        _imgCharacterSprite.gameObject.SetActive(true);
    }
    public Sprite GetSprite() { return _imgCharacterSprite.sprite; }
}
