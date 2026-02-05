using UnityEngine;

[ExecuteAlways]
public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;        
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        transform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}