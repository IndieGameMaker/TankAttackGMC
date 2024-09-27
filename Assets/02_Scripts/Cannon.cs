using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float force = 1500.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * force);

        Destroy(this.gameObject, 5.0f);
    }

    void OnCollisionEnter(Collision coll)
    {
        Destroy(this.gameObject);
    }
}
