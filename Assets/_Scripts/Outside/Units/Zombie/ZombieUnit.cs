using System.Collections;
using UnityEngine;

namespace ReplayValue
{
    public class ZombieUnit : Unit
    {
        private enum AIState { Roam, Chase }
        [SerializeField] private AIState currentState;
        private float roamRadius;

        [SerializeField] private float baseDamage;

        private Coroutine roamCoroutine;

        protected override void Awake()
        {
            base.Awake();

            roamRadius = viewDistance / 2f;
            currentState = AIState.Roam;
        }

        private void Start()
        {
            if (currentState == AIState.Roam)
            {
                roamCoroutine = StartCoroutine(RoamCoroutine());
            }
        }

        protected override void Update()
        {
            base.Update();

            if (lockedUnit == null)
            {
                currentState = AIState.Roam;
                if (!IsInvoking("RoamCoroutine"))
                {
                    roamCoroutine = StartCoroutine(RoamCoroutine());
                }
                return;
            }
            else
            {
                SetTargetPosition(lockedUnit.transform.position);
                currentState = AIState.Chase;
                shouldMove = CanUnitSeeTarget();
                StopCoroutine(roamCoroutine);
            }

        }

        private IEnumerator RoamCoroutine()
        {
            while (true)
            {
                // Generate a random point within a circle of radius 'roamRadius'
                Vector2 randomPoint = Random.insideUnitCircle * roamRadius;

                // Convert the 2D point to a 3D point
                targetPos = transform.position + (Vector3)randomPoint;

                SetTargetPosition(targetPos);

                // Wait until the zombie reaches the target position
                yield return new WaitUntil(() => (targetPos - transform.position).sqrMagnitude <= 0.01f);
            }
        }

        private void Roam()
        {
            SetTargetPosition(targetPos);
        }

        private void SetNextRoamTargetPosition()
        {
            Vector2 randomPoint = Random.insideUnitCircle * roamRadius;
            targetPos = transform.position + new Vector3(randomPoint.x, randomPoint.y);

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
                lockedUnit = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (lockedUnit == null)
            {
                if (other.TryGetComponent(out SquadUnit squadUnit))
                {
                    currentState = AIState.Chase;
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
