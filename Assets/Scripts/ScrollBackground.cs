using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {

    public float speed;

    //private float fTemp = 0f;
    Vector2 offset = Vector2.zero;

    private Renderer render;
    private float timer = 0f;

	void OnEnable()
    {
        render = GetComponent<Renderer>();
        render.sharedMaterial.SetTextureOffset("_MainTex", Vector2.zero);
    }
	
	void Update ()
    {
		if(GameController.IsRun())
        {
            timer += Time.deltaTime;

            offset.x = Mathf.Repeat(timer * speed, 1);
            render.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }

        
	}
}
