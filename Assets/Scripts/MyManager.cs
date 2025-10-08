using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MyManager : MonoBehaviour
{
    public static MyManager Instance;
    public string playerName;

    // data for our ScoreEntry class
    public string bestPlayerName;
    public int bestScore;

    // data for our SaveData class
    public List<ScoreEntry> highScores = new List<ScoreEntry>();

    [System.Serializable]
    public class ScoreEntry
    {
        public string bestPlayerName;
        public int bestScore;
    }

    [System.Serializable]
    class SaveData
    {
        public List<ScoreEntry> highScores = new List<ScoreEntry>();
    }



    private void Awake()
    {
        // Sigleton: ensure thre is just one MyManager GameObject on the Scene
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // LoadScore at Awake
        LoadScore();
        bestPlayerName = highScores[0].bestPlayerName;
        bestScore = highScores[0].bestScore;
    }

    // Add a new score and save it
    public void AddNewScore(string playerName, int score)
    {
        ScoreEntry entry = new ScoreEntry { bestPlayerName = playerName, bestScore = score };
        highScores.Add(entry);

        // Sort descending
        highScores.Sort((a, b) => b.bestScore.CompareTo(a.bestScore));

        // Optional: keep only top 5
        if (highScores.Count > 5)
            highScores = highScores.GetRange(0, 5);

        SaveScore();
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.highScores = highScores;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            highScores = data.highScores ?? new List<ScoreEntry>();
        }
    }
}
