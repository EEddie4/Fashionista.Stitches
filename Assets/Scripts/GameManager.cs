using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TileBoard board;
    public CanvasGroup gameOver;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public AudioSource gameOverMusic;
    public AudioSource levelMusic;

    public TimerScript timer;
    
    public float powerUpDuration = 10.0f;

    private int score;

    public float cooldownFreeze = 0.15f;

    public float cooldownBolt = 0.15f;
    public float timeDelay = 30.0f;

    private void Start()
    {
        NewGame();
    }

    public void NewGame ()
    {
        LoadHighScore();
        timer.TimeLeft = 300f;
        SetScore(0);
        highScoreText.text = LoadHighScore().ToString();
        gameOver.alpha = 0f;
        gameOver.interactable = false; 

        board.ClearBoard();
        board.CreateNewTile();
        board.CreateNewTile();
        board.enabled = true;
        levelMusic.Play();
        timer.TimerOn = true;
        

    }


    public void GameOver ()
    {

        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 1f));
        levelMusic.Stop();
        gameOverMusic.Play();
        timer.TimerOn = false;
        
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds (delay);

        float elapsed = 0;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed<duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;

    }


    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore (int score)
    {
        this.score = score;

        scoreText.text = score.ToString();
    }


    private void SaveHighScore()
    {
        int highscore = LoadHighScore();

        if (score > highscore)
        PlayerPrefs.SetInt("highscore", score);
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public void SetTimeScale (float scale) 
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = scale* .02f;
    }

    public IEnumerator SlowDownTime()
    {
        if (score >= 500 && Time.time > cooldownFreeze )
            SetTimeScale(.25f);
            yield return new WaitForSeconds(powerUpDuration);
            SetTimeScale(1.25f);
            score -= 500;
            cooldownFreeze = Time.time + timeDelay;
    }

    public IEnumerator DestroyAllTiles()
     {
        if (score >= 5000)
        {
            
            board.ClearBoard();
            board.CreateNewTile();
            board.CreateNewTile();
            cooldownBolt = Time.time + timeDelay * 50.0f;
            yield return (0);
        }
     }


    public void BoltAction ()
    {
        StartCoroutine(DestroyAllTiles());
    }

    public void Freeze ()
    {
        StartCoroutine(SlowDownTime());
    }

    private void Update()
    {
        SaveHighScore();
    }
}
