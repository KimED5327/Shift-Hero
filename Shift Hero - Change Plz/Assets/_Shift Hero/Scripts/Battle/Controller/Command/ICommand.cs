using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandResult
{
    Success,
    Failure,
}

public abstract class ICommand
{
    public MonoBehaviour _mono;

    public abstract void ChangeActor(Player player);
    public abstract CommandResult Execute();
}
