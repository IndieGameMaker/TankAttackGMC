using UnityEngine;
using Photon.Pun;

public class CannonController : MonoBehaviour
{
    private float r;
    [SerializeField] private float speed = 200.0f;

    void Start()
    {
        this.enabled = transform.root.GetComponent<PhotonView>().IsMine;
    }

    void Update()
    {
        r = Input.GetAxis("Mouse ScrollWheel");
        transform.Rotate(Vector3.right * r * Time.deltaTime * speed);
    }
}
