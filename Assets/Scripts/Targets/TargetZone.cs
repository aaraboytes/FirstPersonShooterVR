using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public int scoreVal;
    public void Hit(Vector3 bulletCollision)
    {
        //GetScore
        int score = scoreVal;
        LevelManager.Instance.IncreaseScore(score);
    }
}
