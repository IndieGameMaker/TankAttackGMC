using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // a1b2a733-b17c-4d01-91d9-54adfce2e00b

    // 게임 버전 v1.0, v1.1, ...
    [SerializeField] private string version = "1.0";

    // 유저명 (닉네임)
    private string nickName = "Zack";

    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIF;
    [SerializeField] private TMP_InputField roomNameIF;

    [Header("Button")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button makeRoomButton;

    [Header("Room List")]
    // 룸 아이템 프리팹
    [SerializeField] private GameObject roomPrefab;
    // 룸 아이템을 생성할 부모객체
    [SerializeField] private Transform contentTr;
    // 룸 목록을 저장할 딕셔너리 데이터를 선언
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();


    private void Awake()
    {
        // 게임 버전 설정
        PhotonNetwork.GameVersion = version;
        // 유저명 설정
        PhotonNetwork.NickName = nickName;
        // 방장이 씬을 로딩하면 자동으로 로딩하는 기능
        PhotonNetwork.AutomaticallySyncScene = true;

        // 포톤 서버(포톤 클라우드)에 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        // 저장된 닉네임을 로드
        nickName = PlayerPrefs.GetString("NICK_NAME", $"USER_{Random.Range(0, 1001):0000}");
        nickNameIF.text = nickName;

        // 버튼 클릭 이벤트 연결 => goes to
        loginButton.onClick.AddListener(() => OnLoginButtonClick());
        makeRoomButton.onClick.AddListener(() => OnMakeRoomButtonClick());
    }

    public void OnLoginButtonClick()
    {
        SetNickName();
        // 닉네임을 저장
        PlayerPrefs.SetString("NICK_NAME", nickName);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomButtonClick()
    {
        SetNickName();
        // 룸 이름 입력 여부를 확인
        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            roomNameIF.text = $"ROOM_{Random.Range(0, 10000):00000}";
        }

        // 룸 속성을 정의
        //RoomOptions ro = new RoomOptions();
        RoomOptions ro = new();
        ro.MaxPlayers = 20;
        ro.IsOpen = true;
        ro.IsVisible = true;

        // 룸 생성
        PhotonNetwork.CreateRoom(roomNameIF.text, ro);
    }

    private void SetNickName()
    {
        // 닉네임이 비여있는지 확인
        if (string.IsNullOrEmpty(nickNameIF.text))
        {
            // 닉네임을 무작위로 설정
            nickName = $"USER_{Random.Range(0, 1001):0000}";
            nickNameIF.text = nickName;
        }

        // 닉네임이 입력되었을 경우
        nickName = nickNameIF.text;
        // 포톤 닉네임 설정
        PhotonNetwork.NickName = nickName;
    }

    #region 포톤_콜백_메소드

    // 포톤 서버에 접속했을 때 호출되는 콜백 메소드
    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 서버 접속 성공");

        // 로비에 입장 요청
        PhotonNetwork.JoinLobby();
    }

    // 포톤 로비에 입장했을 때 호출되는 콜백 메소드
    public override void OnJoinedLobby()
    {
        Debug.Log("포톤 로비에 입장 완료");

        // 랜덤한 룸에 입장 요청
        // PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤한 룸에 접속을 실패했을 때 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 접속 실패 : {returnCode}, 메시지 : {message}");

        // 룸 생성 로직
        // 룸 속성을 정의
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };
        // 룸 생성
        PhotonNetwork.CreateRoom("MyRoom", ro);
    }

    // 룸생성 실패시 호출되는 콜백
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    // 룸 생성 완료 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("룸 생성 완료");
    }

    // 룸에 입장했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료");
        // 네트워크 탱크 생성
        // PhotonNetwork.Instantiate("Tank", new Vector3(0, 10.0f, 0), Quaternion.identity, 0);

        // 방장만 씬을 호출할 수 있음.
        if (PhotonNetwork.IsMasterClient)
        {
            // SceneManager.LoadScene
            PhotonNetwork.LoadLevel("BattleField");
        }
    }

    // 룸 목록이 변경될 때마다 호출되는 콜백 메서드
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            Debug.Log($"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})");

            // 삭제된 룸 여부를 확인
            if (room.RemovedFromList == true)
            {
                // 룸 삭제 로직
                if (roomDict.TryGetValue(room.Name, out var tempRoom))
                {
                    // 룸 프리팹 삭제
                    Destroy(tempRoom);
                    // 딕셔너리에서 해당 룸 정보를 삭제
                    roomDict.Remove(room.Name);
                }
                continue;
            }

            // 새로 생성된 룸, 변경된 부분을 처리
            // 처음 생성된 룸 (딕셔너리에서 검색을 했을 때 결괏값이 없는 경우)
            if (roomDict.ContainsKey(room.Name) == false)
            {
                // 처음 생성된 룸인 경우
                var _room = Instantiate(roomPrefab, contentTr);
                _room.name = $"ROOM_{room.Name}";
                // 룸 프리팹에 추가한 RoomData에 데이터 저장
                _room.GetComponent<RoomData>().RoomInfo = room;

                // 딕셔너리에 저장
                roomDict.Add(room.Name, _room);
            }
            else
            {
                // 이전에 생성된 룸 (룸의 변경사항 존재)
                // 딕셔너리에 검색 후 룸 정보를 추출한 후 정보를 변경
                if (roomDict.TryGetValue(room.Name, out GameObject tempRoom))
                {
                    tempRoom.GetComponent<RoomData>().RoomInfo = room; // 갱신된 룸 정보
                }
            }
        }
    }

    #endregion
}
