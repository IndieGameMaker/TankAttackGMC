using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    // 룸 정보를 저장할 멤버변수
    private RoomInfo roomInfo;

    // 프로퍼티 선언 getter/setter
    public RoomInfo RoomInfo
    {
        // 읽기 전용
        get
        {
            return roomInfo;
        }
        // 쓰기 전용
        set
        {
            roomInfo = value;
            roomText.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
            // 버튼 클릭 이벤트 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(roomInfo.Name);
            });
        }
    }

    void Awake()
    {
        var photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();


        // 자신의 Child에 있는 Text를 추출
        roomText = GetComponentInChildren<TMP_Text>();
    }
}
