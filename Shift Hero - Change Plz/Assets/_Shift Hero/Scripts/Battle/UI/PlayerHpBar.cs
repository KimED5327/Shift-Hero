using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] Image m_imgHpbar = null;
    [SerializeField] Text[] m_txtHp = null;

    Character m_character; public void SetCharacter(Character p_character) { m_character = p_character; }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_character != null)
        {
            int t_currentHp = m_character.GetCurrentHP();
            m_imgHpbar.fillAmount = (float)t_currentHp / m_character.GetMaxHp();
            m_txtHp[0].text = t_currentHp.ToString();
            m_txtHp[1].text = t_currentHp.ToString();
        }
    }
}
