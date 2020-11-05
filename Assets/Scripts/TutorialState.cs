using System;
using System.Collections;
using System.Collections.Generic;
using RSG;
using UnityEngine;
using UnityEngine.UI;

public class TutorialState : TimingState<TutorialState>
{
    private Image _screen;

    public override void Enter()
    {
        GameObject.Find("Actions").GetComponent<Tutorial>().BeginTutorial();
    }

    public override void Exit()
    {
        PlayerPrefs.SetInt("SeenTutorial", 1);
        PlayerPrefs.Save();
    }
}