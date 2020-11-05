using System;
using RSG;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
        public NavigationController _controller;
        
        private State[] _steps;
        private int _nextStepIdx;

        private void Start()
        {
                _steps = new State[]{
                        TutorialSteps.FadeIn.GetInstance(),
                        TutorialSteps.AxisRotation.GetInstance(),
                        TutorialSteps.CenterRotation.GetInstance(),
                        TutorialSteps.Zoom.GetInstance()
                };
        }

        public void BeginTutorial()
        {
                _nextStepIdx = 0;
                NextStep();
        }

        public void NextStep()
        {
                if (_nextStepIdx < _steps.Length)
                {
                        _controller.ChangeStateTo(_steps[_nextStepIdx]);
                        _nextStepIdx += 1;
                }
                else
                {
                     EndTutorial();   
                }
        }

        public void EndTutorial()
        {
                var screen = GameObject.Find("Canvas").transform.Find("TutorialScreen").GetComponent<Image>();
                screen.gameObject.SetActive(false);
                
                _controller.OnTutorialEnd();
        }
}