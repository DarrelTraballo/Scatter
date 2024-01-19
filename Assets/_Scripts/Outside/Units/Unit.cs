using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    // TODO: base class, separate squad shit to their own class, inheriting this class
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 5f;
        public float viewDistance = 20f;

        public bool shouldMove = false;
        protected Vector3 targetPos;

        protected GameObject selectedCircle;

        protected virtual void Awake()
        {
            selectedCircle = transform.Find("Selected").gameObject;
            SetSelectedVisible(false);
        }

        protected virtual void Update()
        {
            if (shouldMove) MoveTo(targetPos);
        }

        public virtual void SetTargetPosition(Vector3 newTargetPos)
        {
            targetPos = newTargetPos;
            shouldMove = true;
        }

        protected virtual void MoveTo(Vector3 targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        public void SetSelectedVisible(bool isVisible)
        {
            selectedCircle.SetActive(isVisible);
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            shouldMove = false;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
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
