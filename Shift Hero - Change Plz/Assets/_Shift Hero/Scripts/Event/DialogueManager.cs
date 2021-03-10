using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _goUI = null;       // UI
    [SerializeField] Text _txtName = null;          // 이름
    [SerializeField] Text _txtLine = null;          // 대사
    [SerializeField] Image _imgFace = null;         // 얼굴 이미지
    [SerializeField] GameObject _goMarker = null;   // Next 마커

    [Header("Detail")]
    [SerializeField] float _typingSpeed = 0.1f;     // 문자 타이핑 속도
    [SerializeField] float _delayComma = 0.25f;     // 콤마 딜레이
    [SerializeField] float _delayDot = 0.5f;        // 마침표 딜레이

    DialogueEvent _curDialogueEvent;                // 현재 대화 이벤트
    Dialogue _curDialogue;                          // 현재 대화 정보
    string _curLine;                                // 현재 대사
    string _curName;                                // 현재 발언자 이름

    int _lineStartID;                               // 시작 대사 ID값 
    int _lineEndID;                                 // 종료 대사 ID값
    int _curLineID;                                 // 현재 대사 ID값
    int _curLineLength;                             // 현재 대사의 길이
    int _curLineIndex;                              // 현재 대사의 출력중인 인덱스

    bool _canNext;                                  // 출력 완료 후 Next 처리 가능
    bool _isStart;                                  // 대사 시작 여부


    void Update()
    {
        TryNextProcess();
    }

    // 대사 출력 후 Next 처리
    void TryNextProcess()
    {
        if (_isStart && _canNext)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_curLineID <= _lineEndID)    // 아직 대사가 남아있다면 타이핑 시작
                    StartTyping();
                else                            // 대사가 끝났다면 대화 종료
                    FinishDialogue();
            }
        }
    }

    // 대화 시작
    public void ShowDialogue(DialogueEvent dialogueEvent)
    {
        if (_isStart) return;

        _isStart = true;
        _curDialogueEvent = dialogueEvent;
        _lineStartID = _curDialogueEvent.lineStartID;
        _curLineID = _lineStartID;
        _lineEndID = _curDialogueEvent.lineEndID;
        _curLineIndex = 0;


        _goUI.SetActive(true);
        StartTyping();
    }

    // 대사 출력 시작
    void StartTyping()
    {
        _canNext = false;
        _goMarker.SetActive(false);
        _curDialogue = DialogueDB.GetDialogue(_curLineID);  // 대화 정보 캐싱

        SetNameAndSprite();     // 이름과 스프라이트 정보 세팅
        SetCurrentLine();       // 대사 세팅
        TypingLine();           // 타이핑 시작
    }

    // 이름, 스프라이트 세팅
    void SetNameAndSprite()
    {
        Sprite sprite;

        // 발언자가 플레이어 캐릭터인 경우
        if (string.Equals(_curDialogue.tag, StringData.tagPlayer))
        {
            PlayerData data = PlayerDB.GetPlayerData(_curDialogue.characterID);
            _curName = data.name;
            sprite = data.sprite;
            _imgFace.gameObject.SetActive(true);
        }
        // 몬스터인 경우
        else if (string.Equals(_curDialogue.tag, StringData.tagMonster))
        {
            MonsterData data = MonsterDB.GetMonsterData(_curDialogue.characterID);
            _curName = data.name;
            sprite = data.sprite;
            _imgFace.gameObject.SetActive(true);
        }
        // 시스템
        else
        {
            _curName = "System";
            sprite = null;
            _imgFace.gameObject.SetActive(false);
        }

        _txtName.text = _curName;
        _imgFace.sprite = sprite;
    }

    // 대사 세팅 및 Replace
    void SetCurrentLine()
    {
        _txtLine.text = "";
        _curLine = _curDialogue.line;
        _curLine = _curLine.Replace("{Nickname}", PlayerPrefs.GetString(StringData.prefNickName));
        _curLine = _curLine.Replace("'", ",");
        _curLine = _curLine.Replace("#", "\n");
        _curLine = _curLine.Replace("{Name}", _curName);

        _curLineIndex = 0;
        _curLineLength = _curLine.Length;
    }

    // 대사 타이핑 연출
    void TypingLine()
    {
        _txtLine.text = _curLine.Substring(0, _curLineIndex);
        float delay = GetDelay();
        _curLineIndex++;

        // 대사 출력할 게 남아있다면 계속 출력
        if (_curLineIndex <= _curLineLength)
            Invoke(nameof(TypingLine), delay);
        else
            TypingFinished();
    }

    // 다음 출력 딜레이 시간 가져오기
    float GetDelay()
    {
        float delay = _typingSpeed;

        // 콤마 혹은 마침표가 오면 딜레이 시간 증가
        if (_curLineIndex > 0)
        {
            if (_curLine[_curLineIndex - 1].Equals(','))
                delay += _delayComma;
            else if (_curLine[_curLineIndex - 1].Equals('.'))
                delay += _delayDot;
            else if (_curLine[_curLineIndex - 1].Equals('!'))
                delay += _delayDot;
            else if (_curLine[_curLineIndex - 1].Equals('?'))
                delay += _delayDot;
        }

        return delay;
    }

    // 타이핑 종료 처리
    void TypingFinished()
    {
        _goMarker.SetActive(true);
        _canNext = true;
        _curLineID++;
    }

    // 대화 종료 처리
    void FinishDialogue()
    {
        _goUI.SetActive(false);
        _isStart = false;
    }



    // getter
    public bool IsStart() { return _isStart; }
}

