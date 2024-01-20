using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;

namespace ReplayValue
{
    public class SquadController : MonoBehaviour
    {
        [SerializeField] private float circleRadius; // ignore this circleRadius variable in this code
        [SerializeField] private Transform selectionAreaTransform;

        private Vector3 startPos;
        private List<SquadUnit> selectedSquadUnits;

        private bool isDragging = false;

        private void Awake()
        {
            selectedSquadUnits = new List<SquadUnit>();
            selectionAreaTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = UtilsClass.GetMouseWorldPosition();

                if (!isDragging)
                {
                    HandleUnitMovement();
                }
            }

            if (Input.GetMouseButton(0))
            {
                isDragging = true;
                HandleSelectionArea();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    SelectMultipleUnits();
                }

                isDragging = false;
                circleRadius = Mathf.Clamp(selectedSquadUnits.Count * 2 + 1, 5f, 15f);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectUnits();
            }
        }

        private void SelectMultipleUnits()
        {
            Collider2D[] foundColliders = Physics2D.OverlapAreaAll(startPos, UtilsClass.GetMouseWorldPosition());
            selectionAreaTransform.gameObject.SetActive(false);

            foreach (var foundCollider in foundColliders)
            {
                if (foundCollider.TryGetComponent(out SquadUnit unit))
                {
                    selectedSquadUnits.Add(unit);
                    unit.SetSelectedVisible(true);
                }
            }
        }

        private void HandleSelectionArea()
        {
            selectionAreaTransform.gameObject.SetActive(true);
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

        private void HandleUnitMovement()
        {
            Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();

            foreach (var unit in selectedSquadUnits)
            {
                Vector3 randomPos = targetPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * circleRadius;

                unit.SetTargetPosition(randomPos);
            }
        }

        private void DeselectUnits()
        {
            foreach (var unit in selectedSquadUnits)
            {
                unit.SetSelectedVisible(false);
            }

            selectedSquadUnits.Clear();
        }
    }
}
