using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button sendButton;

    [SerializeField] private TMP_Text connectInfoText;
    [SerializeField] private TMP_Text playerListText;

    [SerializeField] private TMP_Text msgListText;
    [SerializeField] private TMP_InputField chatMsgIF;

    private PhotonView pv;

    void Awake()
    {
        // 싱글턴 변수에 할당
        instance = this;

        pv = GetComponent<PhotonView>();
    }

    IEnumerator Start()
    {
        // 버튼 이벤트 연결
        sendButton.onClick.AddListener(() => OnSendButtonClick());
        exitButton.onClick.AddListener(() => OnExitButtonClick());

        yield return new WaitForSeconds(0.5f);

        CreateTank();

        DisplayConnectInfo();
        DisplayPlayerList();
    }

    private void CreateTank()
    {
        // 탱크 생성할 좌표 
        Vector3 pos = new Vector3(Random.Range(-20.0f, 20.0f),
                                  10.0f,
                                  Random.Range(-20.0f, 20.0f));

        // 네트워크 탱크 생성
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }

    // 송수신할 RPC 함수 => 채팅 목록에 문자열을 추가
    [PunRPC]
    public void DisplayMessage(string msg)
    {
        msgListText.text += msg + "\n";
    }

    public void OnSendButtonClick()
    {
        // <color=#00ff00>[닉네임]</color> 메시지
        string msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {chatMsgIF.text}";
        pv.RPC(nameof(DisplayMessage), RpcTarget.AllBufferedViaServer, msg);
    }

    private void OnExitButtonClick()
    {
        // 방을 나가겠다는 요청 (로비 복귀 요청)
        PhotonNetwork.LeaveRoom();
    }

    // 현재 접속자 정보를 출력
    private void DisplayConnectInfo()
    {
        int currPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        string roomName = PhotonNetwork.CurrentRoom.Name;

        // [MyRoom] (2/20)
        string str = $"[{roomName}] ({currPlayer}/{maxPlayer})";

        connectInfoText.text = str;
    }

    // 현재 접속자 명 출력
    private void DisplayPlayerList()
    {
        string playerList = "";

        foreach (var player in PhotonNetwork.PlayerList)
        {
            // 방장일 경우 Red 변경
            string _color = player.IsMasterClient ? "#ff0000" : "#00ff00";

            playerList += $"<color={_color}>{player.NickName}</color>\n";
        }

        playerListText.text = playerList;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayConnectInfo();
        DisplayPlayerList();

        string msg = $"<color=#00ff00>[{newPlayer.NickName}]</color> is joined room.";
        DisplayMessage(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayConnectInfo();
        DisplayPlayerList();
        string msg = $"<color=#ff0000>[{otherPlayer.NickName}]</color> was left room.";
        DisplayMessage(msg);
    }
}
