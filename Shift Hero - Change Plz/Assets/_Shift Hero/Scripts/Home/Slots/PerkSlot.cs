using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkSlot : Slot<PerkMenu>
{
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtDesc = null;
    [SerializeField] Text _txtSP = null;

    [SerializeField] Image _imgSprite = null;

    [SerializeField] GameObject _goScreen = null;
    [SerializeField] GameObject _goLearning = null;

    public override void ClearSlot()
    {

    }

    /// <summary>
    /// 슬롯 클릭시 부모 메뉴로 해당 슬롯이 가지고 있는 퍽의 ID 값 전달
    /// </summary>
    public override void OnClickSlot()
    {
        _parent.OnCallSlot(_id);
    }

    public void SetPerkSlot(PerkInfo info, PerkMenu parent)
    {
        _id = info.id;
        _parent = parent;

        // 기본 정보 세팅
        _txtName.text = info.name;
        _txtDesc.text = info.desc;
        _imgSprite.sprite = info.sprite;
        _txtSP.text = info.sp.ToString();

        // 해당 퍽을 학습했다면 학습 표시
        if (info.isLearn)
        {
            _goLearning.SetActive(true);
        }

        // 선행 조건이 있다면
        if(info.priorID != -1)
        {
            // 선행 조건을 달성했으면 스크린 해제
            if (_parent.GetPerkInfo(info.priorID).isLearn)
            {
                _goScreen.SetActive(false);
            }
            // 선행 조건 미달성시 스크린 활성화
            else
            {
                _goScreen.SetActive(true);
            }
        }

        _isSet = true;
    }




    public override void SetSlot(int id, PerkMenu parent)
    {
        ;
    }
}
