#pragma warning disable CS0108

using UnityEngine;
using Unity.Cinemachine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class TankController : MonoBehaviour
{
    private float v;    // UP, Down
    private float h;    // Left , Right

    public float moveSpeed = 10.0f;  //이동 스피드
    public float turnSpeed = 100.0f; //회전 스피드

    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private TMP_Text nickNameText;

    private AudioSource audio;
    private CinemachineImpulseSource impulseSource;
    private PhotonView pv;
    private CinemachineCamera cc;

    private float initHp = 100.0f;
    private float currHp = 100.0f;

    // 탱크 하위에 있는 모든 랜더러 컴포넌트를 저장
    private Renderer[] renderers;
    private Canvas uiCanvas;

    private Image hpBar;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        pv = GetComponent<PhotonView>();
        cc = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();

        renderers = GetComponentsInChildren<Renderer>();
        uiCanvas = transform.Find("Canvas").GetComponent<Canvas>();
        hpBar = transform.Find("Canvas/Panel/Image - Hpbar").GetComponent<Image>();

        if (pv.IsMine == true)
        {
            cc.Target.TrackingTarget = transform;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        nickNameText.text = pv.Owner.NickName;
    }

    void Update()
    {
        if (pv.IsMine == false) return;

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
            pv.RPC("Fire", RpcTarget.AllViaServer, pv.Owner.ActorNumber);
        }
    }

    [PunRPC]
    private void Fire(int actorNumber)
    {
        // 동적으로 특정 프리팹을 생성
        var cannon = Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        cannon.GetComponent<Cannon>().actorNumber = actorNumber;
        // 사운드 발생
        audio.PlayOneShot(fireSfx, 0.8f);
        // 진동 발생
        impulseSource.GenerateImpulse();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("CANNON"))
        {
            currHp -= 20.0f;
            hpBar.fillAmount = currHp / initHp;

            int actorNumber = coll.gameObject.GetComponent<Cannon>().actorNumber;
            // ActorNumber -> NickName
            Player shooter = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);

            if (currHp <= 0.0f)
            {
                // 자신닉네임 님은 상대편에 피격당했습니다.
                string msg = $"<color=#00ff00>{pv.Owner.NickName}</color>님은 <color=#ff0000>{shooter.NickName}</color>에게 피격당했습니다.";
                GameManager.instance.DisplayMessage(msg);
                TankDestroy();
            }
        }
    }

    void TankDestroy()
    {
        SetVisibleTank(false);

        //Invoke("RespawnTank", 3.0f);
        Invoke(nameof(RespawnTank), 3.0f);
    }

    void RespawnTank()
    {
        // 각종 변수 초기화
        currHp = initHp;
        hpBar.fillAmount = 1.0f;

        // 탱크를 다시 활성화
        SetVisibleTank(true);
    }

    void SetVisibleTank(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }

        uiCanvas.enabled = isVisible;
    }

}

/*
    Vector3.forward = Vector3(0, 0, 1)
    Vector3.up      = Vector3(0, 1, 0)
    Vector3.right   = Vector3(1, 0, 0)

    Vector3.zero = Vector3(0, 0, 0)
    Vector3.one  = Vector3(1, 1, 1)
*/


/*
    RPC (Remote Procedure Call)



*/