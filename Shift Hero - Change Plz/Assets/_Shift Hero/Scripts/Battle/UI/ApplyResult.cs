using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyResult : MonoBehaviour
{
    static readonly string ANIM_SHOW = "Show", ANIM_HIDE = "Hide";
    Animator _anim;

    [Header("Main")]
    [SerializeField] Text _txtTotalExp = null;
    [SerializeField] ApplyResultSlot[] _slots = null;

    [Header("Level Up UI")]
    [SerializeField] GameObject _goLevelUpUI = null;
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtLevelIncrease = null;
    [SerializeField] Text _txtLevelResult = null;
    [SerializeField] Text _txtStatusIncrease = null;
    [SerializeField] Text _txtStatusResult = null;

    StageResult _stageResult;
    Inventory _inven;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _stageResult = FindObjectOfType<StageResult>();
        _inven = FindObjectOfType<Inventory>();
    }


    public void SetSlots(Player[] players)
    {
        _inven.IncreaseGold(BattleData.GetBattleGold());

        int totalExp = BattleData.GetBattleExp();
        int[] giveExp = new int[players.Length];
        _txtTotalExp.text = string.Format("{0:###,0}", totalExp);

        // 0.4 : 0.35 : 0.25 의 비로 경험치 분배
        giveExp[0] = (int)(totalExp * 0.4f);
        giveExp[1] = (int)(totalExp * 0.35f);
        giveExp[2] = (int)(totalExp * 0.25f);
        giveExp[0] += totalExp - (giveExp[0] + giveExp[1] + giveExp[2]);

        // 슬롯 세팅
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetSlot(players[i], giveExp[i], this);
        }

        // 슬롯 보여주기 연출
        _anim.SetTrigger(ANIM_SHOW);
        
        // 경험치 분배 시작
        StartCoroutine(GiveExp());
    }
    
    // 경험치 분배
    IEnumerator GiveExp()
    {
        yield return new WaitForSeconds(1.5f);

        // 슬롯별 분배 시작
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].StartToGiveExp();

            yield return new WaitUntil(() => _slots[i].IsFinish());
            _goLevelUpUI.SetActive(false);
        }

        yield return new WaitForSeconds(3f);

        // 슬롯 감추기
        _anim.SetTrigger(ANIM_HIDE);
        _stageResult.SetActiveButton(true);

        // 기록하기
        MyInfo.GiveBattleExp(BattleData.GetBattleExp() / 3);
        BattleData.RecordBattle();
        BattleData.ClearBattleRecord();

    }


    public void CallLevelUp(Player player, int levelUp)
    {
        _goLevelUpUI.SetActive(true);

        _txtName.text = $"{player.GetName()} 레밸 업!";

        _txtLevelIncrease.text = $"레밸 +{levelUp}";
        _txtLevelResult.text = $"{player.GetLevel()}";

        _txtStatusIncrease.text = $"체력 +{player.GetLevelUpHp() * levelUp}\n";
        _txtStatusIncrease.text += $"공격력 +{player.GetLevelUpAtk() * levelUp}\n";
        _txtStatusIncrease.text += $"방어력 +{player.GetLevelUpDef() * levelUp}";

        _txtStatusResult.text = $"{player.GetMaxHp()}\n";
        _txtStatusResult.text += $"{player.GetAtk()}\n";
        _txtStatusResult.text += $"{player.GetDef()}";
    }
}
