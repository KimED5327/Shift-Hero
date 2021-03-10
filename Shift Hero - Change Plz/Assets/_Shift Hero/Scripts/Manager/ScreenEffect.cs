using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : Singleton<ScreenEffect>
{
    [SerializeField] Image _imgBackground = null;

    bool _isFinished = true;

    /// <summary>
    /// 연출 종료시 true 반환
    /// </summary>
    /// <returns></returns>
    public bool IsFinishEffect() { return _isFinished; }

    Color GetColor(Color p_color, float startAlpha)
    {
        Color color = p_color;
        color.a = startAlpha;
        _imgBackground.color = color;
        _imgBackground.gameObject.SetActive(true);

        return color;
    }

    /// <summary>
    /// 번쩍 연출
    /// </summary>
    /// <param name="speed"></param>
    public void Splash(float speed)
    {
        _isFinished = false;

        StopAllCoroutines();
        StartCoroutine(BlinkCoroutine(speed, GetColor(Color.white, 0f)));
    }

    /// <summary>
    /// 암전 연출
    /// </summary>
    /// <param name="speed"></param>
    public void FadeOutFadeIn(float speed)
    {
        _isFinished = false;

        StopAllCoroutines();
        StartCoroutine(BlinkCoroutine(speed, GetColor(Color.black, 0f)));
    }

    /// <summary>
    /// 페이드 아웃 연출
    /// </summary>
    /// <param name="speed"></param>
    public void FadeOut(float speed)
    {
        _isFinished = false;

        StopAllCoroutines();
        StartCoroutine(FadeOutCo(speed, GetColor(Color.black, 0f)));
    }

    /// <summary>
    /// 페이드 인 연출
    /// </summary>
    /// <param name="speed"></param>
    public void FadeIn(float speed)
    {
        _isFinished = false;

        StopAllCoroutines();
        StartCoroutine(FadeInCo(speed, GetColor(Color.black, 1f)));
    }

    IEnumerator BlinkCoroutine(float speed, Color color)
    {
        while (_imgBackground.color.a < 1.0f)
        {
            color.a += speed * Time.deltaTime;
            _imgBackground.color = color;
            yield return null;
        }
        while (_imgBackground.color.a > 0)
        {
            color.a -= speed * Time.deltaTime;
            _imgBackground.color = color;
            yield return null;
        }

        _imgBackground.gameObject.SetActive(false);
        _isFinished = true;
    }



    IEnumerator FadeOutCo(float speed, Color color)
    {
        while (_imgBackground.color.a < 1.0f)
        {
            color.a += speed * Time.deltaTime;
            _imgBackground.color = color;
            yield return null;
        }

        _isFinished = true;
    }


    IEnumerator FadeInCo(float speed, Color color)
    {
        while (_imgBackground.color.a > 0)
        {
            color.a -= speed * Time.deltaTime;
            _imgBackground.color = color;
            yield return null;
        }

        _imgBackground.gameObject.SetActive(false);
        _isFinished = true;
    }
}
