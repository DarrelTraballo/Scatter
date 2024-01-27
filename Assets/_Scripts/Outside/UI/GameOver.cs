using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReplayValue
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button howToPlayButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private GameObject howToPlayScreen;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }

        public void PlayGame()
        {
            gameManager.ChangeState(GameManager.GameState.Playing);
        }

        public void HowToPlay()
        {
            gameObject.SetActive(false);
            howToPlayScreen.SetActive(true);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
