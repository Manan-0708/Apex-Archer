using System;
using System.IO;
using UnityEngine;

public class VRCSVLogger : MonoBehaviour
{
    private static VRCSVLogger instance;
    private string filePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitCSV();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitCSV()
    {
        string fileName = "VR_Bow_Logs_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(filePath,
            "Timestamp,EventType,ObjectName,Hand,ExtraData\n");
    }

    public static void Log(string eventType, string objectName, string hand, string extraData = "-")
    {
        if (instance == null) return;

        string time = DateTime.Now.ToString("o");
        string line = $"{time},{eventType},{objectName},{hand},{extraData}\n";

        File.AppendAllText(instance.filePath, line);
    }
}