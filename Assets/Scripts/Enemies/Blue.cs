using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : MonoBehaviour {
    
    
    public Animator animator;
    public Collider2D col;
    public void OnEnable()
    {
        animator.Play("blue_run");
        col.enabled = true;
    }

    public void TakeDmg()
    {
        animator.Play("blue_dead");

        // tránh trường hợp player đụng thằng này xong nằm trên không khí
        col.enabled = false;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SoundHit();
        GameController.Stop();
    }
}
