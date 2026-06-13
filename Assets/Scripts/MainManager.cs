using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField] private int waveCount = 1;
    [SerializeField] private Brick BrickPrefab;
    [SerializeField] private int LineCount = 6;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private Rigidbody[] Balls;
    [SerializeField] private GameObject[] pos;

    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI BestScoreText;
    [SerializeField] private GameObject GameOverText;
    
    private bool m_Started;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Started = false;
        Time.timeScale = 1;

        // Instantiate bricks
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x <= i; ++x)
            {
                Vector3 position1 = new Vector3(-3.5f + step * x, 2.5f + i * 0.3f, 0);
                Vector3 position2 = new Vector3(3.5f + step * -x, 2.5f + i * 0.3f, 0);

                var brick1 = Instantiate(BrickPrefab, position1, Quaternion.identity);
                brick1.PointValue = pointCountArray[i];
                brick1.onDestroyed.AddListener(AddPoint);

                var brick2 = Instantiate(BrickPrefab, position2, Quaternion.identity);
                brick2.PointValue = pointCountArray[i];
                brick2.onDestroyed.AddListener(AddPoint);
            }
        }

        for (int j = 0; j < 8; ++j)
        {
            Vector3 position = new Vector3(-2.1f + step * j, 2.5f, 0);
            var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
            brick.PointValue = pointCountArray[0];
            brick.onDestroyed.AddListener(AddPoint);
        }

        if (waveCount == 1)
        {
            BallInstantiate(0);
        }
        else if (waveCount == 2)
        {
            BallInstantiate(0);
            BallInstantiate(1);
        }
        else if (waveCount >= 3)
        {
            waveCount = 2;
            BallInstantiate(0);
            BallInstantiate(1);
            BallInstantiate(2);
        }

        // Interpolate name and score at the start
        ScoreText.text = $"{MyManager.Instance.playerName}'s Score : {m_Points}";
        // Interpolate bestPlayerName and bestScore at start
        BestScoreText.text = $"Best Score : {MyManager.Instance.bestPlayerName} : {MyManager.Instance.bestScore}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                
                foreach (var item in Balls)
                {
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();
                    item.transform.SetParent(null);
                    item.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        var bricks = FindObjectsByType<Brick>(FindObjectsSortMode.None);
        if (bricks.Length == 0)
        {
            m_Started = false;
            waveCount++;
            foreach (var item in Balls)
                item.gameObject.SetActive(false);

            Start();
        }
    }

    void BallInstantiate(int i)
    {    
        Balls[i].transform.position = pos[i].transform.position;
        Balls[i].transform.rotation = pos[i].transform.rotation;
        Balls[i].linearVelocity = new Vector3();
        Balls[i].angularVelocity = new Vector3();
        Balls[i].gameObject.SetActive(true);
        Balls[i].transform.SetParent(paddlePrefab.transform);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{MyManager.Instance.playerName}'s Score : {m_Points}";

        // Check if bestScore is beaten
        if (m_Points >= MyManager.Instance.bestScore)
        {
            MyManager.Instance.bestPlayerName = MyManager.Instance.playerName;
            MyManager.Instance.bestScore = m_Points;   
            BestScoreText.text = $"Best Score : {MyManager.Instance.bestPlayerName} : {m_Points}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        Time.timeScale = 0; // Prevents the player from moving on game over
        if (m_Points >= MyManager.Instance.highScores[4].bestScore)
        {
            MyManager.Instance.AddNewScore(MyManager.Instance.playerName, m_Points);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        MyManager.Instance.SaveScore();
    }
}
