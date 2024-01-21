using System.Collections;
using UnityEngine;

namespace ReplayValue
{
    public class SquadUnit : Unit, IFogRevealer
    {
        public float ViewDistance => viewDistance;
        public Vector3 Position => transform.position;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (lockedUnit == null)
            {
                if (other.TryGetComponent<ZombieUnit>(out var zombieUnit))
                {
                    lockedUnit = zombieUnit;
                    // Debug.Log($"Locked to {zombieUnit.name}");
                }
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
