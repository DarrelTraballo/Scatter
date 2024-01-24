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
        private List<ZombieUnit> selectedZombieUnits;

        private void Awake()
        {
            selectedSquadUnits = new List<SquadUnit>();
            selectedZombieUnits = new List<ZombieUnit>();
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
                circleRadius = Mathf.Clamp(selectedSquadUnits.Count * 2 + 1, 5f, 15f);
                SelectMultipleUnits();
            }

            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(ShowClick());

                Unit selectedUnit = SelectUnit();
                if (selectedUnit != null)
                {
                    Debug.Log($"Clicked {selectedUnit.name}");

                    if (selectedUnit is ZombieUnit zombie)
                    {
                        foreach (var unit in selectedSquadUnits)
                        {
                            unit.AttackUnit(zombie);
                        }
                    }
                }
                else
                {
                    foreach (var unit in selectedSquadUnits)
                    {
                        unit.lockedUnit = null;
                    }

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
                    selectedUnit.SetSelectedVisible(true);
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
                if (foundCollider.TryGetComponent(out Unit unit))
                {
                    if (unit is SquadUnit squadUnit)
                    {
                        selectedSquadUnits.Add(squadUnit);
                    }
                    if (unit is ZombieUnit zombieUnit)
                    {
                        selectedZombieUnits.Add(zombieUnit);
                    }
                    unit.SetSelectedVisible(true);
                }
            }
        }

        private void HandleUnitMovement()
        {
            Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();

            foreach (var unit in selectedSquadUnits)
            {
                if (unit is SquadUnit squadUnit)
                {
                    Vector3 randomPos = targetPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * circleRadius;

                    squadUnit.SetTargetPosition(randomPos);
                }
                else continue;
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
            foreach (var squadUnit in selectedSquadUnits)
            {
                squadUnit.SetSelectedVisible(false);
            }

            foreach (var zombieUnit in selectedZombieUnits)
            {
                zombieUnit.SetSelectedVisible(false);
            }

            selectedSquadUnits.Clear();
            selectedZombieUnits.Clear();
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
