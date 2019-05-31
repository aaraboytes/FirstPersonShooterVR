using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZoneParent : MonoBehaviour
{
    public int scoreValue;
    private void Start()
    {
        for(int i= 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.AddComponent<TargetZone>();
            child.GetComponent<TargetZone>().scoreVal = scoreValue;
        }
    }
}
