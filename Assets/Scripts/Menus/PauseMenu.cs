using System;
using UnityEngine;
using UnityEngine.UI;

namespace IR
{


public class PauseMenu : MonoBehaviour
{
        [SerializeField] Button Resume;
        [SerializeField] Button Restart;
        [SerializeField] Button Quit;

        private void Start()
        {
            Resume.onClick.AddListener(HandleResumeClicked);
            Restart.onClick.AddListener(HandleRestartClicked);
            Quit.onClick.AddListener(HandleQuitClicked);
        }

        private void HandleResumeClicked()
        {
            GameManager.Instance.TogglePause();
        }
        private void HandleRestartClicked()
        {
            GameManager.Instance.RestartGame();
        }
        private void HandleQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }
    }
}