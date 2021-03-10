using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    string homeScene = "Home";

    [SerializeField] GameObject _goLoginUI = null;
    [SerializeField] GameObject _touchUI = null;

    [SerializeField] float _fadeOutSpeed = 0.5f;

    bool _isTouch = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isTouch)
        {
            _isTouch = true;
            _touchUI.SetActive(false);

            // 닉네임 기록 있으면 바로 로딩
            if (PlayerPrefs.HasKey(StringData.prefNickName))
            {
                StartCoroutine(Loading());
            }

            // 없으면 닉네임 입력
            else
            {
                StartCoroutine(ShowLogin());
            }

        }
    }

    IEnumerator ShowLogin()
    {
        ScreenEffect.Instance.FadeOut(_fadeOutSpeed);

        yield return new WaitUntil(() => ScreenEffect.Instance.IsFinishEffect());
        _goLoginUI.SetActive(true);

        ScreenEffect.Instance.FadeIn(_fadeOutSpeed);
    }


    IEnumerator Loading()
    {
        ScreenEffect.Instance.FadeOut(_fadeOutSpeed);

        yield return new WaitUntil(() => ScreenEffect.Instance.IsFinishEffect());

        ScreenEffect.Instance.FadeIn(_fadeOutSpeed);

        SceneManager.LoadScene(homeScene);
    }
}
