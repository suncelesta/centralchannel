using UnityEngine;

public class Exit : MonoBehaviour {
    
    public void OnExitClicked()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
        }
    }
}
