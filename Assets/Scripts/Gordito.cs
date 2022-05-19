using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gordito : SpawnObject
{
    Animator animator;
    AudioSource auso;
    float direccion;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        direccion = transform.localScale.x;
        auso = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        switch (dir.ToString())
        {
            case "(-1.0, 0.0, 0.0)":
                transform.localScale = new Vector2(-direccion, transform.localScale.y);
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                break;
            case "(1.0, 0.0, 0.0)":
                transform.localScale = new Vector2(direccion, transform.localScale.y);
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                break;
            case "(0.0, -1.0, 0.0)":
                animator.SetBool("Down", true);
                animator.SetBool("Up", false);
                animator.SetBool("Side", false);
                break;
            case "(0.0, 1.0, 0.0)":
                animator.SetBool("Up", true);
                animator.SetBool("Side", false);
                animator.SetBool("Down", false);
                break;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bob")) auso.Play();
    }
}
