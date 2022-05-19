using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Brocoli : SpawnObject {

    public float chasetime;
    float direccion;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        direccion = transform.localScale.x;
        if(Photon.Pun.PhotonNetwork.IsMasterClient)
            GetComponent<PhotonView>().RPC("RPC_dir", RpcTarget.AllBuffered, dir);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        

    }

    [PunRPC]
    public void RPC_dir(Vector3 dire)
    {
        StartCoroutine(predir(dire));
    }

    public IEnumerator predir(Vector3 dire)
    {
        yield return new WaitUntil(() => direccion != 0);

        if (dire.x >= 0) transform.localScale = new Vector2(direccion, transform.localScale.y);
        else transform.localScale = new Vector2(-direccion, transform.localScale.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bob") && Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);

        }
    }
}
