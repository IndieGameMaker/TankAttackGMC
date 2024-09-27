using UnityEngine;

public class CannonController : MonoBehaviour
{
    private float r;
    [SerializeField] private float speed = 200.0f;

    void Update()
    {
        r = Input.GetAxis("Mouse ScrollWheel");
        transform.Rotate(Vector3.right * r * Time.deltaTime * speed);
    }
}
