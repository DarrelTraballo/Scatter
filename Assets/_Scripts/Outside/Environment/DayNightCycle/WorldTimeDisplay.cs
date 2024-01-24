using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ReplayValue
{
    [RequireComponent(typeof(TMP_Text))]
    public class WorldTimeDisplay : MonoBehaviour
    {
        [SerializeField] private WorldTime worldTime;
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            // text.SetText(newTime.ToString(@"hh\:mm"));
            string timeString = newTime.ToString(@"hh\:") + "00\n"
                            + $"Day {worldTime.currentDay}";

            text.SetText(timeString);
        }
    }
}
