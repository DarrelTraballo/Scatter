using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReplayValue
{
    public class SquadUIPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentScrapsText;
        [SerializeField] private TMP_Text currentMedKitsText;

        [SerializeField] private Button upgradeWeaponButton;
        [SerializeField] private Button healSquadButton;
        [SerializeField] private Button closeIcon;

        private int currentScrapsAmount;
        private int currentMedKitsAmount;

        [SerializeField] private WeaponData[] weapons;
        private int currentWeaponIndex = 0;
        private int amountNeededToUpgrade = 20;

        private void Start()
        {
            ResourceManager.OnResourceAmountChanged += delegate (object sender, EventArgs e) { UpdateResourceText(); };
            GameManager.OnActiveSquadUnitsCountChanged += delegate (object sender, EventArgs e) { UpdateResourceText(); };
            GameManager.OnInfectedSquadUnitsCountChanged += delegate (object sender, EventArgs e) { UpdateResourceText(); };

        }

        private void Update()
        {
            if (GameManager.Instance.state == GameManager.GameState.Playing)
                UpdateResourceText();
            else return;
        }

        private void UpdateResourceText()
        {
            currentScrapsAmount = ResourceManager.GetResourceAmount(ResourceManager.ResourceType.Scraps);
            currentMedKitsAmount = ResourceManager.GetResourceAmount(ResourceManager.ResourceType.MedKit);

            int squadUnits = GameManager.activeSquadUnits.Count;

            currentScrapsText.text = $"{currentScrapsAmount} / {amountNeededToUpgrade}\nScraps\nCurrent Weapon: {weapons[currentWeaponIndex].weaponName}";
            currentMedKitsText.text = $"{currentMedKitsAmount} / {squadUnits}\nMed Kits / Squad Units";
        }

        public void UpgradeWeapon()
        {
            if (currentScrapsAmount >= amountNeededToUpgrade)
            {
                currentWeaponIndex++;
                if (currentWeaponIndex >= weapons.Length)
                {
                    Debug.Log("Final Upgrade");
                    upgradeWeaponButton.enabled = false;
                    upgradeWeaponButton.GetComponentInChildren<TMP_Text>().text = "Final Upgrade Reached";
                    return;
                }
                ResourceManager.UseResource(ResourceManager.ResourceType.Scraps, amountNeededToUpgrade);

                foreach (var squadUnit in GameManager.GetActiveSquadUnits())
                {
                    squadUnit.weaponData = weapons[currentWeaponIndex];
                    squadUnit.GetWeapon();
                }
            }
            else
            {
                Debug.Log("Not enough scraps");
            }

        }

        public void HealSquad()
        {
            if (currentMedKitsAmount > 0)
            {
                var medKitsUsed = 0;

                foreach (SquadUnit squadUnit in GameManager.GetActiveSquadUnits())
                {
                    if (squadUnit.Heal(UnityEngine.Random.Range(15, 30)))
                    {
                        medKitsUsed++;
                    }
                    else continue;
                }
                ResourceManager.UseResource(ResourceManager.ResourceType.MedKit, medKitsUsed);
            }
            else
            {
                Debug.Log("Not Enough MedKits");
            }
        }

        public void ClosePanel()
        {
            Debug.Log("Clicked Close Panel");
            gameObject.SetActive(false);
        }
    }
}
