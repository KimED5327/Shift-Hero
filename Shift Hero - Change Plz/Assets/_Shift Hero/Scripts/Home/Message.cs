using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [Header("Normal Msg")]
    [SerializeField] GameObject _goMessage = null;      // 메시지 창
    [SerializeField] Text _txtMessage = null;           // 메세지 텍스트

    [Header("Good Msg")]
    [SerializeField] GameObject _goGoodMessage = null;      // 메시지 창
    [SerializeField] Text _txtGoodMessage = null;           // 메세지 텍스트

    public static Message instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// msg 내용을 출력함
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMsg(string msg)
    {
        _txtMessage.text = msg;
        _goMessage.SetActive(true);
    }

    /// <summary>
    /// UI 숨김처리
    /// </summary>
    public void HideMsg()
    {
        _goMessage.SetActive(false);
    }

    /// <summary>
    /// 축하성 메세지 출력
    /// </summary>
    /// <param name="msg"></param>
    public void ShowGoodmsg(string msg)
    {
        _txtGoodMessage.text = msg;
        _goGoodMessage.SetActive(true);
    }

    /// <summary>
    /// 축하성 메세지 닫기
    /// </summary>
    public void HideGoodMsg()
    {
        _goGoodMessage.SetActive(false);
    }
}
