using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSlot : Slot<ArchiveMenu>
{
    public enum BookType
    {
        MONSTER,
        ITEM,
        CHARACTER,
    }

    BookType _bookType = 0; 
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtDesc = null;
    [SerializeField] Image _imgSprite = null;
    [SerializeField] GameObject _goChecker = null;

    public override void ClearSlot()
    {
        _id = -1;
        _isSet = false;
        _txtName.text = "???";
        _txtDesc.text = "???";
        _imgSprite.sprite = null;
        _imgSprite.gameObject.SetActive(false);
        _goChecker.SetActive(false);
    }

    public override void OnClickSlot()
    {
        ;
    }

    public void SetType(BookType type, int id, ArchiveMenu parent)
    {
        _bookType = type;

        _id = id;
        _parent = parent;

        switch (_bookType)
        {
            case BookType.MONSTER:
                MonsterData monster = AchieveDB.GetMonsterGetList(_id);
                _txtName.text = monster.name;
                _txtDesc.text = monster.desc;
                _imgSprite.sprite = monster.sprite;
                _imgSprite.gameObject.SetActive(true);
                break;
            case BookType.ITEM:
                Item item = AchieveDB.GetItemGetList(_id);
                _txtName.text = item.name;
                _txtDesc.text = item.desc;
                _imgSprite.sprite = item.sprite;
                _imgSprite.gameObject.SetActive(true);
                break;
            case BookType.CHARACTER:
                PlayerData player = PlayerDB.GetPlayerData(_id);
                _txtName.text = player.name;
                _txtDesc.text = player.desc;
                _imgSprite.sprite = player.sprite;
                _imgSprite.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        _goChecker.SetActive(true);
    }

    public override void SetSlot(int id, ArchiveMenu parent)
    {
        ;
    }
}
