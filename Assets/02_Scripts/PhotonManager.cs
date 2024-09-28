using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 게임 버전 v1.0, v1.1, ...
    [SerializeField] private string version = "1.0";

    // 유저명 (닉네임)
    [SerializeField] private string nickName = "Zack";

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

    #region 포톤_콜백_메소드

    // 포톤 서버에 접속했을 때 호출되는 콜백 메소드
    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 서버 접속 성공");
    }

    #endregion
}
