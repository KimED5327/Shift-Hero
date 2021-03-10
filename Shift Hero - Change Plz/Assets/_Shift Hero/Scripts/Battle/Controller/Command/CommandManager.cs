using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerCommand
{
    Move,
    Attack,
    Change,
    Dodge,
}

public class CommandManager
{
    
    Dictionary<PlayerCommand, ICommand> _commandDic = new Dictionary<PlayerCommand, ICommand>();

    // 캐릭터 교체 이벤트 
    public delegate void ChangeCall(Player player);
    public static event ChangeCall OnChangePlayer;

    public CommandManager()
    {
        OnChangePlayer = null;
    }

    // 캐릭터 교체 이벤트 발생
    public static void CallChangeActorEvent(Player player)
    {
        OnChangePlayer.Invoke(player);
    }

    public void AddCommand(PlayerCommand cmdName, ICommand cmd)
    {
        if (_commandDic.ContainsKey(cmdName))
            Debug.LogError("이미 추가한 커맨드");
        else
           _commandDic.Add(cmdName, cmd);
    }

    public void ChangeActor(Player player)
    {
        foreach(KeyValuePair<PlayerCommand, ICommand> pair in _commandDic)
        {
            _commandDic[pair.Key].ChangeActor(player);
        }
    }


    public CommandResult InvokeExecute(PlayerCommand cmdName)
    {
        return _commandDic[cmdName].Execute();
    }
}
