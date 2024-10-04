using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameManager : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        CreateTank();
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
}
