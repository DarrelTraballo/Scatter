using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;

namespace ReplayValue
{
    public class SquadController : MonoBehaviour
    {
        [SerializeField] private Transform selectionAreaTransform;
        [SerializeField] private float circleRadius;

        private Vector3 startPos;
        private List<SquadUnit> selectedSquadUnits;

        private void Awake()
        {
            selectedSquadUnits = new List<SquadUnit>();
            selectionAreaTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectionAreaTransform.gameObject.SetActive(true);
                startPos = UtilsClass.GetMouseWorldPosition();
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 currentMousePos = UtilsClass.GetMouseWorldPosition();

                Vector3 lowerLeft = new Vector3(
                    Mathf.Min(startPos.x, currentMousePos.x),
                    Mathf.Min(startPos.y, currentMousePos.y)
                );

                Vector3 upperRight = new Vector3(
                    Mathf.Max(startPos.x, currentMousePos.x),
                    Mathf.Max(startPos.y, currentMousePos.y)
                );

                selectionAreaTransform.position = lowerLeft;

                selectionAreaTransform.localScale = upperRight - lowerLeft;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Collider2D[] foundColliders = Physics2D.OverlapAreaAll(startPos, UtilsClass.GetMouseWorldPosition());
                selectionAreaTransform.gameObject.SetActive(false);

                foreach (var unit in selectedSquadUnits)
                {
                    unit.SetSelectedVisible(false);
                }

                selectedSquadUnits.Clear();

                foreach (var foundCollider in foundColliders)
                {
                    if (foundCollider.TryGetComponent(out SquadUnit unit))
                    {
                        selectedSquadUnits.Add(unit);
                        unit.SetSelectedVisible(true);
                    }
                }

                circleRadius = Mathf.Clamp(selectedSquadUnits.Count * 2 + 1, 5f, 15f);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();


                foreach (var unit in selectedSquadUnits)
                {
                    Vector3 randomPos = targetPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * circleRadius;

                    unit.SetTargetPosition(randomPos);
                }
            }
        }
    }
}
