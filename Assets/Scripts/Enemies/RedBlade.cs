using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedBlade : MonoBehaviour {

    public float timeStrike;
    private float timer;

    public int direct;
	
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
        // chém trúng người chơi
        if(col.tag == "Player")
        {
            col.GetComponent<PlayerController>().TakeDmg(direct);
            GameController.Stop();
        }
    }
}
