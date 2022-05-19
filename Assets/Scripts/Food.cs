using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Food : SpawnObject {

    public int value;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bob") && Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);

        }
    }
}
