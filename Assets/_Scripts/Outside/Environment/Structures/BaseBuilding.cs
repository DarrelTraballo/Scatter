using System.Collections;
using UnityEngine;

namespace ReplayValue
{
    public class BaseBuilding : MonoBehaviour, IFogRevealer
    {
        [SerializeField] private float viewDistance = 50f;

        public float ViewDistance => viewDistance;
        public Vector3 Position => transform.position;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            DrawViewDistance();
        }

        private void DrawViewDistance()
        {
            Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(transform.position, viewDistance);
        }
#endif
    }
}
