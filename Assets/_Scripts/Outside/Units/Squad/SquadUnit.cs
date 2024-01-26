using System.Collections;
using UnityEngine;

namespace ReplayValue
{
    public class SquadUnit : Unit, IFogRevealer
    {
        public float ViewDistance => viewDistance;
        public Vector3 Position => transform.position;

        [SerializeField] private WeaponData weaponData;
        [SerializeField] private WeaponData defaultWeapon;
        protected GameObject weaponHolder;

        [SerializeField] private float baseDamage;
        [SerializeField] private float fireRate;
        [SerializeField] private float fireCooldown = 0f;

        [HideInInspector] public Resource lockedResource;
        [SerializeField] private float collectDamage = 10f;
        [SerializeField] private float collectRate = 2f;
        [SerializeField] private float collectCooldown = 0f;
        [SerializeField] private float collectDistance = 5f;

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

            Attack();

            Collect();
        }

        private void GetWeapon()
        {
            if (weaponData == null)
            {
                weaponData = defaultWeapon;
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

        private void Attack()
        {
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

        public override void AttackUnit(Unit unit)
        {
            lockedUnit = unit;
            lockedResource = null;
        }

        public override void TakeDamage(float amount)
        {
            healthBarCanvas.enabled = true;
            currentHealth += amount;

            healthBar.fillAmount = currentHealth / totalHealth;

            if (currentHealth >= totalHealth)
            {
                Debug.Log($"{name} got fully infected");
                if (!gameManager.infectedSquadUnits.Contains(this))
                    gameManager.infectedSquadUnits.Add(this);
            }
        }

        private void Collect()
        {
            if (lockedResource == null) return;
            SetTargetPosition(lockedResource.transform.position);

            float distance = Vector2.Distance(transform.position, lockedResource.transform.position);

            if (distance <= collectDistance)
            {
                shouldMove = false;
                // use weapon
                if (collectCooldown <= 0)
                {
                    DoCollect();
                    collectCooldown = 1f / collectRate;
                }

            }
            collectCooldown -= Time.deltaTime;
        }

        public void CollectResource(Resource resource)
        {
            lockedUnit = null;
            lockedResource = resource;
            Debug.Log($"Locked Resource : {lockedResource.name}");
        }

        private void DoCollect()
        {
            if (lockedResource == null) return;
            lockedResource.TakeDamage(collectDamage);
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
