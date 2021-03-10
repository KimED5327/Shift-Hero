using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HpBarManager : MonoBehaviour
{
    [SerializeField] Transform m_tfPlayerBar = null;
    Transform m_tfPlayer = null;
    
    // Update is called once per frame
    void Update()
    {
        if (m_tfPlayer != null && m_tfPlayer.gameObject.activeSelf)
            m_tfPlayerBar.position = m_tfPlayer.position + Vector3.up * 2f;
        else
            m_tfPlayerBar.gameObject.SetActive(false);
    }

    public void SetCurPlayerLink(Transform p_tfCurrentPlayer)
    {
        m_tfPlayer = p_tfCurrentPlayer;
        m_tfPlayerBar.gameObject.SetActive(true);
        m_tfPlayerBar.GetComponent<PlayerHpBar>().SetCharacter(m_tfPlayer.GetComponent<Character>());
    }
}