using System;
using RSG;
using UnityEngine;
using UnityEngine.UI;

public static class TutorialSteps
{
        public class FadeIn : TimingState<FadeIn>
        {
                private const float TargetAlpha = 2f;
                public override void Enter()
                {
                        base.Enter();
                        var screen = GameObject.Find("Canvas").transform.Find("TutorialScreen").GetComponent<Image>();
                        screen.gameObject.SetActive(true);
                        FadeInMaterial(screen.material).Then(() => { NextStep(); });
                }
                
                private IPromise FadeInMaterial(Material material)
                {
                        SetAlpha(material, 0);
                        return promiseTimer.WaitUntil(timeData =>
                        {
                                var newAlpha = Mathf.Lerp(material.color.a, TargetAlpha, 0.5f);
                                SetAlpha(material, newAlpha);

                                return newAlpha >= TargetAlpha - 0.01f;
                        });
                }
        
                private static void SetAlpha(Material material, float alpha)
                {
                        var color = material.color;
                        color = new Color(color.r, color.g, color.b, alpha);
                        material.color = color;
                }
        }
        
        public class AxisRotation : State<AxisRotation>
        {
                private readonly GameObject _text = FindText("AxisRotationText");
                public override void Enter()
                {
                        base.Enter();
                        _text.SetActive(true);
                }
                
                public void OnAxisRotationBegan()
                {
                        _text.SetActive(false);
                }
                
                public void OnAxisRotationFinished()
                {
                        NextStep();
                }
        } 
        
        public class CenterRotation : State<CenterRotation>
        {
                private readonly GameObject _text = FindText("CenterRotationText");
                public override void Enter()
                {
                        base.Enter();
                        _text.SetActive(true);
                }
                
                public void OnObjectGrab()
                {
                        _text.SetActive(false);
                }

                public void OnObjectUnGrab()
                {
                        NextStep();
                }
        }
        
        public class Zoom : State<Zoom>
        {
                private const String OrdinaryZoomText = "Приближайся и отдаляйся прокруткой или стрелками ↑↓";
                private const String TouchZoomText = "Приближайся и отдаляйся, сводя и разводя пальцы";
                
                private readonly GameObject _text = GetZoomText();
                
                
                private bool _zoomInProgress;
                
                public override void Enter()
                {
                        base.Enter();
                        _text.SetActive(true);
                        _zoomInProgress = false;
                }

                private static GameObject GetZoomText()
                {
                        var text = FindText("ZoomText");
                        text.GetComponent<Text>().text = Input.touchSupported ? TouchZoomText : OrdinaryZoomText;
                        return text;
                }
                
                public void OnZoom()
                {
                        if (!_zoomInProgress)
                        {
                                _text.SetActive(false);
                                _zoomInProgress = true;
                        }
                        else
                        {
                                NextStep();
                        }
                }
        }
        
        public class Hint : State<Hint>
        {
                private const float MinShowTime = 3f;
                
                private readonly GameObject _text = FindText("HintText");
                private float _showedTime;
                public override void Enter()
                {
                        base.Enter();
                        _text.SetActive(true);
                        _showedTime = Time.time;
                }
                
                public void OnObjectGrab()
                {
                        CloseHint();
                }
                
                public void OnAxisRotationBegan()
                {
                        CloseHint();
                }
                
                public void OnZoom()
                {
                        CloseHint();
                }

                private void CloseHint()
                {
                        if (Time.time - _showedTime >= MinShowTime)
                        {
                                _text.SetActive(false);
                                NextStep(); 
                        } 
                }
        }

        private static void NextStep()
        {
                GameObject.Find("Actions").GetComponent<Tutorial>().NextStep();
        }

        private static GameObject FindText(string name)
        {
                return GameObject.Find("Canvas").transform.Find(name).gameObject;
        }
}