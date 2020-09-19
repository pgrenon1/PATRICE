using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool IsGodMode { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            IsGodMode = !IsGodMode;
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
    }
}
