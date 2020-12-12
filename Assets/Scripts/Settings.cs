using System;
using UnityEngine;

public static class Settings
{
    private static String SeenTutorialKey = "SeenTutorial";
    private static String GenderKey = "Gender";

    public enum Gender
    {
        Male,
        Female
    }

    public static bool LoadSeenTutorial()
    {
        return PlayerPrefs.HasKey(SeenTutorialKey);
    }

    public static void SaveSeenTutorial()
    {
        PlayerPrefs.SetInt(SeenTutorialKey, 1);
        PlayerPrefs.Save();
    }
    
    public static Gender LoadGender()
    {
        return Enum.TryParse<Gender>(PlayerPrefs.GetString(GenderKey), out var savedGender)
            ? savedGender
            : Gender.Male;
    }

    public static void SaveGender(Gender gender)
    {
        PlayerPrefs.SetString(GenderKey, gender.ToString());
        PlayerPrefs.Save();
    }
}