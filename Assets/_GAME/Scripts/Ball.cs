using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class Ball : Singleton<Ball>
{
    // Start is called before the first frame update
    void Start()
    {
        //rigidBody.AddForce(Vector3.right*3f, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Obstacle")
        {
            Time.timeScale = 0f;
            GameController.Instance.FailLevel();
        }
    }
}
