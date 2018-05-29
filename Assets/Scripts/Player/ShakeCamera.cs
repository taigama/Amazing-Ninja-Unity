using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour {

    static bool isShaked;

    public float distance;
    public float time;

    static float trueTimeLimit;
    private static float timer;

    private static AudioListener sound;

    public float speed;
    private Vector3 vecMove;

	void Start ()
	{
        vecMove = new Vector3(speed, 0f);

        isShaked = false;
        trueTimeLimit = time;

        sound = GetComponent<AudioListener>();
    }

    private float currentX;

    void Update()
    {
        if (isShaked)
        {
            transform.position = new Vector3(                  
                Random.Range(-distance + currentX, distance + currentX),                   
                Random.Range(-distance, distance), -10.0f);


            timer -= Time.deltaTime;
            if (timer < 0)
                isShaked = false;
        }

        if(GameController.IsRun())
        {
            transform.position += vecMove * Time.deltaTime;
            currentX = transform.position.x;
        }
    }

    public static void Shake()
    {
        isShaked = true;
        timer = trueTimeLimit;
    }

    public static AudioListener getSound()
    {
        return sound;
    }

    public void Reset()
    {
        transform.position = new Vector3(0f, 0f, -10f);
    }
}
