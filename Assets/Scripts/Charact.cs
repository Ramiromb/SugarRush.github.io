using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Linq;

public class Charact : Photon.Pun.MonoBehaviourPun //MonoBehaviour
{

    public int playnum;
    //public Text eat;
    public float eattextspeed;

    public List<Color> colores;

    //public CamShake cam;
    //public HighScore hc;
    public ObjectManager manger;
    public GameObject fasesound;
    public FloatingText ftext;
    public Color colorbueno;
    public Color colormalo;
    public float masarribatext;

    public float speed;
    public float maxspeed;
    public float acceleration;
    public float basespeed;

    public float respawntime;

    public Vector3 inipoint;

    Rigidbody2D rb;
    public BoxCollider2D bobflaco;
    public BoxCollider2D bobnormal1;
    public BoxCollider2D bobnormal2;
    public BoxCollider2D bobgordo1;
    public BoxCollider2D bobgordo2;
    public BoxCollider2D bobobeso1;
    public BoxCollider2D bobobeso2;
    public BoxCollider2D bobobeso3;
    public Animator animator;
    Vector3 dir;
    float direccion;
    public bool moving;
    public float fat;
    public int lives;
    public float targetfat;
    public float changingtargetfat;
    public bool canmove;
    public bool canplay;
    public float pushedtime;
    public float pushforce;
    //public Text points;
    public int fase;
    //public Image barra;
    float fatarestar;


    public bool invul;
    //public Image currentbob1;
    //public Image currentbob2;
    //public Image currentbob3;
    //public Image currentbob4;
    //
    //public Image goalbob1;
    //public Image goalbob2;
    //public Image goalbob3;
    //public Image goalbob4;
    //
    //public InputField entername;
    //
    //public Image heart1;
    //public Image heart2;
    //public Image heart3;
    //
    //public Image currentheart;

    public float hmoveinx;
    public float hmoveiny;

    public float hmoveinxstar;
    public float hmoveinystar;

    public float heartspeed;

    public bool movingheart;

    //public GameObject hsui;

    AudioSource auso;
    public AudioClip munch;
    public AudioClip diuj;
    public AudioClip gover;

    public RuntimeAnimatorController normalanim;
    public RuntimeAnimatorController gordoanim;
    public RuntimeAnimatorController obesoanim;

    public float flashduration;
    public float flashtimer;
    public bool flashon;
    SpriteRenderer sr;

    public float stuntimer;
    public float stancechangetime;
    public int stance;

    public bool stuned;

    public GameObject flecha;
    // Use this for initialization
    void Start()
    {

        if (photonView.IsMine && !PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
        }

        ObjectManager.om.chars.Add(this);
        ObjectManager.om.cantvivos++;

        Cursor.visible = false;
        fatarestar = 0;
        lives = 3;
        canmove = true;
        direccion = transform.localScale.x;
        transform.localScale = new Vector3(3.469241f, 3.469241f, 3.469241f);
        rb = GetComponent<Rigidbody2D>();
        auso = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        changingtargetfat = targetfat * 1 / 4;
        //currentbob1.gameObject.SetActive(true);
        //goalbob1.gameObject.SetActive(true);
        inipoint = this.transform.position;
        sr = GetComponent<SpriteRenderer>();
        Respawn();

        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
            GetComponent<PhotonView>().RPC("RPC_Numero", RpcTarget.AllBuffered, Mathf.Clamp( ObjectManager.om.chars.Count()-1, 0, 3) );


    }

    public IEnumerator prenumero(int num)
    {
        yield return new WaitUntil(() => sr != null);
        playnum = num;
        sr.color = colores[num];
    }

    [PunRPC]
    public void RPC_Numero(int num)
    {
        StartCoroutine(prenumero(num));
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            flecha.SetActive(true);
            //eat.transform.position += Vector3.right * eattextspeed;

            if (canmove) Move();
        }
        //points.text = "Peso: " + fat + " Kg";

        //barra.fillAmount = (fat - fatarestar) / (changingtargetfat - fatarestar);

        //if (canplay == false) Flashing();

        //if (hsui.activeSelf)
        //{
        //    if (Input.GetKeyDown(KeyCode.Return) && entername.text.Length >= 3 && entername.text.Length < 17)
        //    {
        //        hc.AddHighscore((int)fat, entername.text);
        //        print(fat + " " + entername.text);
        //        SceneManager.LoadScene("HighScores");
        //    }
        //}

        //if (movingheart) GoneHeart(currentheart);
        if (stuned) Stuning();

    }


    public void Stuning()
    {
        canmove = false;
        stuntimer += Time.deltaTime;
        if (stuntimer >= stancechangetime)
        {
            stuntimer = 0;
            stance++;
            if (stance > 3) stance = 0;
        }
        switch (stance)
        {
            case 0:
                transform.localScale = new Vector2(-direccion, transform.localScale.y);
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                animator.SetBool("Idle", false);
                break;
            case 1:
                animator.SetBool("Up", true);
                animator.SetBool("Side", false);
                animator.SetBool("Down", false);
                animator.SetBool("Idle", false);
                break;
            case 2:
                transform.localScale = new Vector2(direccion, transform.localScale.y);
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                animator.SetBool("Idle", false);
                break;
            case 3:
                animator.SetBool("Down", true);
                animator.SetBool("Up", false);
                animator.SetBool("Side", false);
                animator.SetBool("Idle", false);
                break;
        }
    }

    public void Move()
    {

        moving = false;
        dir = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir -= Vector3.right;
            transform.localScale = new Vector2(-direccion, transform.localScale.y);
            moving = true;

            animator.SetBool("Side", true);
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Idle", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector3.right;
            transform.localScale = new Vector2(direccion, transform.localScale.y);
            moving = true;

            animator.SetBool("Side", true);
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Idle", false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector3.up;
            moving = true;

            animator.SetBool("Up", true);
            animator.SetBool("Side", false);
            animator.SetBool("Down", false);
            animator.SetBool("Idle", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir -= Vector3.up;
            moving = true;

            animator.SetBool("Down", true);
            animator.SetBool("Up", false);
            animator.SetBool("Side", false);
            animator.SetBool("Idle", false);
        }
        Speed();
    }

    public void Speed()
    {
        if (moving)
        {
            if (speed < maxspeed) speed += acceleration;
            rb.velocity = dir.normalized * speed;
        }
        else if (!moving)
        {
            speed = basespeed;
            animator.SetBool("Down", false);
            animator.SetBool("Up", false);
            animator.SetBool("Side", false);
            animator.SetBool("Idle", true);
            rb.velocity = Vector3.zero;
        }
    }

    public void Fase2()
    {
        bobflaco.enabled = false;
        bobnormal1.enabled = true;
        bobnormal2.enabled = true;
        direccion = 3.66495f;
        transform.localScale = new Vector3(3.66495f, 3.78012f, 3.78012f);
        GetComponent<Animator>().runtimeAnimatorController = normalanim;
        changingtargetfat += targetfat * 2 / 4;
        fase = 2;
        maxspeed--;
        fatarestar = targetfat / 4;
        //currentbob1.gameObject.SetActive(false);
        //goalbob1.gameObject.SetActive(false);
        //currentbob2.gameObject.SetActive(true);
        //goalbob2.gameObject.SetActive(true);
        fasesound.GetComponent<AudioSource>().Play();
    }

    public void Fase3()
    {
        bobnormal1.enabled = false;
        bobnormal2.enabled = false;
        bobgordo1.enabled = true;
        bobgordo2.enabled = true;
        direccion = 4.163589f;
        transform.localScale = new Vector3(4.163589f, 4.102744f, 3.922955f);
        GetComponent<Animator>().runtimeAnimatorController = gordoanim;
        changingtargetfat += targetfat * 3 / 4;
        fase = 3;
        maxspeed--;
        fatarestar = targetfat * 2 / 4;
        //currentbob2.gameObject.SetActive(false);
        //goalbob2.gameObject.SetActive(false);
        //currentbob3.gameObject.SetActive(true);
        //goalbob3.gameObject.SetActive(true);
        fasesound.GetComponent<AudioSource>().Play();

    }

    public void Fase4()
    {
        bobgordo1.enabled = false;
        bobgordo2.enabled = false;
        bobobeso1.enabled = true;
        bobobeso2.enabled = true;
        bobobeso3.enabled = true;
        direccion = 3.249516f;
        transform.localScale = new Vector3(3.249516f, 3.116286f, 3.249516f);
        GetComponent<Animator>().runtimeAnimatorController = obesoanim;
        changingtargetfat += targetfat;
        fase = 4;
        maxspeed--;
        fatarestar = targetfat * 3 / 4;
        //currentbob3.gameObject.SetActive(false);
        //goalbob3.gameObject.SetActive(false);
        //currentbob4.gameObject.SetActive(true);
        //goalbob4.gameObject.SetActive(true);
        fasesound.GetComponent<AudioSource>().Play();

    }

    //public void Flashing()
    //{
    //    sr.enabled = flashon;
    //    flashtimer += Time.deltaTime;
    //    if (flashtimer >= flashduration)
    //    {
    //        flashtimer = 0;
    //        flashon = !flashon;
    //    }
    //}

    public void Respawn()
    {
        this.transform.position = inipoint;
        //manger.Respawning();
        canmove = false;
        canplay = false;
        if (fat < 40) fat = 40;
        StartCoroutine(RespawnedReady());

    }

    public void GoneHeart(Image heart)
    {
        hmoveinx -= Time.deltaTime;
        hmoveiny += Time.deltaTime;
        heart.rectTransform.position += new Vector3(hmoveinx, hmoveiny, 0) * heartspeed;
        heart.rectTransform.localScale *= 1.02f;
    }

    public void LoseLive()
    {
        lives--;
        rb.velocity = Vector3.zero;
        if (lives == 2)
        {
            movingheart = true;
            //currentheart = heart3;
            //StartCoroutine(DestroyHeart(heart3));
            hmoveinx = hmoveinxstar;
            hmoveiny = hmoveinystar;
        }
        if (lives == 1)
        {
            movingheart = true;
            //currentheart = heart2;
            //StartCoroutine(DestroyHeart(heart2));
            hmoveinx = hmoveinxstar;
            hmoveiny = hmoveinystar;
        }
        if (lives == 0)
        {
            movingheart = true;
            //currentheart = heart1;
            //StartCoroutine(DestroyHeart(heart1));
            hmoveinx = hmoveinxstar;
            hmoveiny = hmoveinystar;
            Ded();
        }
        else
        {
            //cam.Shake();
            Respawn();
            stuned = true;
        }
    }

    public void Ded()
    {
        canmove = false;
        canplay = false;
        manger.GetComponent<AudioSource>().clip = gover;
        manger.GetComponent<AudioSource>().Play();
        //manger.Stopsall();
        animator.SetBool("Down", false);
        animator.SetBool("Up", false);
        animator.SetBool("Side", false);
        animator.SetBool("Idle", true);
        //if (fat >= hc.highscores.hclist[9].score)
        //{
        //    hsui.SetActive(true);
        //    entername.Select();
        //
        //}
        //else
        //{
        //    SceneManager.LoadScene("HighScores");
        //}
    }
    IEnumerator DestroyHeart(Image heary)
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(heary);
        movingheart = false;

    }
    IEnumerator CanMoveAgain()
    {
        yield return new WaitForSeconds(pushedtime);
        stuned = false;
        canmove = true;
    }

    IEnumerator RespawnedReady()
    {
        yield return new WaitForSeconds(respawntime);
        canmove = true;
        stuned = false;
        canplay = true;
        sr.enabled = true;
    }

    public void Empuje(Vector3 dir, Vector3 side)
    {
        rb.velocity = (dir / 2 + side) * pushforce;
    }

    [PunRPC]
    public void RPC_ChangeGrasa(int grasa)
    {
        if (grasa >= 0)
        {
            auso.clip = munch;
            auso.Play();
            fat += grasa;
            FloatingText ft = Instantiate(ftext);
            ft.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + masarribatext, -10);
            ft.color = colorbueno;
            ft.numero = "+" + grasa;
        }
        else
        {

            auso.clip = diuj;
            auso.Play();
            fat += grasa;
            FloatingText ft = Instantiate(ftext);
            ft.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + masarribatext, -10);
            ft.color = colormalo;
            ft.numero = "" + grasa;

        }
        if (fat >= changingtargetfat && fase == 1) Fase2();
        if (fat >= changingtargetfat && fase == 2) Fase3();
        if (fat >= changingtargetfat && fase == 3)
        {
            Fase4();
            if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
                GetComponent<PhotonView>().RPC("RPC_Win", RpcTarget.AllBuffered);
        }

        ObjectManager.om.log.text += "   +" + grasa+"     ";

    }

    [PunRPC]
    public void RPC_Die()
    {
        if (!invul)
        {
            if (canplay)
                ObjectManager.om.cantvivos--;
            canplay = false;
            bobflaco.enabled = false;
            bobnormal1.enabled = false;
            bobnormal2.enabled = false;
            bobgordo1.enabled = false;
            bobgordo2.enabled = false;
            bobobeso1.enabled = false;
            bobobeso2.enabled = false;
            bobobeso3.enabled = false;
            sr.enabled = false;
            ObjectManager.om.LastOne();  
            ObjectManager.om.log.text += "         hay vivos: " + ObjectManager.om.cantvivos;
            
        }
    }

    [PunRPC]
    public void RPC_Win()
    {
        canplay = false;
        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
            foreach (Charact c in ObjectManager.om.chars.Where(x => x != this))
            {
                c.GetComponent<PhotonView>().RPC("RPC_Die", RpcTarget.AllBuffered);
            }
        ObjectManager.om.log.text += "      wiiiiiiiiiinnnn        ";
        if (!photonView.IsMine)
            ObjectManager.om.iswin = true;
        invul = true;
        if (photonView.IsMine)
        {
            ObjectManager.om.wintex.text = "YOU WIN";
        }
        else
        {
            ObjectManager.om.wintex.text = "YOU LOSE" + "" + "\nPlayer " + (playnum + 1) + " WINS";
        }
        ObjectManager.om.wintex.gameObject.SetActive(true);
        StartCoroutine(WaitForLoad());
        
    }
    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Portada");
    }

    public void WIN()
    {

        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
            GetComponent<PhotonView>().RPC("RPC_Win", RpcTarget.AllBuffered);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canplay)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Food"))
            {
                if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
                    GetComponent<PhotonView>().RPC("RPC_ChangeGrasa", RpcTarget.AllBuffered, collision.gameObject.GetComponent<Food>().value);



                
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null && !invul)

                    GetComponent<PhotonView>().RPC("RPC_Die", RpcTarget.AllBuffered);
            }
            //if (collision.gameObject.layer == LayerMask.NameToLayer("Gordo"))
            //{
            //    stuned = true;
            //    StartCoroutine(CanMoveAgain());
            //    Vector3 dire = collision.gameObject.GetComponent<Gordito>().dir;
            //    if (dire == Vector3.up || dire == -Vector3.up)
            //    {
            //        if (this.transform.position.x < collision.transform.position.x) Empuje(dire, -Vector3.right);
            //        else Empuje(dire, Vector3.right);
            //    }
            //    else
            //    {
            //        if (this.transform.position.y < collision.transform.position.y) Empuje(dire, -Vector3.up);
            //        else Empuje(dire, Vector3.up);
            //    }
            //}
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}

    //public void Move()
    //{
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        rb.velocity = transform.right * speed;
    //        transform.localScale = new Vector2(dir, transform.localScale.y);

    //        animator.SetBool("Side", true);
    //        animator.SetBool("Up", false);
    //        animator.SetBool("Down", false);
    //        animator.SetBool("Idle", false);
    //    }

    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        rb.velocity = -transform.right * speed;
    //        transform.localScale = new Vector2(-dir, transform.localScale.y);

    //        animator.SetBool("Side", true);
    //        animator.SetBool("Up", false);
    //        animator.SetBool("Down", false);
    //        animator.SetBool("Idle", false);
    //    }

    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        rb.velocity = transform.up * speed;
    //        animator.SetBool("Up", true);
    //        animator.SetBool("Side", false);
    //        animator.SetBool("Down", false);
    //        animator.SetBool("Idle", false);
    //    }

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        rb.velocity = -transform.up * speed;
    //        animator.SetBool("Down", true);
    //        animator.SetBool("Up", false);
    //        animator.SetBool("Side", false);
    //        animator.SetBool("Idle", false);
    //    }

        

        
    

