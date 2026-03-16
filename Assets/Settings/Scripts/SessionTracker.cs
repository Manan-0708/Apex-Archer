using System;
using System.IO;
using UnityEngine;

public class SessionTracker : MonoBehaviour
{
    public static SessionTracker Instance;

    private string sessionPath;

    private string playerName;
    private string startTime;

    private int arrowsShot = 0;
    private int hits = 0;
    private int misses = 0;
    private float score = 0f;
    private float bowHoldTime = 0f;

    private float bowGrabStartTime = 0f;
    private bool isBowHeld = false;
    private bool sessionActive = false;

    public int ArrowsShot => arrowsShot;
    public int Hits => hits;
    public int Misses => misses;
    public float Score => score;
    public float Accuracy => arrowsShot > 0 ? (float)hits / arrowsShot : 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            sessionPath = Path.Combine(Application.persistentDataPath, "sessiondetails.csv");

            if (!File.Exists(sessionPath))
            {
                File.WriteAllText(sessionPath,
                    "Player,StartTime,ArrowsShot,Hits,Misses,Accuracy,Score,BowHoldTime\n");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSession(string player)
    {
        Debug.Log("Starting session for player: " + player);
        if (string.IsNullOrEmpty(player))
            return;

        sessionActive = true;

        playerName = player;
        startTime = DateTime.Now.ToString("o");

        arrowsShot = 0;
        hits = 0;
        misses = 0;
        score = 0f;
        bowHoldTime = 0f;
    }

    public void EndSession()
    {
        if (!sessionActive) return;

        sessionActive = false;

        float accuracy = arrowsShot > 0 ? (float)hits / arrowsShot : 0f;

        string line =
            $"{playerName},{startTime},{arrowsShot},{hits},{misses},{accuracy},{score},{bowHoldTime}\n";

        File.AppendAllText(sessionPath, line);

        FileDataManager.Instance.UpdateLeaderboard(playerName, (int)score, accuracy, hits, arrowsShot);
    }

    public void OnArrowShot()
    {
        if (!sessionActive) return;
        arrowsShot++;
    }

    public void OnHit(int points)
    {
        Debug.Log("Hit registered with points: " + points);
        if (!sessionActive) return;
        hits++;
        score += points;
    }

    public void OnMiss()
    {
        if (!sessionActive) return;
        misses++;
    }

    public void OnBowGrab()
    {
        if (!sessionActive) return;
        bowGrabStartTime = Time.time;
        isBowHeld = true;
    }

    public void OnBowRelease()
    {
        if (!sessionActive) return;

        if (isBowHeld)
        {
            bowHoldTime += Time.time - bowGrabStartTime;
            isBowHeld = false;
        }
    }
}