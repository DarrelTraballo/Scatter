using System.Collections;
using UnityEngine;

namespace ReplayValue
{
    public class SquadUnit : Unit, IFogRevealer
    {
        public float ViewDistance => viewDistance;
        public Vector3 Position => transform.position;

        // TODO: change to weapon system
        // ScriptableObjects!
        [SerializeField] private float baseDamage = 5f;
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private float fireCooldown = 0f;

        [SerializeField] private WeaponData weaponData;
        protected GameObject weaponHolder;

        // TODO: weapon projectiles

        protected override void Awake()
        {
            base.Awake();

            weaponHolder = transform.Find("Weapon Holder").gameObject;

            currentHealth = 0;

            GetWeapon();
        }

        protected override void Update()
        {
            base.Update();

            if (lockedUnit == null) return;

            SetTargetPosition(lockedUnit.transform.position);

            float distance = Vector2.Distance(transform.position, lockedUnit.transform.position);

            if (distance <= attackRange - 1)
            {
                PointWeaponAtTarget();
                shouldMove = false;
                // use weapon
                if (fireCooldown <= 0)
                {
                    UseWeapon();
                    fireCooldown = 1f / fireRate;
                }

            }
            fireCooldown -= Time.deltaTime;
        }

        private void GetWeapon()
        {
            if (weaponData == null)
            {
                Debug.LogError($"No Weapon Data given to {name}");
                return;
            }

            baseDamage = weaponData.baseDamage;
            fireRate = weaponData.fireRate;
            attackRange = weaponData.attackRange;
            var weapon = Instantiate(weaponData.weaponPrefab, transform);
            UpdateAttackRange(attackRange);
        }

        private void PointWeaponAtTarget()
        {
            Vector2 dir = lockedUnit.transform.position - weaponHolder.transform.position;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            weaponHolder.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        private void UseWeapon()
        {
            if (lockedUnit == null) return;
            lockedUnit.TakeDamage(baseDamage);
        }

        public override void AttackUnit(Unit unit)
        {
            Debug.Log($"{name} is Attacking {unit.name}");
            lockedUnit = unit;
        }

        public override void TakeDamage(float amount)
        {
            healthBarCanvas.enabled = true;
            currentHealth += amount;

            healthBar.fillAmount = currentHealth / totalHealth;

            if (currentHealth >= totalHealth)
            {
                Debug.Log($"{name} got fully infected");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (lockedUnit != null && other.gameObject == lockedUnit.gameObject)
            {
                lockedUnit = null;
            }
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForSingleton());
        }

        private void OnDisable()
        {
            FogManager.Instance?.UnregisterFogRevealer(this);
        }

        private IEnumerator WaitForSingleton()
        {
            yield return new WaitUntil(() => FogManager.Instance != null);
            FogManager.Instance?.RegisterFogRevealer(this);
        }
    }
}
