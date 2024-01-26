using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReplayValue
{
    public class Resource : MonoBehaviour
    {
        private GameObject selectedCircle;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private ResourceData resourceData;
        private ResourceManager.ResourceType[] resourceTypes;
        private float totalResourceHealth;
        private float currentResourceHealth;

        [SerializeField] private Image healthBar;
        [SerializeField] private Canvas healthBarCanvas;

        private int resourcesCollected;

        private void Awake()
        {
            selectedCircle = transform.Find("Selected").gameObject;
            spriteRenderer = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();

            healthBarCanvas.enabled = false;

            InitResource();
            SetSelectedVisible(false);
        }

        private void InitResource()
        {
            if (resourceData == null)
            {
                Debug.Log($"No resource data found", gameObject);
                return;
            }

            resourceTypes = resourceData.resourceTypes;
            spriteRenderer.sprite = resourceData.sprites[Random.Range(0, resourceData.sprites.Length)];

            totalResourceHealth = Mathf.Round(Random.Range(resourceData.minResourceHealth, resourceData.maxResourceHealth));
            currentResourceHealth = totalResourceHealth;

            resourcesCollected = Random.Range(resourceData.minAmount, resourceData.maxAmount);
        }

        public void SetSelectedVisible(bool isVisible)
        {
            selectedCircle.SetActive(isVisible);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void TakeDamage(float amount)
        {
            healthBarCanvas.enabled = true;
            currentResourceHealth -= amount;

            healthBar.fillAmount = currentResourceHealth / totalResourceHealth;

            if (currentResourceHealth <= 0f)
            {
                Debug.Log($"{resourcesCollected} {name} has been collected");
                foreach (var resourceType in resourceTypes)
                {
                    ResourceManager.AddResource(resourceType, resourcesCollected);

                    Debug.Log($"Collected Resources : {ResourceManager.GetResourceAmount(resourceType)}");
                }
                Destroy(gameObject);
            }
        }
    }
}
