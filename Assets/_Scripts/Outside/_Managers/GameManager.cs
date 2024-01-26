using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        private GameManager() { }

        public List<SquadUnit> activeSquadUnits;
        public List<SquadUnit> infectedSquadUnits;

        public static int currentDay = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            SetCursorState(CursorLockMode.Confined);
            ResourceManager.Init();

            activeSquadUnits = new List<SquadUnit>();
            infectedSquadUnits = new List<SquadUnit>();
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
