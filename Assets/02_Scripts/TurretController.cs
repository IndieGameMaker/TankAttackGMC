using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 10.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 메인 카메라에서 발사되는 Ray(광선)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

        // 레이케스팅
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, 1 << 8))
        {
            // 터렛 기준으로 월드좌표계를 로컬좌표계로 변환
            Vector3 pos = transform.InverseTransformPoint(hit.point);

            // 두 좌표간의 각도(사잇각) Atan2 , Atan(pos.x/pos.z)
            float angle = Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg;

            // 터렛을 회전
            transform.Rotate(Vector3.up * angle * Time.deltaTime * turnSpeed);
        }
    }
}
