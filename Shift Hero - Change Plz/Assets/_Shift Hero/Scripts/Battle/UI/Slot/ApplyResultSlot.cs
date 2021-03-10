using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyResultSlot : Slot<ApplyResult>
{
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtExp = null;
    [SerializeField] Text _txtLevel = null;

    [SerializeField] Image _imgExpGuage = null;
    [SerializeField] Image _imgCharacter = null;

    int _exp;
    int _originLv;
    bool _isFinish;

    Player _player;

    WaitForSeconds _waitTime;

    public override void ClearSlot()
    {
        _isSet = false;
        ;
    }

    public override void OnClickSlot()
    {
        ;
    }


    /// <summary>
    /// 캐릭터 레밸업시, 부모에게 전달
    /// </summary>
    /// <param name="value"></param>
    public void LevelUp(int value)
    {
        _parent.CallLevelUp(_player, value);
    }

    /// <summary>
    /// 경험치 전달 전, 슬롯 세팅
    /// </summary>
    /// <param name="player"></param>
    /// <param name="exp"></param>
    /// <param name="parent"></param>
    public void SetSlot(Player player, int exp, ApplyResult parent)
    {
        _parent = parent;
        _player = player;
        _exp = exp;

        _originLv = _player.GetLevel();
        _txtLevel.text = $"LEVEL {_originLv}";
        _txtName.text = _player.GetName();
        _txtExp.text = string.Format("{0:###,0}", _exp);
        _imgCharacter.sprite = PlayerDB.GetPlayerData(_player.GetID()).sprite;
        _imgExpGuage.fillAmount = (float)_player.GetCurrentExp() / PlayerDB.GetLevelUpNeedExp(_player.GetLevel());

        _isSet = true;
        _waitTime = new WaitForSeconds(0.02f);
        _isFinish = false;
    }

    /// <summary>
    /// 경험치 전달 실행
    /// </summary>
    public void StartToGiveExp()
    {
        StartCoroutine(IncreaseExpEffect());
    }


    // 경험치 전달 코루틴
    IEnumerator IncreaseExpEffect()
    {
        yield return new WaitForSeconds(0.75f);

        int leftExp = _exp;

        // 경험치가 전부 전달될 때까지 반복
        while(leftExp > 0)
        {
            // 경험치 분할 전달
            int giveExp = GetNumExp(leftExp);
            leftExp -= giveExp;
            _player.IncreaseExp(giveExp);

            // 게이지 반영
            _txtExp.text = string.Format("{0:###,0}", leftExp);
            _imgExpGuage.fillAmount = (float)_player.GetCurrentExp() / PlayerDB.GetLevelUpNeedExp(_player.GetLevel());
            _txtLevel.text = $"LEVEL {_player.GetLevel()}";

            yield return _waitTime;
        }

        yield return new WaitForSeconds(0.5f);

        // 레밸업 시 팝업 호출
        int levelDiff = _player.GetLevel() - _originLv;
        if (levelDiff != 0)
        {
            LevelUp(levelDiff);
            yield return new WaitForSeconds(2.5f);
        }

        _txtLevel.text = $"LEVEL {_originLv + levelDiff}";
        _isFinish = true;
    }

    // 경험치 쪼개기
    int GetNumExp(int leftExp)
    {
        int giveExp;

        if (_exp < 10)
            giveExp = 1;

        else if (_exp < 100)
            giveExp = (leftExp > 1) ? 1 : leftExp;
        else if (_exp < 500)
            giveExp = (leftExp > 5) ? 5 : leftExp;
        else if(_exp < 5000)
            giveExp = (leftExp > 50) ? 50 : leftExp;
        else
            giveExp = (leftExp > 100) ? 100 : leftExp;

        return giveExp;
    }

    /// <summary>
    /// 경험치 전달이 끝났는지 여부
    /// </summary>
    /// <returns></returns>
    public bool IsFinish() { return _isFinish; }

    public override void SetSlot(int id, ApplyResult parent)
    {
        ;
    }
}
