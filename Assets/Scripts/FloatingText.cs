using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    public float lifetime;
    public float speed;
    public Color color;
    public string numero;
    public TextMesh tm;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifetime);
        tm = GetComponent<TextMesh>();
        tm.color = color;
        tm.text = numero;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += Vector3.up * speed * Time.deltaTime;
        this.transform.localScale *= 1.007f;
	}
}
