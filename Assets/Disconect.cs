using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Disconect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
