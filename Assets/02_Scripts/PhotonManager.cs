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
    }
}
