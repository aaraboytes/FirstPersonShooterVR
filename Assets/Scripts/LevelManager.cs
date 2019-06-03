using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BulletType
{
    public AmmoType ammoType;
    public GameObject bullet;
}
public class Stats
{
    public int score = 0;
    public int bulletsShooted = 0;
    public int successfulBullets = 0;
    public float trackTime = 0;
    public float accuracy = 0;
    public int prom = 0;
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int Score { get; set; }
    public Text scoreText;
    float trackTime = 0;
    bool trackPlaying = false;
    int bulletsShooted = 0;
    int successfulBullets = 0;

    public BulletType[] bulletTypes;

    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        scoreText.text = "0";
    }
    private void Update()
    {
        if (trackPlaying)
            trackTime += Time.deltaTime;
    }
    public void IncreaseScore(int extra)
    {
        Score += extra;
        scoreText.text = Score.ToString();
        print("Score increased" + Score.ToString());
    }
    public void StartTrack() { trackPlaying = true; }
    public void EndTrack() { trackPlaying = false; }
    public void ResetTrack() { trackTime = 0; }
    public void IncreaseBulletsShooted() { bulletsShooted++; }
    public void IncreaseSuccessfulBullets() { successfulBullets++; }
    public int GetBulletsShooted() { return bulletsShooted; }
    public int GetSuccessfulBullets() { return successfulBullets; }
    public Stats CalculateStats()
    {
        Stats stats = new Stats();
        stats.score = Score;
        stats.trackTime = trackTime;
        stats.bulletsShooted = bulletsShooted;
        stats.successfulBullets = successfulBullets;
        stats.accuracy = successfulBullets / bulletsShooted;
        stats.prom = Score / successfulBullets;
        return stats;
    }
    public GameObject FindKindOfBullet(AmmoType ammoType)
    {
        GameObject bullet;
        foreach(BulletType bt in bulletTypes)
        {
            if (bt.ammoType.Equals(ammoType))
                return bt.bullet;
        }
        return null;
    }
}
