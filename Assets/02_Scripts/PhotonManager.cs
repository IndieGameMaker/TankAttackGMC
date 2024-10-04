using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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

    #endregion
}
