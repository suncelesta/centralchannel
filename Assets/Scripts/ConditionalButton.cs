using System;
using UnityEngine.UI;

public class ConditionalButton
{
    private readonly Button button;
    private readonly Func<Boolean> condition;

    public ConditionalButton(Button button, Func<Boolean> condition)
    {
        this.button = button;
        this.condition = condition;
    }


    public void Update()
    {
        SetActive(condition());
    }

    public void SetActive(bool value)
    {
        button.gameObject.SetActive(value);
    }
}
