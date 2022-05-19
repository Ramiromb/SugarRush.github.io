using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : Photon.Pun.MonoBehaviourPun
//  ,  IPunObservable
{


    public float speed;
    public Vector3 dir;
    public float death;

    protected PhotonView pv;

    float timer= 0;


    // Use this for initialization
    virtual protected void Start () {
        //pv = GetComponent<PhotonView>();
        //pv.ObservedComponents.Add(this);
        timer = 0;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
        {
            transform.position += dir * speed * Time.deltaTime;

            if (timer <= death)
                timer += Time.deltaTime;
            else
                PhotonNetwork.Destroy(this.gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bob") && Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);

        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsReading)
    //    {
    //        Vector3 p = this.transform.position;
    //        Vector3 r = this.transform.rotation.eulerAngles;
    //        stream.SendNext(p);
    //        stream.SendNext(r);
    //
    //    }
    //    else
    //    {
    //        Vector3 p = (Vector3)stream.ReceiveNext();
    //        Vector3 r = (Vector3)stream.ReceiveNext();
    //        this.transform.position = p;
    //        this.transform.rotation = Quaternion.Euler(r);
    //    }
    //}
}
