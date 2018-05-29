using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public float speed;
    
    private Vector3 vecMoveLeft;
    private Transform myTransform;

    void Start()
    {
        vecMoveLeft = new Vector3(speed, 0.0f);
        myTransform = this.transform;
    }

	void Update ()
    {
        if(GameController.IsRun())
            myTransform.position -= vecMoveLeft * Time.deltaTime;
	}

    public void Reset()
    {
        gameObject.SetActive(true);

        foreach(Transform child in myTransform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
