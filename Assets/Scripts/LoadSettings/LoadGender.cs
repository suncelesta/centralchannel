using UnityEngine;
using UnityEngine.UI;

public class LoadGender : SettingsLoader
{
    public Toggle selectMale;
    public Toggle selectFemale;
    
    public override void Load()
    {
        switch (Settings.LoadGender())
        {
            case Settings.Gender.Male:
                selectMale.isOn = true;
                break;
            
            case Settings.Gender.Female:
                selectFemale.isOn = true;
                break;
        }
    }
}
