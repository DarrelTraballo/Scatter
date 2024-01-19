using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class ZombieUnit : Unit
    {
        [SerializeField] private SquadUnit lockedSquadUnit;
        private CircleCollider2D viewCollider;

        protected override void Awake()
        {
            base.Awake();

            viewCollider = GetComponent<CircleCollider2D>();
            if (viewCollider != null)
            {
                viewCollider.radius = viewDistance;
            }
            else
            {
                Debug.LogError("CircleCollider2D component not found on the zombie unit!");
            }
        }

        protected override void Update()
        {
            base.Update();

            if (lockedSquadUnit != null)
            {
                SetTargetPosition(lockedSquadUnit.transform.position);
                shouldMove = true;
            }
            else
            {
                shouldMove = false;
            }
        }

        private void UpdateViewDistance(float newViewDistance)
        {
            viewDistance = newViewDistance;
            if (viewCollider != null)
            {
                viewCollider.radius = viewDistance;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (lockedSquadUnit == null)
            {
                if (other.TryGetComponent<SquadUnit>(out var squadUnit))
                {
                    lockedSquadUnit = squadUnit;
                    SetTargetPosition(squadUnit.transform.position);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (lockedSquadUnit != null && other.gameObject == lockedSquadUnit.gameObject)
            {
                lockedSquadUnit = null;
                shouldMove = false;
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            DrawLockedTarget();
        }

        private void DrawLockedTarget()
        {
            Gizmos.color = Color.black;

            if (lockedSquadUnit != null)
            {
                Gizmos.DrawLine(transform.position, lockedSquadUnit.transform.position);
            }
        }
#endif
    }
}
