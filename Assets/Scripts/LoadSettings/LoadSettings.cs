using UnityEngine;

public class LoadSettings : MonoBehaviour
{
    void Start()
    {
        foreach (var settingsLoader in GetComponentsInChildren<SettingsLoader>(true))
        {
            settingsLoader.Load();
        }
    }
}
