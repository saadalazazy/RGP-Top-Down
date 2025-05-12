using UnityEngine;

public class CircularMouseFollower : MonoBehaviour
{
    [SerializeField] private float maxRange = 3f;
    [SerializeField] private float movementSpeed = 5f;
    private Camera targetCamera;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Update()
    {
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = hit.point;

            Vector3 direction = targetPos - initialPosition;
            direction.y = 0f;

            if (direction.magnitude > maxRange)
                direction = direction.normalized * maxRange;

            Vector3 desiredPosition = initialPosition + direction;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * movementSpeed);
        }
    }
}
