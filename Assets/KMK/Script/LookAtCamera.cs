using UnityEngine;

[ExecuteAlways]
public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;        
    }

    void Update()
    {
        if (cameraTransform == null) return;

        transform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, 0, 0);
    }
}