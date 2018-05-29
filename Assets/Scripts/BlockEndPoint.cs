using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEndPoint : MonoBehaviour {

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "box")
        {
            //Debug.Log("end point reach!");
            transform.parent.gameObject.SetActive(false);
        }
    }
}
