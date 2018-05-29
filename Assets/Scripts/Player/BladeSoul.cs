using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladeSoul : MonoBehaviour {
    

    public float timeStrike;
    private float timer;

    public PlayerController host;

    void Start()
    {

        host = transform.parent.GetComponent<PlayerController>();
    }
	
    public void StartStrike()
    {
        timer = timeStrike;
        gameObject.SetActive(true);
    }

	void Update ()
    {
		if(timer <= 0)
        {
            gameObject.SetActive(false);

            return;
        }
        timer -= Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        // chém trúng đồng đội
        if (col.tag == "ally")
        {
            col.GetComponent<Blue>().TakeDmg();
            host.KillAlly();
        }

        // chém trúng kẻ địch
        if (col.tag == "enemy")
        {
            col.GetComponent<Enemy>().TakeDmg();
        }
    }
}
