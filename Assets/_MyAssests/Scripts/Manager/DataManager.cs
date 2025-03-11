using System.Diagnostics;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public TMP_Text[] currentScoreText;
    public TMP_Text[] HighestScoreText;
    [SerializeField] GameObject gameEndPanel;
    [SerializeField] internal int _currentScore = 0;
    [SerializeField] private GameObject crownImg;
    void Start()
    {
        _currentScore = 0;
    }

    public void RestCurrentScore(){
        _currentScore = 0;
        foreach (var score in currentScoreText)
        {
            score.text = _currentScore.ToString();
        }

    }

    // Update is called once per frame
    public void UpdateScore()
    {
        _currentScore++;
        foreach (var score in currentScoreText)
        {
            score.text = _currentScore.ToString();
        }
    }
    public void GameEndMenu()
    {
        crownImg.SetActive(false);
        int lastScore = PlayerPrefs.GetInt("Score");
        if (lastScore < _currentScore)
        {
            PlayerPrefs.SetInt("Score", _currentScore);
            foreach (var VARIABLE in HighestScoreText)
            {
                VARIABLE.text = _currentScore.ToString();
            }
            crownImg.SetActive(true);
        }
        else
        {
            foreach (var VARIABLE in HighestScoreText)
            {
                VARIABLE.text = lastScore.ToString();
            }
        }
        foreach (var score in currentScoreText)
        {
            score.text = _currentScore.ToString();
        }

        gameEndPanel.SetActive(true);
    }

    public void ReStartMenu()
    {
        Invoke(nameof(ReleadScene), 0.2f);
    }

    void ReleadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void CurrentSelectedCharIndex()
    {

    }
}
