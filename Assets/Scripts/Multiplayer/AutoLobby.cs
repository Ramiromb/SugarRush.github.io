using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class AutoLobby : MonoBehaviourPunCallbacks
{

    public Button ConnectButton;
    public Button JoinRandomButton;
    public Text Log;
    public Text PlayerCount;
    public Button play;
    public int playersCount;
    public Button disconectButton;

    public string sceneName;


    public byte maxPlayersPerRoom = 4;
    public byte minPlayerPerRoom = 2;

    private bool isLoading = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            if (PhotonNetwork.ConnectUsingSettings())
            {
                Log.text += "\nConnected to Server";
            }
            else
            {
                Log.text += "\nFalling Connecting to Server";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        ConnectButton.interactable = false;
        JoinRandomButton.interactable = true;
        
    }

    public void JoinRandom()
    {
        if (PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\nJoinned Room";
            disconectButton.interactable = true;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        Log.text += "\nNo Rooms to Join, creating one...";

        if(PhotonNetwork.CreateRoom(null,new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayersPerRoom }))
        {
            Log.text += "\nRoom Created";
        }
        else
        {
            Log.text += "\nFail Creating Room";

        }
    }

    public override void OnJoinedRoom()
    {
        Log.text += "\nJoined";
        JoinRandomButton.interactable = false;
        //disconectButton.interactable = false;

    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        PlayerCount.text = playersCount + "/" + maxPlayersPerRoom;

        if (!isLoading && playersCount >= minPlayerPerRoom && PhotonNetwork.IsMasterClient)
        {
            play.gameObject.SetActive(true);
        }

    }

    public void ButtonPlay()
    {
        play.gameObject.SetActive(false);

        LoadMap();

    }

    private void LoadMap()
    {
        ObjectManager.cantplayers = playersCount;
        isLoading = true;
        PhotonNetwork.LoadLevel(sceneName);

    }

    public void Disconect()
    {
        PhotonNetwork.Disconnect();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Portada");
    }
}
