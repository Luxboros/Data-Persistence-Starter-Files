using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    private int LineCount = 21;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject NameInput;
    public GameObject GameOverText;

    private bool m_Started;
    private int m_Points;

    private bool m_GameOver;
    private static string m_BestScoreName;
    private static int m_BestScorePoints;


    private void Awake()
    {
        LoadBestScore();
        if (m_BestScorePoints != 0 && m_BestScoreName != null)
            BestScoreText.text = $"Best Score : {m_BestScoreName} : {m_BestScorePoints}";
    }

    void Start()
    {
        const float step = 0.7f;
        int perLine = Mathf.FloorToInt(17f / step);
        int[] pointsArray = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-8f + step * x, 2f + i * 0.35f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointsArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > m_BestScorePoints)
            NameInput.SetActive(true);
    }

    public void RegisterName(string name)
    {
        if (m_Points > m_BestScorePoints)
        {
            m_BestScoreName = name;
            m_BestScorePoints = m_Points;
            SaveBestScore();
        }

        BestScoreText.text = $"Best Score : {m_BestScoreName} : {m_BestScorePoints}";
        NameInput.SetActive(false);
    }

    [Serializable]
    public class BestScoreRecord
    {
        public int PlayerBestScore;
        public string PlayerName;
    }

    private static void SaveBestScore()
    {
        BestScoreRecord data = new BestScoreRecord
        {
            PlayerName = m_BestScoreName,
            PlayerBestScore = m_BestScorePoints
        };

        var json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/record.json", json);
    }

    private void LoadBestScore()
    {
        var path = Application.persistentDataPath + "/record.json";
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<BestScoreRecord>(json);

            m_BestScoreName = data.PlayerName;
            m_BestScorePoints = data.PlayerBestScore;
        }
    }
}