using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensa;
    private Transform parent;

    private void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        parent.Rotate(Vector3.up, Input.GetAxis("Mouse X") * mouseSensa * Time.deltaTime);
    }
}
