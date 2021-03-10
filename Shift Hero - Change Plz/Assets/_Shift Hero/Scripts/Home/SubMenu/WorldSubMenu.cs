using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSubMenu : MonoBehaviour
{
    PerkMenu _perkMenu;
    ArchiveMenu _archiveMenu;

    private void Awake()
    {
        _perkMenu = FindObjectOfType<PerkMenu>();
        _archiveMenu = FindObjectOfType<ArchiveMenu>();
    }

    /// <summary>
    /// 업적 기록 메뉴 열기
    /// </summary>
    public void OnClickArchiveMenuBtn()
    {
        _archiveMenu.ShowMenu();
    }

    /// <summary>
    /// 능력 메뉴 열기
    /// </summary>
    public void OnClickPerkMenuBtn()
    {
        _perkMenu.ShowMenu();
    }
}
