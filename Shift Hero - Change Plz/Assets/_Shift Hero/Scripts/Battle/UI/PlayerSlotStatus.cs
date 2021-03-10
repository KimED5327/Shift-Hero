using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotStatus : MonoBehaviour
{
    Player[] m_Players;

    [SerializeField] Image[] _imgPlayerFace = null;
    [SerializeField] Image[] m_imgHpBar = null;

    //[SerializeField] Image[] m_imgExpBar = null;

    [SerializeField] Transform m_tfChoiceSlot = null;
    [SerializeField] Transform[] m_tfSlotPos = null;

    int m_curPlayerNum = 0;

    float[] m_restHpRecoverTime;    // 캐릭별 체력 회복 시간
    float[] m_currentTime;          // 캐릭별 경과 시간 체크
    int[] m_restHpRecover;          // 캐릭별 회복량

    bool _isSetting = false;

    public void SetPlayerLink(Player[] player)
    {
        m_Players = player;
        m_restHpRecoverTime = new float[m_Players.Length];
        m_currentTime = new float[m_Players.Length];
        m_restHpRecover = new int[m_Players.Length];
        for (int i = 0; i < m_Players.Length; i++)
        {
            _imgPlayerFace[i].sprite = PlayerDB.GetPlayerData(m_Players[i].GetID()).sprite;
            m_imgHpBar[i].fillAmount = 1f;
            m_restHpRecoverTime[i] = m_Players[i].GetRecoverTime();
            m_restHpRecover[i] = m_Players[i].GetRecoverHp();
            m_currentTime[i] = 0f;
        }

        _isSetting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSetting)
        {
            CalcHpBar();
            RestRecover();
        }
    }

    // hp bar 길이 계산
    void CalcHpBar()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_imgHpBar[i].fillAmount = (float)m_Players[i].GetCurrentHP() / m_Players[i].GetMaxHp();
        }
    }

    // 휴식 중인 캐릭터만 회복
    void RestRecover()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            // 현재 조종중인 캐릭터는 제외
            if (m_curPlayerNum == i) continue;

            // 타이머 증가 후 체력 회복
            m_currentTime[i] += Time.deltaTime;
            if (m_currentTime[i] >= m_restHpRecoverTime[i])
            {
                m_currentTime[i] = 0;
                m_Players[i].RecoverHp(m_restHpRecover[i]);
            }
        }

    }

    public void SetChoiceCharacter(int p_num)
    {
        m_curPlayerNum = p_num;

        m_tfChoiceSlot.localPosition = m_tfSlotPos[m_curPlayerNum].localPosition;
    }

}
