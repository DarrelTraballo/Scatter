using UnityEngine;

namespace ReplayValue
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Object/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;

        public float baseDamage;
        public float attackRange;
        public float fireRate;

        public GameObject weaponPrefab;
    }
}
