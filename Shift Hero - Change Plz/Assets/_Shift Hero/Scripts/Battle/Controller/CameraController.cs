using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform m_tfPlayer;
    [SerializeField] BoxCollider2D m_col = null;

    float m_widthHalf;
    float m_heightHalf;
    Vector3 m_minBound;
    Vector3 m_maxBound;
    Vector3 m_camOriginDistance;
    [SerializeField] float m_followSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        m_heightHalf = GetComponent<Camera>().orthographicSize;
        m_widthHalf = m_heightHalf * Screen.width / Screen.height;
  
        m_minBound = m_col.bounds.min;
        m_maxBound = m_col.bounds.max;

        m_camOriginDistance = new Vector3(0, 0, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(m_tfPlayer != null && m_tfPlayer.gameObject.activeSelf)
        {
            transform.position = Vector3.Lerp(transform.position, m_tfPlayer.position + m_camOriginDistance, m_followSpeed);
            float t_clampedX = Mathf.Clamp(transform.position.x, m_minBound.x + m_widthHalf, m_maxBound.x - m_widthHalf);
            transform.position = new Vector3(t_clampedX, m_camOriginDistance.y, m_camOriginDistance.z);
        }
    }

    public void SetTargetLink(Transform p_tfPlayer) { m_tfPlayer = p_tfPlayer; }
}
