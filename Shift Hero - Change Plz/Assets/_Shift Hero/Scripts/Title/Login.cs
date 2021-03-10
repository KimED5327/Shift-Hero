using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Collections;

public class Login : MonoBehaviour
{
    [SerializeField] InputField _InputNickname = null;

    string homeScene = "Home";

    public void OnClickAccess()
    {
        string nickname = _InputNickname.text;

        if (nickname == "")
        {
            Message.instance.ShowMsg("닉네임을 입력해주세요.");
            return;
        }
        else if (nickname.Length > 6)
        {

            Message.instance.ShowMsg("닉네임은 6글자 이내로 해주세요.");
            return;
        }
        else if (nickname.Length < 2)
        {
            Message.instance.ShowMsg("닉네임은 2글자 이상으로 해주세요.");
            return;
        }
        if (!IsValidStr(nickname)) // 닉네임에 특수문자, 초성, 띄어쓰기가 포함된 경우.
        {
            Message.instance.ShowMsg("초성과 띄어쓰기, 특수문자는 불가능합니다.");
            return;
        }

        StringData.myNickname = nickname;
        PlayerPrefs.SetString(StringData.prefNickName, nickname);

        StartCoroutine(Loading());
    }

    bool IsValidStr(string text)
    {
        string pattern = @"^[a-zA-Z0-9가-힣]*$";
        return Regex.IsMatch(text, pattern);
    }

    public void OnClickExit()
    {

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    
    IEnumerator Loading()
    {
        ScreenEffect.Instance.FadeOut(1f);

        yield return new WaitUntil(() => ScreenEffect.Instance.IsFinishEffect());

        ScreenEffect.Instance.FadeIn(1f);

        SceneManager.LoadScene(homeScene);
    }
}
