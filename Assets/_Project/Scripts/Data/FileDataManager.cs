using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class FileDataManager : MonoBehaviour
{
    public static FileDataManager Instance;

    private string leaderboardPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            leaderboardPath = Path.Combine(Application.persistentDataPath, "leaderboard.csv");

            // Create file with header if it doesn't exist
            if (!File.Exists(leaderboardPath))
            {
                File.WriteAllText(leaderboardPath,
                    "Player,HighScore,Accuracy,Hits,Arrows\n");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLeaderboard(string player, int score, float accuracy, int hits, int arrows)
    {
        Dictionary<string, LeaderboardEntry> leaderboard =
            new Dictionary<string, LeaderboardEntry>();

        // Read existing data
        if (File.Exists(leaderboardPath))
        {
            var lines = File.ReadAllLines(leaderboardPath);

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(',');

                if (parts.Length < 5) continue;

                string existingPlayer = parts[0];
                int existingScore = int.Parse(parts[1]);
                float existingAccuracy = float.Parse(parts[2]);
                int existingHits = int.Parse(parts[3]);
                int existingArrows = int.Parse(parts[4]);

                leaderboard[existingPlayer] =
                    new LeaderboardEntry(existingPlayer,
                                         existingScore,
                                         existingAccuracy,
                                         existingHits,
                                         existingArrows);
            }
        }

        // Update only if new score is higher
        if (leaderboard.ContainsKey(player))
        {
            if (score > leaderboard[player].Score)
            {
                leaderboard[player] =
                    new LeaderboardEntry(player, score, accuracy, hits, arrows);
            }
        }
        else
        {
            leaderboard[player] =
                new LeaderboardEntry(player, score, accuracy, hits, arrows);
        }

        // Sort descending by score
        var sorted = leaderboard.Values
                                .OrderByDescending(e => e.Score)
                                .ToList();

        // Rewrite file
        using (StreamWriter writer = new StreamWriter(leaderboardPath, false))
        {
            writer.WriteLine("Player,HighScore,Accuracy,Hits,Arrows");

            foreach (var entry in sorted)
            {
                writer.WriteLine(
                    $"{entry.Player}," +
                    $"{entry.Score}," +
                    $"{entry.Accuracy}," +
                    $"{entry.Hits}," +
                    $"{entry.Arrows}"
                );
            }
        }

        Debug.Log("Leaderboard Updated Successfully.");
    }

    public class TopEntry
    {
        public string player;
        public int score;
    }

    public List<TopEntry> GetTopEntries(int count)
    {
        List<TopEntry> result = new List<TopEntry>();
        if (!File.Exists(leaderboardPath)) return result;

        var lines = File.ReadAllLines(leaderboardPath);
        for (int i = 1; i < lines.Length; i++) // Skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var parts = lines[i].Split(',');
            if (parts.Length < 5) continue;
            result.Add(new TopEntry { player = parts[0], score = int.Parse(parts[1]) });
        }

        return result.OrderByDescending(e => e.score).Take(count).ToList();
    }

    // Internal leaderboard structure
    private class LeaderboardEntry
    {
        public string Player;
        public int Score;
        public float Accuracy;
        public int Hits;
        public int Arrows;

        public LeaderboardEntry(string player, int score, float accuracy, int hits, int arrows)
        {
            Player = player;
            Score = score;
            Accuracy = accuracy;
            Hits = hits;
            Arrows = arrows;
        }
    }
}