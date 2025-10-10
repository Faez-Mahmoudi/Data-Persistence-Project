using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MyManager : MonoBehaviour
{
    public static MyManager Instance;

    public string playerName;

    // My music
    public AudioSource musicSource;

    // Best score tracking (for quick access)
    public string bestPlayerName;
    public int bestScore;

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
        public float volume = 1f;
        public bool soundEnabled = true;
    }

    // Actual runtime data
    public List<ScoreEntry> highScores = new List<ScoreEntry>();
    public float volume = 1f;
    public bool soundEnabled = true;

    private string savePath;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/savefile.json";

        LoadScore();
        ApplyAudioSettings();

        // safely update "best" values
        if (highScores.Count > 0)
        {
            bestPlayerName = highScores[0].bestPlayerName;
            bestScore = highScores[0].bestScore;
        }

        // apply loaded settings
        AudioListener.volume = volume;
        AudioListener.pause = !soundEnabled;
    }

    public void ApplyAudioSettings()
    {
        musicSource.volume = volume;
        musicSource.mute = !soundEnabled;
    }

    // Add a new score and save it
    public void AddNewScore(string playerName, int score)
    {
        ScoreEntry entry = new ScoreEntry
        {
            bestPlayerName = playerName,
            bestScore = score
        };

        highScores.Add(entry);

        // Sort descending
        highScores.Sort((a, b) => b.bestScore.CompareTo(a.bestScore));

        // Keep only top 5
        if (highScores.Count > 5)
            highScores = highScores.GetRange(0, 5);

        bestPlayerName = highScores[0].bestPlayerName;
        bestScore = highScores[0].bestScore;

        SaveScore();
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.highScores = highScores;
        data.volume = volume;
        data.soundEnabled = soundEnabled;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScores = data.highScores ?? new List<ScoreEntry>();
            volume = data.volume;
            soundEnabled = data.soundEnabled;
        }
        else
        {
            // initialize defaults for first run
            highScores = new List<ScoreEntry>();
            volume = 1f;
            soundEnabled = true;
        }
    }
}