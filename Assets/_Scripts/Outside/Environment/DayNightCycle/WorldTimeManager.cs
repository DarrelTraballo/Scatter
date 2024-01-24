using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Reference : https://youtu.be/0nq1ZFxuEJY?si=asSwm2ywqgJxJUuA
namespace ReplayValue
{
    public class WorldTimeManager : MonoBehaviour
    {
        [SerializeField] private WorldTime worldTime;

        [SerializeField] private List<Schedule> schedules;

        [Serializable]
        private class Schedule
        {
            public int hour;
            public int minute;
            public UnityEvent action;
        }

        private void Start()
        {
            worldTime.WorldTimeChanged += CheckSchedule;
        }

        private void OnDestroy()
        {
            worldTime.WorldTimeChanged -= CheckSchedule;
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule = schedules.FirstOrDefault(s =>
                s.hour == newTime.Hours &&
                s.minute == newTime.Minutes);

            schedule?.action?.Invoke();
        }
    }
}
