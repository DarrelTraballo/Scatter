using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class ZombieUnit : Unit
    {
        // [SerializeField] private SquadUnit lockedUnit;

        public RaycastHit2D[] hits = new RaycastHit2D[5];
        public ContactFilter2D contactFilter;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (lockedUnit != null)
            {
                if (CanZombieStillSee())
                {
                    SetTargetPosition(lockedUnit.transform.position);
                    shouldMove = true;
                }
                else
                {
                    shouldMove = false;
                }
            }
            else
            {
                shouldMove = false;
            }
        }

        public override void Attack(Unit squadUnit)
        {
            Debug.Log($"Attacekd {squadUnit.name}!");
        }

        private bool CanZombieStillSee()
        {
            Vector2 direction = lockedUnit.transform.position - transform.position;
            direction.Normalize();
            float maxDistance = Vector3.Distance(transform.position, lockedUnit.transform.position);
            int hitCount = Physics2D.Raycast(transform.position, direction, contactFilter, hits, maxDistance);

            if (hitCount > 0)
            {
                RaycastHit2D hit = hits[0];
                Debug.DrawRay(transform.position, direction * maxDistance, Color.green);

                if (hit.transform == lockedUnit.transform)
                {
                    Debug.Log("Path to target is clear!");
                    return true;
                }
                else
                {
                    if (hit.transform.TryGetComponent(out SquadUnit squadUnit))
                    {
                        Debug.Log($"Another Squad Unit {hit.collider.name}", hit.collider.gameObject);
                        return true;
                    }
                    Debug.Log($"Path to target is blocked by {hit.collider.name}", hit.collider.gameObject);
                    lockedUnit = null;
                    return false;
                }
            }
            else
            {
                Debug.Log("No object was hit by the raycast.");
                Debug.DrawRay(transform.position, direction, Color.red);
                return false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (lockedUnit == null)
            {
                if (other.TryGetComponent<SquadUnit>(out var squadUnit))
                {
                    lockedUnit = squadUnit;
                    SetTargetPosition(squadUnit.transform.position);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (lockedUnit != null && other.gameObject == lockedUnit.gameObject)
            {

            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out SquadUnit squadUnit))
            {
                Attack(squadUnit);
            }
        }
    }
}
