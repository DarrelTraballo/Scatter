using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Scriptable Object/Resource")]
    public class ResourceData : ScriptableObject
    {
        public string resourceName;

        [Header("How long it takes to collect resource")]
        public float minResourceHealth;
        public float maxResourceHealth;

        [Header("Amount given when collected")]
        public int minAmount;
        public int maxAmount;

        [Header("Resource Type and Sprites")]
        public ResourceManager.ResourceType[] resourceTypes;
        public Sprite[] sprites;
    }
}
