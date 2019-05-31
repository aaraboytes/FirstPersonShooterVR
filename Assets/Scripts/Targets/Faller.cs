using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : MonoBehaviour
{
    [SerializeField] bool animated = true;
    [SerializeField] float waitTime;
    [SerializeField] float angleSpeed;
    [SerializeField]float timer = 0;
    [SerializeField] bool falled = false;
    Animator anim;
    bool moving = false;
    private void Start()
    {
        timer = Random.Range(0, waitTime);
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime && !moving)
        {
            if (falled)
                GetUp();
            else
                Fall();
        }
    }
    public void Fall()
    {
        moving = true;
        falled = true;
        anim.SetTrigger("Fall");
    }
    public void GetUp()
    {
        moving = true;
        falled = false;
        anim.SetTrigger("GetUp");
    }
    public void EndAnim()
    {
        moving = false;
        timer = 0;
    }
}
