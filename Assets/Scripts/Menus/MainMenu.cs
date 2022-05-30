using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class MainMenu : MonoBehaviour
    {
        // track the animation component
        // track the animationclips for fade
        // function that can receive animation events

        [SerializeField] Animation _mainMenuAnimator;
        [SerializeField] AnimationClip _fadeOutAnimation;
        [SerializeField] AnimationClip _fadeInAnimation;

        public Events.EventFadeComplete OnMainMenuFadeComplete;

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        }

        private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
        {
            switch (previousState)
            {
                case GameManager.GameState.PRE_GAME:
                    if (currentState == GameManager.GameState.RUNNING)
                        FadeOut();
                    break;
                case GameManager.GameState.RUNNING:
                case GameManager.GameState.PAUSED:
                    if (currentState == GameManager.GameState.PRE_GAME)
                    {
                        FadeIn();
                    }
                    break;

                default:
                    break;
            }
        }

        public void OnFadeOutComplete()
        {
            gameObject.SetActive(false);
            OnMainMenuFadeComplete.Invoke(FadeTypes.FADE_OUT);
            UIManager.Instance.SetDummyCameraActive(false);
        }

        public void OnFadeInComplete()
        {
            UIManager.Instance.SetDummyCameraActive(true);
            OnMainMenuFadeComplete.Invoke(FadeTypes.FADE_IN);
            Debug.Log("FadeIn Complete");

        }

        public void FadeIn()
        {
            gameObject.SetActive(true);
            _mainMenuAnimator.Stop();
            _mainMenuAnimator.clip = _fadeInAnimation;
            _mainMenuAnimator.Play();
        }
        public void FadeOut()
        {
            Debug.Log("Fading out!");

            _mainMenuAnimator.Stop();
            _mainMenuAnimator.clip = _fadeOutAnimation;
            _mainMenuAnimator.Play();
        }
    }
}