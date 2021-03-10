using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventCall : MonoBehaviour
{
    DialogueManager _theDM;

    private void Awake()
    {
        _theDM = FindObjectOfType<DialogueManager>();
    }

    
    
    void Start()
    {
        Invoke(nameof(EventCall), 0.5f);
    }

    void EventCall()
    {
        DialogueEvent dialogueEvent = DialogueDB.GetMatchedGameStartEvent();

        if (dialogueEvent != null)
            _theDM.ShowDialogue(dialogueEvent);
    }
}
