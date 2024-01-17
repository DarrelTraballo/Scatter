using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private Vector3 targetPos;
        private Rigidbody2D rb;
        public bool shouldMove = false;

        private GameObject selectedCircle;

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
    }
}
