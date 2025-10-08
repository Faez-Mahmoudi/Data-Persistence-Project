using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class HighScoreHandler : MonoBehaviour
{

    public List<TextMeshProUGUI> nameTexts;
    public List<TextMeshProUGUI> scoreTexts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       for (int i = 0; i < MyManager.Instance.highScores.Count; ++i)
       {
            nameTexts[i].text = MyManager.Instance.highScores[i].bestPlayerName;
            scoreTexts[i].text = MyManager.Instance.highScores[i].bestScore.ToString(); 
       }   
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
