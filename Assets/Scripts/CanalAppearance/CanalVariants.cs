using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalVariants : MonoBehaviour
{
    public SideCanal left;
    public SideCanal right;
    public GameObject maleBody;
    public GameObject femaleBody;

    public void MakeMale(bool doMake)
    {
        if (doMake)
        {
            left.MakeRed();
            right.MakeWhite();
            maleBody.SetActive(true);
            femaleBody.SetActive(false);
            Settings.SaveGender(Settings.Gender.Male);
        }
    }

    public void MakeFemale(bool doMake)
    {
        if (doMake)
        {
            left.MakeWhite();
            right.MakeRed();
            maleBody.SetActive(false);
            femaleBody.SetActive(true);
            Settings.SaveGender(Settings.Gender.Female);
        }
    }
}
