using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalVariants : MonoBehaviour
{
    public SideCanal left;
    public SideCanal right;

    public void MakeMale(bool doMake)
    {
        if (doMake)
        {
            left.MakeRed();
            right.MakeWhite();
        }
    }
    
    public void MakeFemale(bool doMake)
    {
        if (doMake)
        {
            left.MakeWhite();
            right.MakeRed();
        }
    }
}
