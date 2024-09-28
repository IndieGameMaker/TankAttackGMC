using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float force = 1500.0f;
    [SerializeField] private GameObject expEffect;

    private Rigidbody rb;

    void Awake()
    {
        // Resources 폴더에 있는 Asset을 로드
        expEffect = Resources.Load<GameObject>("BigExplosion");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * force);

        Destroy(this.gameObject, 5.0f);
    }

    void OnCollisionEnter(Collision coll)
    {
        Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
