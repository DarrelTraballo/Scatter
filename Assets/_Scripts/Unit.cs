using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class Unit : MonoBehaviour, IFogRevealer
    {
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private Vector3 targetPos;
        private Rigidbody2D rb;
        public bool shouldMove = false;

        private GameObject selectedCircle;

        public float viewDistance = 20f;

        public float ViewDistance => viewDistance;
        public Vector3 Position => transform.position;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            selectedCircle = transform.Find("Selected").gameObject;
            SetSelectedVisible(false);
        }

        private void Update()
        {
            if (shouldMove) MoveTo(targetPos);
        }

        public void SetTargetPosition(Vector3 newTargetPos)
        {
            targetPos = newTargetPos;
            shouldMove = true;
        }

        private void MoveTo(Vector3 targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        public void SetSelectedVisible(bool isVisible)
        {
            selectedCircle.SetActive(isVisible);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            shouldMove = false;
        }

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
