using UnityEngine;
using System.IO;

public class MyManager : MonoBehaviour
{
    public static MyManager Instance;
    public string playerName;

    // data for our SaveData class
    public string bestPlayerName;
    public int bestScore;

    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public int bestScore;
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
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = bestPlayerName;
        data.bestScore = bestScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayerName = data.bestPlayerName;
            bestScore = data.bestScore;
        }
    }
}
