using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    void Update()
    {
        KeepLightRotation();
    }
    
    private void KeepLightRotation()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).LookAt(Camera.main.transform);
        }
    }
}
