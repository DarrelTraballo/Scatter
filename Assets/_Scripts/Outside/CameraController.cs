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

        [Header("Camera Zoom")]
        public float zoomSpeed;
        public float minZoom;
        public float maxZoom;

        private bool isDragging;
        private Vector3 lastMousePosition;
        private Vector3 targetPosition;

        private void Awake()
        {
            targetPosition = transform.position;
            childCamera = transform.GetChild(0).GetComponent<Camera>();
        }

        private void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            // Zoom in and out using the mouse scroll wheel
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            childCamera.orthographicSize -= scrollWheel * zoomSpeed;

            // Clamp the orthographic size within the specified limits
            childCamera.orthographicSize = Mathf.Clamp(childCamera.orthographicSize, minZoom, maxZoom);

            MoveCamearaUsingScreenEdges();
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

            transform.Translate(panSpeed * Time.deltaTime * moveDirection.normalized, Space.World);
        }

        private void MoveCameraWithMouseClick()
        {
            // Adjust pan speed based on zoom level
            float panSpeed = basePanSpeed * childCamera.orthographicSize;
            // Camera movement using the mouse
            if (Input.GetMouseButtonDown(1))  // Right mouse button
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }

            else if (Input.GetMouseButtonUp(1))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
                targetPosition += panSpeed * Time.deltaTime * -deltaMouse;

                // Smoothly interpolate between the current position and the target position

                lastMousePosition = Input.mousePosition;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, panLerpSpeed * Time.deltaTime);

        }
    }
}
