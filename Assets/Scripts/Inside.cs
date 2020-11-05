using UnityEngine;
using RSG;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inside : TimingState<Inside>
{
    private GameObject _blackMirror;
    private GameObject _exitCanalButton;

    public override void Enter()
    {
	    _exitCanalButton = GameObject.Find("Canvas").transform.Find("ExitCanalButton").gameObject;
	    _exitCanalButton.SetActive(false);
	    
        promiseTimer.WaitFor(2f)
        .Then(() => FadeInButton(0.1f));
    }

    private IPromise FadeInButton(float speed)
    {
		var image = _exitCanalButton.GetComponent<Image>();
        var text = _exitCanalButton.GetComponentInChildren<Text>();
        var alpha = 0f;

        _exitCanalButton.SetActive(true);

        return promiseTimer.WaitWhile(timeData =>
		{
            alpha += speed;
            SetAlpha(image, alpha);
            SetAlpha(text, alpha);

            return alpha < 1;
		});
	}

    private static void SetAlpha(Graphic obj, float alpha)
	{
		var color = obj.color;
		color.a = alpha;
        obj.color = color;
	}
    
    public void ExitCanal()
    {
	    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
