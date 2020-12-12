using System;
using RSG;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NavigationController : FSMBehaviour
{
    void Start()
    {
        if (Settings.LoadSeenTutorial())
        {
            ChangeStateTo(Outside.GetInstance());
        }
        else
        {
            ChangeStateTo(TutorialState.GetInstance());
        }
    }

    public void OnGetInside()
    {
        ChangeStateTo(InsideTransition.GetInstance());
    }

    public void OnInside()
    {
        ChangeStateTo(Inside.GetInstance());
    }

    public void OnHelp()
    {
        ChangeStateTo(TutorialState.GetInstance());
    }

    public void OnTutorialEnd()
    {
        ChangeStateTo(Outside.GetInstance());
    }
}