using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float force = 1500.0f;
    [SerializeField] private GameObject expEffect;

    // 발사한 유저의 ID
    public int actorNumber;

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
        var cannon = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(cannon, 5.0f);

        Destroy(this.gameObject);
    }
}
