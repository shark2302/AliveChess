using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Temp : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Yahoo");
    }
}
