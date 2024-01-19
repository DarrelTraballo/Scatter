using System.Collections;
using System.Collections.Generic;
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

        protected override void Update()
        {
            if (shouldMove) MoveTo(targetPos);
        }

        public override void SetTargetPosition(Vector3 newTargetPos)
        {
            targetPos = newTargetPos;
            shouldMove = true;
        }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            shouldMove = false;
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
