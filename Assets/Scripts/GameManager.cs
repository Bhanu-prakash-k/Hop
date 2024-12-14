using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver = false;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel;
    public int score;
    public TMP_Text scoreText;
    public TMP_Text gameOverScoreText;

    private void Awake() {
        if(instance == null)
            instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameOver = true;
        startPanel.SetActive(true);
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        ResetScore();
        //hudPanel.SetActive(false);
        //gameOverPanel.SetActive(false);
    }

    public void OnPlayButtonClick(){
        isGameOver = false;
        startPanel.SetActive(false);
        hudPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }
    public void AddScore(int amount){
        score += amount;
        scoreText.text = score.ToString("00");
    }
    public void ResetScore(){
        score = 0;
        scoreText.text = score.ToString("00");
    }
    public void OnRetryButtonClick(){
        SceneManager.LoadScene(0);
    }
    public void OnGameOver(){
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score - " + score.ToString("00");
    }
}
