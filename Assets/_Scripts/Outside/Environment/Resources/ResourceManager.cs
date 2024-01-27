using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public static class ResourceManager
    {
        public enum ResourceType
        {
            Scraps,
            Food,
            MedKit,
        }

        public static event EventHandler OnResourceAmountChanged;

        private static Dictionary<ResourceType, int> resourceAmountDictionary;

        public static void Init()
        {
            resourceAmountDictionary = new Dictionary<ResourceType, int>();

            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                resourceAmountDictionary[resourceType] = 0;
            }
        }

        public static void AddResource(ResourceType resourceType, int amount)
        {
            resourceAmountDictionary[resourceType] += amount;
            OnResourceAmountChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void UseResource(ResourceType resourceType, int amount)
        {
            resourceAmountDictionary[resourceType] -= amount;
            OnResourceAmountChanged?.Invoke(null, EventArgs.Empty);
        }

        public static int GetResourceAmount(ResourceType resourceType) => resourceAmountDictionary[resourceType];
    }
}
