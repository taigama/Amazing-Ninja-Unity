using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRanger : MonoBehaviour {

    private Enemy host;

	void Start () {
        host = transform.parent.GetComponent<Enemy>();
	}
	
	void OnTriggerEnter2D(Collider2D col)
    {
        // Nếu là kẻ địch thì chém nó
        if (col.tag == "Player")
        {
            host.target = col.GetComponent<PlayerController>();
            host.Attack();
        }
    }
}
