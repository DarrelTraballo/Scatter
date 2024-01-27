using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReplayValue
{
    public class HowToPlay : MonoBehaviour
    {
        [SerializeField] private Button backButton;

        [SerializeField] private GameObject mainMenuScreen;

        public void BackToMainMenu()
        {
            gameObject.SetActive(false);
            mainMenuScreen.SetActive(true);
        }
    }
}
