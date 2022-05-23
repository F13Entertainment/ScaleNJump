using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 Offset;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        //cam?.transform.DOKill();
        Lerp(Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude / 3f), 60f, 70f));
        //cam?.DOFieldOfView(Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude / 3f), 60f, 70f), .5f);
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Ball.Instance.transform.position.x, transform.position.y, transform.position.z) + Offset, ref velocity, .55f);
    }

    private void Lerp(float target)
    {
        cam.fieldOfView = Mathf.Clamp(target > cam.fieldOfView ? cam.fieldOfView + (Time.fixedDeltaTime) : cam.fieldOfView - Time.fixedDeltaTime, 60f, 70f);
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy FİXED E GİRDİM");
        cam?.transform.DOKill();
    }
}

//using UnityEngine;

//public class CameraController : MonoBehaviour
//{
//    [SerializeField] private Camera cam;
//    [SerializeField] private Vector3 Offset;
//    private Vector3 velocity = Vector3.zero;

//    private void FixedUpdate()
//    {
//        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude / 3f), 60f, 70f),ref velocity.x,.5f);
//        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Ball.Instance.transform.position.x, transform.position.y, transform.position.z) + Offset, ref velocity, .55f);
//    }

//    //void LateUpdate()
//    //{
//    //    //Debug.Log("Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude), 60f, 70f): " + Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude/3), 60f, 70f));

//    //    //cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, Mathf.Clamp((60f + Ball.Instance.Rb.velocity.magnitude), 60f, 70f), ref velocity.x, 1f);
//    //    //transform.position =  Vector3.SmoothDamp(transform.position, new Vector3(Ball.Instance.transform.position.x, transform.position.y, transform.position.z) + Offset, ref velocity, .55f);
//    //}

//    ////private void LerpFieldOfView()
//    ////{

//    ////    // Distance moved equals elapsed time times speed..
//    ////    float distCovered = (Time.time - startTime) * speed;

//    ////    // Fraction of journey completed equals current distance divided by total distance.
//    ////    float fractionOfJourney = distCovered / journeyLength;

//    ////    // Set our position as a fraction of the distance between the markers.
//    ////    transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
//    ////}
//}
