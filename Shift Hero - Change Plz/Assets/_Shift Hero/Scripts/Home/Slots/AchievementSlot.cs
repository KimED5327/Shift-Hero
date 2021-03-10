using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSlot : Slot<ArchiveMenu>
{
    static readonly float ALPHA_LOW = 0.25f, ALPHA_HIGH = 1f;

    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtDetail = null;
    [SerializeField] Image _imgIcon = null;

    [SerializeField] Image _imgFilled = null;   // 슬라이드
    [SerializeField] Image _imgButton = null;   // 버튼 컬러
    [SerializeField] Text _txtButton = null;    // 버튼 컬러

    int _targetAmount;                          // 목표량
    int _curAmount;                             // 현재량
    float _achievementRate = 0f;                // 달성도
    bool _isAchieve;                            // 달성 여부

    public override void ClearSlot()
    {
        ;
    }

    public override void OnClickSlot()
    {
        // 달성 보상
        if(_isAchieve)
        {
            _parent.OnCallAchieveRewardBtn(_id);
        }
        else
        {
            // 달성 실패 메세지
            if(_targetAmount > 0)
                Message.instance.ShowMsg(StringData.msgNotEnoughAchieve);
            // 모든 업적 달성 완료 메세지
            else
                Message.instance.ShowMsg(StringData.msgFinishAchieve);
        }
    }

    public override void SetSlot(int id, ArchiveMenu parent)
    {
        _id = id;
        _parent = parent;


        Achieve achieve = AchieveDB.GetAchieve(_id);
        AchieveDetail detail = AchieveDB.GetAchieveDetail(_id, achieve.clearCount);

        // 해당 업적을 전부 달성하지 않은 경우 새로 세팅
        if(detail != null)
        {
            _targetAmount = detail.count;
            _curAmount = AchieveParser.GetAchieveCount(id);
            _achievementRate = (float)_curAmount / _targetAmount;

            _imgIcon.sprite = achieve.sprite;
            _txtName.text = $"{achieve.title} ({achieve.clearCount + 1})";
            _txtDetail.text = achieve.desc.Replace("count", detail.count.ToString());
            _txtDetail.text += $" ({_curAmount} / {_targetAmount})";
            _imgFilled.fillAmount = _achievementRate;

            _isAchieve = _achievementRate >= 1.0f;
        }
        // 전부 달성한 경우 정보 최소화
        else
        {
            _achievementRate = 0f;
            _curAmount = 0;
            _targetAmount = 0;
            _imgIcon.sprite = achieve.sprite;
            _txtName.text = $"{achieve.title} (클리어)";
            _txtDetail.text = "-";
            _imgFilled.fillAmount = 1f;
            _isAchieve = false;
        }

        // 색깔 조정
        Color btnColor = _imgButton.color;
        Color txtColor = _txtButton.color;
        btnColor.a = _isAchieve ? ALPHA_HIGH : ALPHA_LOW;
        txtColor.a = _isAchieve ? ALPHA_HIGH : ALPHA_LOW;
        _imgButton.color = btnColor;
        _txtButton.color = txtColor;
    }
}
