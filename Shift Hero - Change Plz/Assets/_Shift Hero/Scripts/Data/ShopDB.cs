using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopDB : MonoBehaviour
{
    static List<int>[] _shopItems;


    public static void SetShopItem(List<int>[] item)
    {
        _shopItems = item;
    }

    public static List<int> GetItemListFromItemType(int itemType)
    {
        List<int> itemList = new List<int>();

        for(int i = 0; i < _shopItems[itemType].Count; i++)
        {
            itemList.Add(_shopItems[itemType][i]);
        }

        return itemList;
    }

}
