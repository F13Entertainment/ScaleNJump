using UnityEngine;

public class RampPrefab : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private MeshCollider meshCollider;

    private bool isLastPrefab = false;
    public bool IsLastPrefab
    {
        get => isLastPrefab;
        set => isLastPrefab = value;
    }
    private Mesh bakedMesh;

    // Start is called before the first frame update
    void Awake()
    {
        bakedMesh = new Mesh();
    }

    public void Bake(SkinnedMeshRenderer skinnedMeshRenderer, int blendShapeCount)
    {
        for (int i = 0; i < blendShapeCount; i++)
            this.skinnedMeshRenderer.SetBlendShapeWeight(i, skinnedMeshRenderer.GetBlendShapeWeight(i));

        this.skinnedMeshRenderer.BakeMesh(bakedMesh);
        meshCollider.sharedMesh = bakedMesh;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            var velo = collision.gameObject.GetComponent<Rigidbody>().velocity.normalized;
            Vector3 temp = new Vector3(Mathf.Abs(velo.x), velo.y, 0f);
            collision.gameObject.GetComponent<Rigidbody>().velocity = temp * 12f;// .AddForce(collision.gameObject.GetComponent<Rigidbody>().velocity * 3f, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            var velo = collision.gameObject.GetComponent<Rigidbody>().velocity.normalized;
            Vector3 temp = new Vector3(Mathf.Abs(velo.x), velo.y, 0f);
            collision.gameObject.GetComponent<Rigidbody>().velocity = temp * 12f;// .AddForce(collision.gameObject.GetComponent<Rigidbody>().velocity * 3f, ForceMode.VelocityChange);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Ramp" && IsLastPrefab)
        {
            Destroy(other.gameObject);
        }
    }
}
