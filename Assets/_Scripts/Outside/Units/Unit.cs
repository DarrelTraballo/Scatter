using UnityEngine;
using UnityEngine.UI;

namespace ReplayValue
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 5f;
        public float viewDistance = 20f;
        [SerializeField] protected float attackRange;

        protected CircleCollider2D viewCollider;

        [HideInInspector] public Unit lockedUnit;

        [SerializeField] protected float totalHealth;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected Image healthBar;
        [SerializeField] protected Canvas healthBarCanvas;

        [Space]
        [Header("Raycasting shenanigans")]
        protected RaycastHit2D[] hits = new RaycastHit2D[5];
        [SerializeField] protected ContactFilter2D contactFilter;

        protected bool shouldMove = false;
        protected Vector3 targetPos;

        protected GameObject selectedCircle;
        protected GameObject viewDistSprite;

        public abstract void AttackUnit(Unit unit);
        public abstract void TakeDamage(float amount);

        protected virtual void Awake()
        {
            selectedCircle = transform.Find("Selected").gameObject;

            healthBarCanvas.enabled = false;
            currentHealth = totalHealth;

            UpdateViewDistance(viewDistance);
            UpdateAttackRange(attackRange);
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
            viewDistSprite.SetActive(isVisible);
        }

        protected bool CanUnitSeeTarget()
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
                    if (hit.transform.TryGetComponent(out Unit unit))
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

        protected void UpdateViewDistance(float newViewDistance)
        {

            GameObject viewDistanceGO = transform.Find("ViewDistance").gameObject;
            viewDistSprite = viewDistanceGO.transform.Find("View Distance Sprite").gameObject;

            if (viewDistanceGO.TryGetComponent(out viewCollider))
            {
                viewCollider.radius = newViewDistance;
            }
            else
            {
                Debug.LogError("CircleCollider2D component not found on the unit!");
            }
        }

        protected void UpdateAttackRange(float newAttackRange)
        {
            if (viewDistSprite != null)
            {
                viewDistSprite.transform.localScale = new Vector2(newAttackRange, newAttackRange) * 2f;
            }
            else
            {
                Debug.LogError("No SpriteRenderer component found! D:");
            }

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
