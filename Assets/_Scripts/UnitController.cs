using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;

namespace ReplayValue
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private Transform selectionAreaTransform;

        private Vector3 startPos;
        private List<Unit> selectedUnits;

        private void Awake()
        {
            selectedUnits = new List<Unit>();
            selectionAreaTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                selectionAreaTransform.gameObject.SetActive(true);
                startPos = UtilsClass.GetMouseWorldPosition();
            }

            if (Input.GetMouseButton(1))
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

            if (Input.GetMouseButtonUp(1))
            {
                Collider2D[] foundColliders = Physics2D.OverlapAreaAll(startPos, UtilsClass.GetMouseWorldPosition());
                selectionAreaTransform.gameObject.SetActive(false);

                foreach (var unit in selectedUnits)
                {
                    unit.SetSelectedVisible(false);
                }

                selectedUnits.Clear();

                foreach (var foundCollider in foundColliders)
                {
                    if (foundCollider.TryGetComponent(out Unit unit))
                    {
                        selectedUnits.Add(unit);
                        unit.SetSelectedVisible(true);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();

                List<Vector3> targetPositionList = GetPositionListAround(targetPosition, 10f, selectedUnits.Count);

                List<int> positionIndices = new List<int>();
                for (int i = 0; i < selectedUnits.Count - 1; i++)
                {
                    positionIndices.Add(i);
                }

                foreach (var unit in selectedUnits)
                {
                    // int randomPositionIndex = (int)Random.Range(1f, positionIndices.Count);
                    // positionIndices.Remove(randomPositionIndex);

                    // unit.SetTargetPosition(targetPositionList[randomPositionIndex]);

                    Vector3 randomPos = targetPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * 10f;

                    unit.SetTargetPosition(randomPos);


                    // targetPosIndex = (targetPosIndex + 1) % targetPositionList.Count;
                }
            }
        }

        private List<Vector3> GetPositionListAround(Vector3 startPos, float distance, int positionCount)
        {
            List<Vector3> positionList = new List<Vector3>();

            for (int i = 0; i < positionCount; i++)
            {
                float angle = i * (360f / positionCount);
                Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);

                Vector3 position = startPos + dir * distance;
                positionList.Add(position);
            }

            return positionList;
        }

        private Vector3 ApplyRotationToVector(Vector3 vector, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * vector;
        }
    }
}
