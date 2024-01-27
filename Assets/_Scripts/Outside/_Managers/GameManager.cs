using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ReplayValue
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState
        {
            MainMenu,
            Playing,
            GameOver,
        }

        public static GameManager Instance { get; private set; }
        private GameManager() { }

        public static event EventHandler OnActiveSquadUnitsCountChanged;
        public static event EventHandler OnInfectedSquadUnitsCountChanged;

        public static List<SquadUnit> activeSquadUnits;
        public static List<SquadUnit> infectedSquadUnits;

        public GameState state;

        public static int currentDay = 1;

        [SerializeField] private GameObject worldTimeUI;
        [SerializeField] private GameObject resourcesUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private TMP_Text daysSurvivedText;


        [SerializeField] private WorldTime worldTime;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            SetCursorState(CursorLockMode.Confined);

            activeSquadUnits = new List<SquadUnit>();
            infectedSquadUnits = new List<SquadUnit>();

            ChangeState(GameState.MainMenu);
            ResourceManager.Init();
        }

        private void Update()
        {
            if (IsGameOver() && state == GameState.Playing)
            {
                ChangeState(GameState.GameOver);
            }
        }

        public void ChangeState(GameState newState)
        {
            state = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    mainMenuUI.SetActive(true);
                    break;
                case GameState.Playing:
                    currentDay = 1;
                    StartCoroutine(worldTime.AddMinute());

                    ResourceManager.AddResource(ResourceManager.ResourceType.Food, 10);

                    mainMenuUI.SetActive(false);
                    worldTimeUI.SetActive(true);
                    resourcesUI.SetActive(true);
                    break;
                case GameState.GameOver:
                    worldTimeUI.SetActive(false);
                    resourcesUI.SetActive(false);
                    gameOverUI.SetActive(true);

                    daysSurvivedText.text = $"Days Survived: {currentDay}";

                    StopCoroutine(worldTime.AddMinute());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        public static void AddSquadUnit(SquadUnit squadUnit)
        {
            if (!activeSquadUnits.Contains(squadUnit))
            {
                activeSquadUnits.Add(squadUnit);
                OnActiveSquadUnitsCountChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static void AddInfectedSquadUnit(SquadUnit squadUnit)
        {
            if (!infectedSquadUnits.Contains(squadUnit))
            {
                infectedSquadUnits.Add(squadUnit);
                OnInfectedSquadUnitsCountChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public bool IsGameOver()
        {
            return activeSquadUnits.Count != 0 && infectedSquadUnits.Count != 0 && activeSquadUnits.Count == infectedSquadUnits.Count;
        }

        public static List<SquadUnit> GetActiveSquadUnits() => activeSquadUnits;
        public static int GetActiveSquadUnitsCount() => activeSquadUnits.Count;
        public static List<SquadUnit> GetInfectedSquadUnits() => infectedSquadUnits;
        public static int GetInfectedSquadUnitsCount() => infectedSquadUnits.Count;

        public void EatFood()
        {
            ResourceManager.UseResource(ResourceManager.ResourceType.Food, activeSquadUnits.Count);
        }

        public void SetCursorState(CursorLockMode cursorLockMode)
        {
            Cursor.lockState = cursorLockMode;
            switch (cursorLockMode)
            {
                case CursorLockMode.Locked:
                    Cursor.visible = false;
                    break;

                case CursorLockMode.Confined:
                    Cursor.visible = true;
                    break;

                default:
                    break;
            }
        }
    }
}
