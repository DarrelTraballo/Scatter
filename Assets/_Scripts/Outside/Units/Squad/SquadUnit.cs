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
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private float fireCooldown = 0f;
        [SerializeField] private float baseDamage = 5f;

        // TODO: weapon projectiles

        protected override void Awake()
        {
            base.Awake();

            currentHealth = 0;
        }

        protected override void Update()
        {
            base.Update();

            if (lockedUnit == null) return;

            SetTargetPosition(lockedUnit.transform.position);

            float distance = Vector2.Distance(transform.position, lockedUnit.transform.position);

            if (distance <= viewDistance - 2)
            {
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
