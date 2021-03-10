using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool isCloverEnough;
    public static int stageNum;
    public static int themaNum;
    public static Sprite spriteThema;

    public static bool canPlayerMove = false;

    private void Awake()
    {
        instance = this;
    }
}
