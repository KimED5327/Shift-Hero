using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTBlackBoard
{
    public static readonly string Dir = "Dir";
    public static readonly string IsAttack = "IsAttack";
    public static readonly string ForceChase = "IsHurt";

    Dictionary<string, float> _floatDic;
    Dictionary<string, int> _intDic;
    Dictionary<string, bool> _boolDic;
    Dictionary<string, Transform> _tfDic;
    Dictionary<string, Vector2> _vector2Dic;


    public void SetValueVector2(string key, Vector2 value)
    {
        if (_vector2Dic == null)
            _vector2Dic = new Dictionary<string, Vector2>();

        if (_vector2Dic.ContainsKey(key))
        {
            _vector2Dic[key] = value;
        }
        else
        {
            _vector2Dic.Add(key, value);
        }
    }

    public Vector2 GetValueVector2(string key)
    {
        if (_vector2Dic.ContainsKey(key))
        {
            return _vector2Dic[key];
        }
        else
        {
            Debug.LogError("Not Contain a Vector2 Key");
            return Vector2.zero;
        }
    }


    public void SetValueTransform(string key, Transform value)
    {
        if (_tfDic == null)
            _tfDic = new Dictionary<string, Transform>();

        if (_tfDic.ContainsKey(key))
        {
            _tfDic[key] = value;
        }
        else
        {
            _tfDic.Add(key, value);
        }
    }

    public Transform GetValueTransform(string key)
    {
        if (_tfDic.ContainsKey(key))
        {
            return _tfDic[key];
        }
        else
        {
            Debug.LogError("Not Contain a Transform Key");
            return null;
        }
    }


    public void SetValueFloat(string key, float value)
    {
        if (_floatDic == null)
            _floatDic = new Dictionary<string, float>();

        if (_floatDic.ContainsKey(key))
        {
            _floatDic[key] = value;
        }
        else
        {
            _floatDic.Add(key, value);
        }
    }

    public float GetValueFloat(string key)
    {
        if (_floatDic.ContainsKey(key))
        {
            return _floatDic[key];
        }
        else
        {
            Debug.LogError("Not Contain a Float Key");
            return -1f;
        }
    }



    public void SetValueInt(string key, int value)
    {
        if (_intDic == null)
            _intDic = new Dictionary<string, int>();

        if (_intDic.ContainsKey(key))
        {
            _intDic[key] = value;
        }
        else
        {
            _intDic.Add(key, value);
        }
    }

    public float GetValueInt(string key)
    {
        if (_intDic.ContainsKey(key))
        {
            return _intDic[key];
        }
        else
        {
            Debug.LogError("Not Contain a Int Key");
            return -1f;
        }
    }

    public void SetValueBool(string key, bool value)
    {
        if (_boolDic == null)
            _boolDic = new Dictionary<string, bool>();

        if (_boolDic.ContainsKey(key))
        {
            _boolDic[key] = value;
        }
        else
        {
            _boolDic.Add(key, value);
        }
    }

    public bool GetValueBool(string key)
    {
        if (_boolDic.ContainsKey(key))
        {
            return _boolDic[key];
        }
        else
        {
            Debug.LogError("Not Contain a Bool Key");
            return false;
        }
    }
}
