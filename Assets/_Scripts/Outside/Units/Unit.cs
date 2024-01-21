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

        protected CircleCollider2D viewCollider;
        [SerializeField] protected Unit lockedUnit;

        public bool shouldMove = false;
        protected Vector3 targetPos;

        protected GameObject selectedCircle;

        protected virtual void Awake()
        {
            selectedCircle = transform.Find("Selected").gameObject;
            SetSelectedVisible(false);

            if (transform.Find("ViewDistance").gameObject.TryGetComponent(out viewCollider))
            {
                viewCollider.radius = viewDistance;
            }
            else
            {
                Debug.LogError("CircleCollider2D component not found on the unit!");
            }
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

        protected void UpdateViewDistance(float newViewDistance)
        {
            viewDistance = newViewDistance;
            if (viewCollider != null)
            {
                viewCollider.radius = viewDistance;
            }
        }

        public virtual void Attack(Unit unit)
        {

        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            DrawViewDistance();
            DrawLockedTarget();
        }

        private void DrawViewDistance()
        {
            Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(transform.position, viewDistance);
        }
        private void DrawLockedTarget()
        {
            Gizmos.color = Color.black;

            if (lockedUnit != null)
            {
                Gizmos.DrawLine(transform.position, lockedUnit.transform.position);
            }
        }
#endif
    }
}
