using UnityEngine;

public class Exit : MonoBehaviour {
    
    public void OnExitClicked()
    {
        if (!Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
