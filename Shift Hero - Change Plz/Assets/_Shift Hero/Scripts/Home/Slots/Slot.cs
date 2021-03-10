using UnityEngine;

public abstract class Slot<T> : MonoBehaviour
{
    protected int _id = -1;
    protected bool _isSet = false;

    protected T _parent;

    public abstract void SetSlot(int id, T parent);
    public abstract void OnClickSlot();
    public abstract void ClearSlot();

    public int GetID() { return _id; }
    public bool IsSet() { return _isSet; }


}
