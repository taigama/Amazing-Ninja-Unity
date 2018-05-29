using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBody : MonoBehaviour {

    public RedBlade bladeFront;
    public RedBlade bladeBack;

    private Enemy host;

	void Start ()
    {
        host = transform.parent.GetComponent<Enemy>();
	}

    public void StrikeFront()
    {
        bladeFront.StartStrike();
    }

    public void StrikeBack()
    {
        bladeBack.StartStrike();
    }

    public void CheckTarget()
    {
        host.AttackAgain();
    }

    public void Faded()
    {
        host.Faded();
    }
}
