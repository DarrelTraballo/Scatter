using UnityEngine;

namespace ReplayValue
{
    public class CameraController : MonoBehaviour
    {
        private Camera childCamera;

        [Header("Camera Movement")]
        public float basePanSpeed;
        public float panLerpSpeed;
        public float panBorderThickness;
        [Tooltip("For camera bounds")]
        [SerializeField] private Transform groundPlaneTransform;

        [Header("Camera Zoom")]
        public float zoomSpeed;
        public float minZoom;
        public float maxZoom;

        [SerializeField] private bool isCameraLocked = false;

        private void Awake()
        {
            childCamera = transform.GetChild(0).GetComponent<Camera>();
        }

        private void Update()
        {
            if (GameManager.Instance.state == GameManager.GameState.MainMenu || GameManager.Instance.state == GameManager.GameState.GameOver) return;

            HandleZoom();
            if (!isCameraLocked)
            {
                MoveCamearaUsingScreenEdges();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                isCameraLocked = !isCameraLocked;
            }
        }

        private void HandleZoom()
        {
            // Zoom in and out using the mouse scroll wheel
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            childCamera.orthographicSize -= scrollWheel * zoomSpeed;

            // Clamp the orthographic size within the specified limits
            childCamera.orthographicSize = Mathf.Clamp(childCamera.orthographicSize, minZoom, maxZoom);

        }

        private void MoveCamearaUsingScreenEdges()
        {
            // Adjust pan speed based on zoom level
            float panSpeed = basePanSpeed * childCamera.orthographicSize;

            Vector3 moveDirection = Vector3.zero;

            // If mouse is on top side of screen
            if (Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                moveDirection += Vector3.up;
            }

            if (Input.mousePosition.y <= panBorderThickness)
            {
                moveDirection += Vector3.down;
            }

            if (Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                moveDirection += Vector3.right;
            }

            if (Input.mousePosition.x <= panBorderThickness)
            {
                moveDirection += Vector3.left;
            }

            Vector3 newPos = transform.position + panSpeed * Time.deltaTime * moveDirection.normalized;

            float boundsX = groundPlaneTransform.localScale.x / 2;
            float boundsY = groundPlaneTransform.localScale.y / 2;

            newPos.x = Mathf.Clamp(newPos.x, -boundsX, boundsX);
            newPos.y = Mathf.Clamp(newPos.y, -boundsY, boundsY);

            transform.position = newPos;
        }
    }
}
