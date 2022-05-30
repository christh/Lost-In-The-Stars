using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private Camera dummyCamera;
        [SerializeField] private GameObject winScreen;

        private bool sceneTransitionInProgress = false;

        public Events.EventFadeComplete OnMainMenuFadeComplete;

        private void Start()
        {
            mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
            //HealthBar healthBar = HealthBar.Create(new Vector3(0, 0), new Vector3(40, 5), Color.red, Color.grey);
        }

        private void HandleMainMenuFadeComplete(FadeTypes fadeType)
        {
            winScreen.SetActive(false);
            OnMainMenuFadeComplete.Invoke(fadeType);
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.PRE_GAME)
                return;

            if ((Input.GetKeyDown(KeyCode.Space) || Input.touches.Length > 0) 
                && !sceneTransitionInProgress)
            {
                sceneTransitionInProgress = true;
                Debug.Log("Space pressed.");
                GameManager.Instance.StartGame();

                mainMenu.FadeOut();
            }
        }

        private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
        {
            pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
            gameUI.gameObject.SetActive(currentState != GameManager.GameState.PRE_GAME);
        }

        public void SetDummyCameraActive(bool active)
        {
            sceneTransitionInProgress = !active;
            dummyCamera.gameObject.SetActive(active);
        }

        public void WinState()
        {
            gameUI.enabled = false;
            winScreen.SetActive(true);
            Time.timeScale = 0.0f;
            MusicManager.Instance.StopPlayingCurrentTrack();
        }

    }
}
