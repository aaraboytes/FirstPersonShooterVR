using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] Transform end;
    [SerializeField] Transform horizontalBound;
    [SerializeField] int segments;
    [SerializeField] int originScore;
    [SerializeField] bool rotates = true;
    Rotate rotator;

    float radio,segmentLenght;
    private void Start()
    {
        rotator = rotates?transform.parent.GetComponent<Rotate>() : null;
        radio = Vector3.Distance(origin.transform.position, end.transform.position);
        segmentLenght = radio / segments;
    }
    public void Hit(Vector3 bulletCollision)
    {
        if (Vector3.Distance(bulletCollision, origin.position) < Vector3.Distance(end.transform.position, origin.position))
        {
            //Get score        
            float bulletToOriginDist = Vector3.Distance(origin.transform.position, bulletCollision);
            print(bulletToOriginDist);
            int selectedSegment = 4;
            for (int i = 0; i < segments; i++)
            {
                if (bulletToOriginDist < segmentLenght * i)
                {
                    selectedSegment = i - 1;
                    break;
                }
            }
            int score = originScore - selectedSegment;
            LevelManager.Instance.IncreaseScore(score);
        }
        //Rotate
        if (rotates)
        {
            Vector3 dirToBullet = bulletCollision - new Vector3(origin.position.x, bulletCollision.y, origin.position.z);
            if (dirToBullet.magnitude > 0)
            {
                float horBounds = Vector3.Distance(horizontalBound.position, new Vector3(origin.position.x, horizontalBound.position.y, origin.position.z));
                float conversion = dirToBullet.magnitude / horBounds;
                //Determine front or back
                Vector3 targetToBullet = bulletCollision - transform.parent.position;
                float dot = Vector3.Dot(Vector3.up, targetToBullet);
                if (dot > 0)
                {
                    //front
                    if (bulletCollision.x < origin.position.x)
                        rotator.RotateInAxis(-conversion);
                    else
                        rotator.RotateInAxis(conversion);

                }
                else
                {
                    //back
                    if (bulletCollision.x < origin.position.x)
                        rotator.RotateInAxis(conversion);
                    else
                        rotator.RotateInAxis(-conversion);
                }
            }
        }
    }
}
