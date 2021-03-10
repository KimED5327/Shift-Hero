using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum FloatingTextType
{
    RED,
    WHITE,
    GRAY,
}

public class FloatingText : MonoBehaviour
{
    TextMeshPro m_tmpFloating;
    float _originSize;
    float _criSize;

    // Start is called before the first frame update
    void Awake()
    {
        m_tmpFloating = GetComponentInChildren<TextMeshPro>();
        _originSize = m_tmpFloating.fontSize;
        _criSize = _originSize * 1.5f;
    }



    public void Setup(Vector3 pos, int p_damage, FloatingTextType p_type, bool isCritical)
    {
        transform.position = pos;
        m_tmpFloating.SetText(p_damage.ToString());
        m_tmpFloating.fontSize = isCritical ? _criSize : _originSize;

        switch (p_type)
        {
            case FloatingTextType.RED:
                m_tmpFloating.color = Color.red;
                break;
            case FloatingTextType.WHITE:
                m_tmpFloating.color = Color.white;
                break;
        }

        gameObject.SetActive(true);
    }

    public void MissSetup(Vector3 pos)
    {
        m_tmpFloating.fontSize = _originSize;
        transform.position = pos;
        m_tmpFloating.SetText(StringData.Miss);
        m_tmpFloating.color = Color.gray;
        gameObject.SetActive(true);
    }
}
