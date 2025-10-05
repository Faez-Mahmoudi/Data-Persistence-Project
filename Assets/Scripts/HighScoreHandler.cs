using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreHandler : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    //public List<TextMeshProUGUI> nameTexts;
    //public List<create a class> className;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       nameText.text = MyManager.Instance.bestPlayerName;
       scoreText.text = MyManager.Instance.bestScore.ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
