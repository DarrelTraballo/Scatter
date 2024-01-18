using UnityEngine;

namespace ReplayValue
{
    public class BaseBuilding : MonoBehaviour, IFogRevealer
    {
        [SerializeField] private float viewDistance = 50f;

        public float ViewDistance => viewDistance;
        public Vector3 Position => transform.position;

        private void OnEnable()
        {
            FogManager.Instance.RegisterFogRevealer(this);
        }

        private void OnDisable()
        {
            if (FogManager.Instance != null)
            {
                FogManager.Instance.UnregisterFogRevealer(this);
            }
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
