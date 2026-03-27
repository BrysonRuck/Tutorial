using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
               LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted

    }
    [SerializeField] private Mode mode;
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                transform.LookAt(transform.position + (transform.position - Camera.main.transform.position));
                break;
                case Mode.CameraForward:
                transform.rotation = Camera.main.transform.rotation;
                break;
                case Mode.CameraForwardInverted:
                transform.rotation = Quaternion.Inverse(Camera.main.transform.rotation);
                break;
        }
    }
}
