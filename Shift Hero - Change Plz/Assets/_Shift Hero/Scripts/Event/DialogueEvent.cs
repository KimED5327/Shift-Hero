[System.Serializable]
public class DialogueEvent
{
    public int id;
    public int lineStartID;
    public int lineEndID;
    public bool isShow;
    public Condition condition;
}

[System.Serializable]
public class Condition
{
    public int type;
    public int option;
}