using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    bool _isMuteSFX = false;
    bool _isMuteBGM = false;
    bool _isMuteVibrate = false;
    bool _isAutoSave = false;

    Color _originColor;

    [SerializeField] Image _imgSFX = null;
    [SerializeField] Image _imgBGM = null;
    [SerializeField] Image _imgSAVE = null;
    [SerializeField] Image _imgVIBRATE = null;

    [SerializeField] Text _txtSFX = null;
    [SerializeField] Text _txtBGM = null;
    [SerializeField] Text _txtSAVE = null;
    [SerializeField] Text _txtVibrate = null;


    private void Awake()
    {
        _originColor = _imgSFX.color;
    }

    public void OnClickSFXBtn()
    {
        _isMuteSFX = !_isMuteSFX;
        SoundManager.instance.SetMuteSFXPlayer(_isMuteSFX);
        _imgSFX.color = (_isMuteSFX) ? Color.gray : _originColor;
        _txtSFX.color = (_isMuteSFX) ? Color.gray : _originColor;
        _txtSFX.text = (_isMuteSFX) ? "효과음 OFF" : "효과음 ON";
    }

    public void OnClickBGMBtn()
    {
        _isMuteBGM = !_isMuteBGM;
        SoundManager.instance.SetMuteBGMPlayer(_isMuteBGM);

        _imgBGM.color = (_isMuteBGM) ? Color.gray : _originColor;
        _txtBGM.color = (_isMuteBGM) ? Color.gray : _originColor;
        _txtBGM.text = (_isMuteBGM) ? "배경음 OFF" : "배경음 ON";
    }

    public void OnClickVibrateBtn()
    {
        _isMuteVibrate = !_isMuteVibrate;
        _imgVIBRATE.color = (_isMuteVibrate) ? Color.gray : _originColor;
        _txtVibrate.color = (_isMuteVibrate) ? Color.gray : _originColor;
        _txtVibrate.text = (_isMuteVibrate) ? "진동 OFF" : "진동 ON";
    }

    public void OnClickAutoSaveBtn()
    {
        _isAutoSave = !_isAutoSave;
        _imgSAVE.color = (_isAutoSave) ? Color.gray : _originColor;
        _txtSAVE.color = (_isAutoSave) ? Color.gray : _originColor;
        _txtSAVE.text = (_isAutoSave) ? "서버 저장 OFF" : "서버 저장 ON";
    }

    public void OnClickYoutubeBtn()
    {
        Application.OpenURL("https://youtube.com/케이디");
    }

    public void OnClickTistoryBtn()
    {
        Application.OpenURL("https://Keidy.Tistory.com");
    }
}
