using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   



    Animator[] _anim;
    string _animTrigger = "Show";

    int _mainMenuChoice = 2;

    private void Awake()
    {
        _anim = GetComponentsInChildren<Animator>();
    }


    /// <summary>
    /// 메인 메뉴 슬라이딩 애니메이션 교체 (0 : 리워드, 1 : 캐릭터, 2 : 월드, 3 : 상점, 4 : 옵션)
    /// </summary>
    /// <param name="value"></param>
    public void OnClickMainMenu(int value)
    {
        if(_mainMenuChoice != value)
        {
            _mainMenuChoice = value;
            for(int i = 0; i < _anim.Length; i++)
                _anim[i].SetTrigger(_animTrigger + value);
        }
    }
}
