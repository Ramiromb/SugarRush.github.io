using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour {

    public Transform camTransform;


    public float shakeDuration = 0f;

    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    AudioSource auso;

    void Awake()
    {
        auso = GetComponent<AudioSource>();

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
        shakeDuration = 0;
    }

    // Use this for initialization
    void Start () {
        originalPos = camTransform.localPosition;

    }

    // Update is called once per frame
    void Update () {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void Shake()
    {
        shakeDuration = 1;
        auso.Play();
    }
}
