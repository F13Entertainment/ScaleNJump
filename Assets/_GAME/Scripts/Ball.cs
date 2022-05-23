using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class Ball : Singleton<Ball>
{
    [SerializeField]private Rigidbody rb;
    public Rigidbody Rb => rb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Obstacle")
            GameController.Instance.FailLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diamond")
            Destroy(other.gameObject);
    }
}
