using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class WorldTime : MonoBehaviour
    {
        public EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField] private float dayLengthInMinutes;

        private TimeSpan currentTime;
        public int currentDay = 1;
        private float MinuteLength => dayLengthInMinutes / 60;

        private void Start()
        {
            StartCoroutine(AddMinute());
        }

        private IEnumerator AddMinute()
        {
            currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, currentTime);
            yield return new WaitForSeconds(MinuteLength);

            if (currentTime.TotalDays % dayLengthInMinutes == 0)
            {
                Debug.Log($"Day Incremented at {currentTime}");
                GameManager.currentDay++;
            }

            StartCoroutine(AddMinute());
        }
    }
}
