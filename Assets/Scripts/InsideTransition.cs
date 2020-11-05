using UnityEngine;
using RSG;
using UnityEngine.UI;
using System;
using Debug = System.Diagnostics.Debug;

public class InsideTransition : TimingState<InsideTransition>
{
    private Transform _camera;
    private Transform _canal;
    private Image _lightImage;
    private GameObject[] _controls;
    private Zoom _zoom;
    private RotateObject _rotateCanal;
    private static readonly int TintColor = Shader.PropertyToID("_Tint");

    public override void Enter()
    {
        _camera = Camera.main.transform;
        _canal = GameObject.Find("Canals").transform;
        _lightImage = GameObject.Find("Canvas").transform.Find("Light image").GetComponent<Image>();
        _controls = GameObject.FindGameObjectsWithTag("Outside Controls");
        
        _zoom = _camera.gameObject.GetComponentInParent<Zoom>();
        _zoom.enabled = false;
        
        _rotateCanal = GameObject.Find("Canals").gameObject.GetComponent<RotateObject>();
        _rotateCanal.enabled = false;

        var lightColor = _lightImage.color;
        lightColor.a = 0;
        _lightImage.color = lightColor;
        
        RotateCanal(5f, 4f, CameraDownRotation())
        .Then(() => FillWithLightWhile(() => Move(10f, 7f), 0.01f, 0.01f))
        .Then(() => promiseTimer.WaitFor(2f))
        .Then(() => EndTransition());
    }

    private Quaternion CameraDownRotation()
    {
        var toCamera = _camera.position - _canal.transform.position;
        var targetRotation = Quaternion.LookRotation(-_camera.up, toCamera);
        return targetRotation;
    }

    private IPromise RotateCanal(float initSpeed, float acceleration, Quaternion targetRotation) 
    {
        return promiseTimer.WaitUntil(Accelerate(initSpeed, acceleration, (timeData, speed) =>
        {
            var rotateTowards = Quaternion.RotateTowards(
                _canal.transform.rotation,
                targetRotation,
                speed * Time.deltaTime
            );

            _canal.transform.rotation = rotateTowards;

            return rotateTowards == targetRotation;
        }));
    }

    private IPromise Move(float initSpeed, float acceleration)
    {
        return promiseTimer.WaitUntil(Accelerate(initSpeed, acceleration, (timeData, speed) =>
        {
            var canalPosition = _canal.position;
            var cameraPosition = _camera.position;
            cameraPosition += Time.deltaTime * speed * 0.02f * (canalPosition - cameraPosition);
            _camera.position = cameraPosition;


            return cameraPosition == canalPosition;
        }));
    }

    private IPromise FillWithLightWhile(Func<IPromise> action, float initSpeed, float acceleration)
    {
        Vector3 finalScale = GetFinalImageScale(_lightImage);

        //MakeImageOpaque(_lightImage);
        foreach (var control in _controls) {
            control.SetActive(false);
        }

        var actionInProgress = true;

        var fill = promiseTimer
            .WaitFor(0.5f)
            .Then(() => promiseTimer.WaitWhile(Accelerate(initSpeed, acceleration, (timeData, speed) =>
            {
                var alpha = Mathf.Lerp(_lightImage.color.a, 1f, Time.deltaTime * 2f);
                SetColorImage(alpha, speed);
                _lightImage.rectTransform.localScale = Vector3.Slerp(
                _lightImage.rectTransform.localScale, finalScale,
                speed * Time.deltaTime
                );

            return actionInProgress;
        })));

        return Promise.Race(
                action().Finally(() => actionInProgress = false),
                fill
            );
    }
    
    private void SetColorImage(float alpha, float speed)
    {
        var color = _lightImage.color;
        color = new Color (color.r,color.g, color.b, alpha);
        _lightImage.color = color;
    }

    private IPromise EndTransition()
    {
        var controller = GameObject.Find("Actions").GetComponent<NavigationController>();
        controller.OnInside();
        return Promise.Resolved();
    }

    private static void MakeImageOpaque(Image image)
    {
        var lightColor = image.color;
        lightColor.a = 1;
        image.color = lightColor;
    }

    private static Vector3 GetFinalImageScale(Image image)
    {
        var rectTransform = image.rectTransform;
        var initialScale = rectTransform.localScale;
        var initialSize = rectTransform.sizeDelta;
        var finalScale = 5f *
            Mathf.Max(Screen.height, Screen.width) / Mathf.Max(initialSize.x, initialSize.y) * initialScale;
        return finalScale;
    }

    private static Func<TimeData, bool> Accelerate(float initSpeed, float acceleration, Func<TimeData, float, bool> predicate)
    {
        var speed = initSpeed;
        return timeData =>
        {
            var result = predicate(timeData, speed);
            speed += acceleration;
            return result;
        };
    }
}
