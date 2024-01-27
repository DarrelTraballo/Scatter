using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ReplayValue
{
    public class ResourcesUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text scrapsText;
        [SerializeField] private TMP_Text foodText;
        [SerializeField] private TMP_Text medKitText;

        private void Start()
        {
            ResourceManager.OnResourceAmountChanged += delegate (object sender, EventArgs e) { UpdateResourceText(); };
        }

        private void Update()
        {
            if (GameManager.Instance.state == GameManager.GameState.Playing)
                UpdateResourceText();
            else return;
        }

        private void UpdateResourceText()
        {
            scrapsText.text = $"Scraps : {ResourceManager.GetResourceAmount(ResourceManager.ResourceType.Scraps)}";
            foodText.text = $"Food : {ResourceManager.GetResourceAmount(ResourceManager.ResourceType.Food)}";
            medKitText.text = $"Med Kit : {ResourceManager.GetResourceAmount(ResourceManager.ResourceType.MedKit)}";
        }
    }
}
