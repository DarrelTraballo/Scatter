using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;
using System.Collections;

namespace ReplayValue
{
    public class SquadController : MonoBehaviour
    {
        private float circleRadius;
        [SerializeField] private Transform selectionAreaTransform;
        [SerializeField] private Transform clickIndicatorTransform;
        [SerializeField] private float clickIndicatorDuration = 0.05f;

        private Vector3 startPos;
        private List<SquadUnit> selectedSquadUnits;

        private void Awake()
        {
            selectedSquadUnits = new List<SquadUnit>();
            selectionAreaTransform.gameObject.SetActive(false);
            clickIndicatorTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = UtilsClass.GetMouseWorldPosition();
            }

            if (Input.GetMouseButton(0))
            {
                HandleSelectionArea();
            }

            if (Input.GetMouseButtonUp(0))
            {
                SelectMultipleUnits();

                circleRadius = Mathf.Clamp(selectedSquadUnits.Count * 2 + 1, 5f, 15f);
            }

            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(ShowClick());

                Unit selectedUnit = SelectUnit();
                if (selectedUnit != null)
                {
                    Debug.Log($"Clicked {selectedUnit.name}");
                }
                else
                {
                    HandleUnitMovement();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectUnits();
            }
        }

        private Unit SelectUnit()
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();

            int layerMask = LayerMask.GetMask("Unit", "Squad", "Zombie");
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Unit selectedUnit))
                {
                    if (selectedUnit is SquadUnit squadUnit)
                    {
                        return squadUnit;
                    }

                    if (selectedUnit is ZombieUnit zombieUnit)
                    {
                        return zombieUnit;
                    }
                }
            }

            return null;
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

        private void HandleUnitMovement()
        {
            Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();

            foreach (var unit in selectedSquadUnits)
            {
                Vector3 randomPos = targetPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * circleRadius;

                unit.SetTargetPosition(randomPos);
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

        private void DeselectUnits()
        {
            foreach (var unit in selectedSquadUnits)
            {
                unit.SetSelectedVisible(false);
            }

            selectedSquadUnits.Clear();
        }

        private IEnumerator ShowClick()
        {
            clickIndicatorTransform.position = UtilsClass.GetMouseWorldPosition();
            clickIndicatorTransform.gameObject.SetActive(true);

            yield return new WaitForSeconds(clickIndicatorDuration);

            clickIndicatorTransform.gameObject.SetActive(false);
        }
    }
}
