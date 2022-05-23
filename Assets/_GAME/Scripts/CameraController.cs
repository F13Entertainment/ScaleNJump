using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 Offset;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Lerp(Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude / 3f), 60f, 70f));
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Ball.Instance.transform.position.x, transform.position.y, transform.position.z) + Offset, ref velocity, .55f);
    }

    private void Lerp(float target)
    {
        cam.fieldOfView = Mathf.Clamp(target > cam.fieldOfView ? cam.fieldOfView + (Time.fixedDeltaTime) : cam.fieldOfView - Time.fixedDeltaTime, 60f, 70f);
    }

    private void OnDestroy()
    {
        cam?.transform.DOKill();
    }
}