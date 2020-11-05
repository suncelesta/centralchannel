using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class SceneFader : MonoBehaviour {
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f; 
    
    private Image _progressBarImage;
    private Text _progressBarText;
    private static readonly int Progress = Shader.PropertyToID("_Progress");

    public enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }

    private void Start()
    {
        _progressBarImage = GameObject.Find("Progress").GetComponent<Image>();
        _progressBarImage.material.SetFloat(Progress, 0);
        _progressBarText = GameObject.Find("ProgressText").GetComponent<Text>();

        StartCoroutine(FadeAndLoadScene(FadeDirection.Out, "MainScene"));
    }
    private IEnumerator Fade(FadeDirection fadeDirection) 
    {
        float alpha = (fadeDirection == FadeDirection.Out)? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out)? 0 : 1;
        if (fadeDirection == FadeDirection.Out) {
            while (alpha >= fadeEndValue)
            {
                SetColorImage (ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false; 
        } else {
            fadeOutUIImage.enabled = true; 
            while (alpha <= fadeEndValue)
            {
                SetColorImage (ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneToLoad) 
    {
        yield return AsynchronousLoad(sceneToLoad);
        yield return Fade(fadeDirection);
    }
    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        var color = fadeOutUIImage.color;
        color = new Color (color.r,color.g, color.b, alpha);
        fadeOutUIImage.color = color;
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out)? -1 : 1) ;
    }

    private IEnumerator AsynchronousLoad(string scene)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        QualitySettings.asyncUploadTimeSlice = 4;
        QualitySettings.asyncUploadBufferSize = 16;
        QualitySettings.asyncUploadPersistentBuffer = true;
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            var progress = Mathf.Clamp01(ao.progress / 0.9f);
            var visualProgress = progress - 0.05f;
            _progressBarImage.material.SetFloat(Progress, visualProgress);
            _progressBarText.text = string.Format("{0:0.0}%", Math.Max(visualProgress * 100, 0));

            // Loading completed
            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
        
        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }
}