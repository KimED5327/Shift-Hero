using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopMenu : MonoBehaviour
{
    static readonly int WEAPON = 0;


    [SerializeField] ShopSlot[] _shopSlots = null;      // 상점 슬롯
    [SerializeField] ShopSlot _specialSlot = null;      // 스페셜 슬롯

    int _curTabNumber = -1;                             // 현재 활성화된 탭 번호
    Item _clickItem;                                    // 클릭한 슬롯의 아이템 번호

    [SerializeField] GameObject _goBuy = null;          // 구입 UI
    [SerializeField] Image _imgItem = null;             // 구입 아이템 이미지
    [SerializeField] Text _txtName = null;              // 구입 아이템 이름
    [SerializeField] Text _txtDesc = null;              // 구입 아이템 설명
    [SerializeField] Text _txtOption = null;            // 구입 아이템 옵션
    [SerializeField] Text _txtPrice = null;             // 구입 아이템 가격


    Inventory _inven;

    public void Start()
    {
        _inven = FindObjectOfType<Inventory>();

        OnClickTab(WEAPON);

        // 스페셜 슬롯 임시 세팅.
        _specialSlot.SetSlot(100001, this);
    }

    /// <summary>
    /// 탭 클릭
    /// </summary>
    /// <param name="value"></param>
    public void OnClickTab(int value)
    {
        if (_curTabNumber == value) return;
        
        _curTabNumber = value;

        List<int> shopItemList = ShopDB.GetItemListFromItemType(_curTabNumber);

        for (int i = 0; i < _shopSlots.Length; i++)
        {
            if(i < shopItemList.Count)
            {
                _shopSlots[i].gameObject.SetActive(true);
                _shopSlots[i].SetSlot(shopItemList[i], this);
            }
            else
            {
                _shopSlots[i].gameObject.SetActive(false);
                _shopSlots[i].ClearSlot();
            }
        }
    }

    /// <summary>
    /// 슬롯에서 호출
    /// </summary>
    /// <param name="itemID"></param>
    public void SlotCall(int itemID)
    {
        _clickItem = ItemDB.GetItem(itemID);

        _imgItem.sprite = _clickItem.sprite;
        _txtName.text = _clickItem.name;
        _txtDesc.text = _clickItem.desc;
        _txtOption.text = _clickItem.optionDesc;
        _txtPrice.text = _clickItem.price.ToString();

        _goBuy.SetActive(true);
    }

    /// <summary>
    /// 구매 버튼
    /// </summary>
    public void OnClickBuy()
    {
        // 골드가 충분한 경우
        if (_inven.IsEnoughGold(_clickItem.price))
        {
            _inven.DecreaseGold(_clickItem.price);  // 골드 감소
            _inven.AcquireItem(_clickItem.id);      //  구매한 아이템, 인벤토리에 푸시
            MyInfo.CallMyInfoChange();              //  골드 변경 사항. HUD에 반영
        }
        // 골드 부족
        else
        {
            Message.instance.ShowMsg(StringData.msgNotEnoughGold);
            _inven.IncreaseGold(50000);
        }

        _goBuy.SetActive(false);
    }

    /// <summary>
    /// 구매 취소 버튼
    /// </summary>
    public void OnClickCancel()
    {
        _goBuy.SetActive(false);
    }
}
