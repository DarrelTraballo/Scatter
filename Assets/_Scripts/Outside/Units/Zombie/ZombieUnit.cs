using UnityEngine;

namespace ReplayValue
{
    public class ZombieUnit : Unit
    {
        [SerializeField] private float baseDamage;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (lockedUnit == null)
            {
                shouldMove = false;
                return;
            }

            SetTargetPosition(lockedUnit.transform.position);
            shouldMove = CanUnitSeeTarget();
        }

        public override void AttackUnit(Unit squadUnit)
        {
            Debug.Log($"{name} Attacekd {squadUnit.name}!");
            squadUnit.TakeDamage(baseDamage);
        }

        public override void TakeDamage(float amount)
        {
            healthBarCanvas.enabled = true;
            currentHealth -= amount;

            healthBar.fillAmount = currentHealth / totalHealth;

            if (currentHealth <= 0)
            {
                Debug.Log($"Killed {name}");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (lockedUnit == null)
            {
                if (other.TryGetComponent(out SquadUnit squadUnit))
                {
                    lockedUnit = squadUnit;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out SquadUnit squadUnit))
            {
                AttackUnit(squadUnit);
            }
        }
    }
}
