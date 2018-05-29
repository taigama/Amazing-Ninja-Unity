using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLocalPos : MonoBehaviour
{
    public Vector3 postReset;

    public void OnEnable()
    {
        transform.localPosition = postReset;
    }
}