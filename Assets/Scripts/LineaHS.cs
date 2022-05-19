using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineaHS : MonoBehaviour
{

    public Text score;
    public Text nickname;

    // Use this for initialization
    void Awake()
    {
        score = transform.Find("Score").gameObject.GetComponent<Text>();
        nickname = transform.Find("Nickname").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
