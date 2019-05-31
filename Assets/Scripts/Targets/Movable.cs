using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] Transform[] limits;
    [SerializeField]
    Transform currentDestination;
    public float speed;
    float magnitude;
    int index = 0;
    void Start()
    {
        index = Random.Range(0, limits.Length - 1);
        currentDestination = limits[index];
        magnitude = Vector3.Distance(limits[1].position,limits[0].position);
        //transform.localPosition = limits[0].localPosition;
        transform.localPosition = (currentDestination.localPosition - transform.localPosition).normalized * Random.Range(0, magnitude);
    }

    void Update()
    {
        if (Vector3.Distance(new Vector3(transform.localPosition.x, currentDestination.localPosition.y, transform.localPosition.z), currentDestination.localPosition) <= 0.1f)
        {
            index = index == 0 ? 1 : 0;
            currentDestination = limits[index];
        }
        Vector3 movementDir = currentDestination.localPosition - new Vector3(transform.localPosition.x,currentDestination.localPosition.y,transform.localPosition.z);
        transform.Translate(movementDir.normalized * speed * Time.deltaTime);
    }
}
