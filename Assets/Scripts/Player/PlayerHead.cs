using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour {

    public BladeSoul bladeSoul;
    
    public void ActiveBlade()
    {
        bladeSoul.StartStrike();
    }
}
