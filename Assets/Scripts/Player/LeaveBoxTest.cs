using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveBoxTest : MonoBehaviour {

    //public float speedMove;

    //void Start()
    //{
    //    GetComponent<Rigidbody2D>().velocity = new Vector2(speedMove, 0f);
    //}


	void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            GameController.Stop();
        }
        col.gameObject.SetActive(false);
    }
}
