using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
            gameObject.SetActive(false);
    }
}
