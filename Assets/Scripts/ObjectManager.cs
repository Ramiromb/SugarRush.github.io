using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;

public class ObjectManager : Photon.Pun.MonoBehaviourPun //MonoBehaviour
{
    public Text log;
    public Text wintex;
    public bool iswin;
    public bool readgame;
    public static int cantplayers;

    public static ObjectManager om;

    public List<Charact> chars = new List<Charact>();
    public int cantvivos;

    public Charact bob;
    public Transform pos;


    public List<Food> junkfood = new List<Food>();
    public List<Food> healthyfood = new List<Food>();
    public Food hambur;
    public Food pancho;
    public Food papas;
    public Food pizza;
    public Food badhambur;
    public Food badpancho;
    public Food badpapas;
    public Food badpizza;
    public SpawnObject gordo;
    public SpawnObject brocoli;
    public float xIzq;
    public float xDer;
    public float yAba;
    public float yArri;
    public float timerfood;
    public float timerenemy;
    public float spawnfoodtime;
    public float spawnenemytime;
    public float spawnfoodtimemin;
    public float spawnfoodtimemax;
    public float spawnenemytimemin;
    public float spawnenemytimemax;

    public bool cansapwn;
    public float spwntmr;
    public float spwntime;

    public bool stopspanwing;

    bool uno ;

    // Use this for initialization
    void Start ()
    {
        if (ObjectManager.om == null)
            ObjectManager.om = this;
        else
            if(ObjectManager.om != this)
        {
            Destroy(ObjectManager.om);
            ObjectManager.om = this;
        }


        stopspanwing = false;
        spwntmr = 0;
        GameObject b = Photon.Pun.PhotonNetwork.Instantiate(bob.name, pos.position, Quaternion.identity);
        b.GetComponent<Charact>().manger = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cantplayers == chars.Count)
            readgame = true;
        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
        {
            
            if (!stopspanwing && readgame)
            {
                if (cansapwn)
                {
                    Timelapse();
                    TimeEnemy();
                }
                if (cansapwn == false) SpawnTimer();
            }
            
        }
    }

    public void LastOne()
    {
        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
            if (chars.Where(x => x.canplay).Count() == 1 && chars.Count() > 1)
            {
                chars.Where(X => X.canplay).First().WIN();
                stopspanwing = true;
            }
    }
    //public void Stopsall()
    //{
    //    stopspanwing = true;
    //    ElminateObjects();
    //}
    //
    //public void Respawning()
    //{
    //    cansapwn = false;
    //    ElminateObjects();
    //}

    public void SpawnTimer()
    {
        spwntmr += Time.deltaTime;
        if (spwntmr >= spwntime)
        {
            spwntmr = 0;
            cansapwn = true;
        }
    }

    public void Timelapse()
    {
        timerfood += Time.deltaTime;
        if(timerfood >= spawnfoodtime)
        {
            timerfood = 0;
            spawnfoodtime = Random.Range(spawnfoodtimemin, spawnfoodtimemax);

            if (Random.Range(0, 5) < 3) wheretospawn(whatfood(true));
            else wheretospawn(whatfood(false));
        }
    }

    public void TimeEnemy()
    {
        timerenemy += Time.deltaTime;
        if(timerenemy >= spawnenemytime)
        {
            timerenemy = 0;
            spawnenemytime = Random.Range(spawnenemytimemin, spawnenemytimemax);

            if (Random.Range(0, 10) >= 4) wheretospawn(brocoli);
            else wheretospawn(gordo);
        }
    }

    public Food whatfood(bool good)
    {
        int ran = Random.Range(0, 100);
        if (good)
        {
            if (ran < 40) return papas;
            else if (ran < 70) return pancho;
            else if (ran < 90) return pizza;
            else return hambur;
        }
        else
        {
            if (ran < 40) return badpapas;
            else if (ran < 70) return badpancho;
            else if (ran < 90) return badpizza;
            else return badhambur;
        }
    }

    public void wheretospawn(SpawnObject prefod)
    {
        Vector3 dir = Vector3.zero;
        Vector3 pos = Vector3.zero;
        switch (Random.Range(0, 4))
        {
            case 0:
                dir = Vector3.right;
                pos = new Vector3(xIzq, Random.Range(yAba, yArri), -4);
                break;
            case 1:
                dir = -Vector3.right;
                pos = new Vector3(xDer, Random.Range(yAba, yArri), -4);
                break;
            case 2:
                dir = Vector3.up;
                pos = new Vector3(Random.Range(xIzq, xDer), yAba, -4);
                break;
            case 3:
                dir = -Vector3.up;
                pos = new Vector3(Random.Range(xIzq, xDer), yArri, -4);
                break;
        }
        spawn(dir, pos, prefod);
    }

    public void spawn(Vector3 dir, Vector3 pos, SpawnObject prefod)
    {
        GameObject c = Photon.Pun.PhotonNetwork.Instantiate(prefod.name, pos, Quaternion.identity);
        SpawnObject p = c.GetComponent<SpawnObject>();
        p.dir = dir;
        p.transform.SetParent(this.transform);
    }

    [PunRPC]
    public void RPC_ElminateObject(SpawnObject obj)
    {
        Destroy(obj);
    }
}
