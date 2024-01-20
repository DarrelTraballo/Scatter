using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class ZombieUnit : Unit
    {
        // [SerializeField] private SquadUnit lockedUnit;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (lockedUnit != null)
            {
                SetTargetPosition(lockedUnit.transform.position);
                shouldMove = true;
            }
            else
            {
                shouldMove = false;
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
                lockedUnit = null;
                shouldMove = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out SquadUnit squadUnit))
            {
                Debug.Log($"Hit {squadUnit.name}!");
            }
        }
    }
}
