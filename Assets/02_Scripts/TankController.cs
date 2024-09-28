#pragma warning disable CS0108

using UnityEngine;
using Unity.Cinemachine;

public class TankController : MonoBehaviour
{
    private float v;    // UP, Down
    private float h;    // Left , Right

    public float moveSpeed = 10.0f;  //이동 스피드
    public float turnSpeed = 100.0f; //회전 스피드

    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireSfx;

    private AudioSource audio;
    private CinemachineImpulseSource impulseSource;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        // 키입력값 받아오기
        v = Input.GetAxis("Vertical");   // -1.0f ~ 0.0f ~ +1.0f
        h = Input.GetAxis("Horizontal"); // -1.0f ~ 0.0f ~ +1.0f

        // 이동로직
        transform.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        // 회전로직
        transform.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);

        // Cannon 발사로직
        if (Input.GetMouseButtonDown(0) == true)
        {
            Fire();
        }
    }

    private void Fire()
    {
        // 동적으로 특정 프리팹을 생성
        Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        // 사운드 발생
        audio.PlayOneShot(fireSfx, 0.8f);
    }

}

/*
    Vector3.forward = Vector3(0, 0, 1)
    Vector3.up      = Vector3(0, 1, 0)
    Vector3.right   = Vector3(1, 0, 0)

    Vector3.zero = Vector3(0, 0, 0)
    Vector3.one  = Vector3(1, 1, 1)
*/