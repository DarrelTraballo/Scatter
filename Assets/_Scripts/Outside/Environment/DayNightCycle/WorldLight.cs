using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ReplayValue
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        private Light2D worldLight;

        [SerializeField] private WorldTime worldTime;

        [SerializeField] private Gradient gradient;

        private void Awake()
        {
            worldLight = GetComponent<Light2D>();
            worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            worldLight.color = gradient.Evaluate(PercentOfDay(newTime));
        }

        private float PercentOfDay(TimeSpan timeSpan)
        {
            return (float)timeSpan.TotalMinutes % 1440 / 1440;
        }
    }
}
