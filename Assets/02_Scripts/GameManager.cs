using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text connectInfoText;

    IEnumerator Start()
    {
        // 버튼 이벤트 연결
        exitButton.onClick.AddListener(() => OnExitButtonClick());

        yield return new WaitForSeconds(0.5f);

        CreateTank();

        DisplayConnectInfo();
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
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayConnectInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayConnectInfo();
    }
}
