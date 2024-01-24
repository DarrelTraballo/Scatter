using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        #region Singleton
        private GameManager() { }
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;


            SetCursorState(CursorLockMode.Confined);
        }
        #endregion

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
