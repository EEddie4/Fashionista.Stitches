using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] popUps;
    private int popUpIndex ; 
    
    void CheckForTutorial() 
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex){
                popUps[i].SetActive(true);
            }
            else{
                popUps[i].SetActive(false);
            }
        }


        if(popUpIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || TileBoard.swipeDirection == TileBoard.Swipe.Up)
            {
                popUpIndex++;
                Debug.Log("0");
            }
        }
            
        else if (popUpIndex == 1)
            {
                if(Input.GetKeyDown(KeyCode.DownArrow) || TileBoard.swipeDirection == TileBoard.Swipe.Down)
                    {
                        popUpIndex++;
                        Debug.Log("1");
                    }
            }
        else if (popUpIndex == 2)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || TileBoard.swipeDirection == TileBoard.Swipe.Left)
                    {   popUpIndex++;
                        Debug.Log("2");
                    }
            }
        else if (popUpIndex == 3)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || TileBoard.swipeDirection == TileBoard.Swipe.Right)
                    {   popUpIndex++;
                        Debug.Log("3");
                        popUpIndex = 4
                        ;
                    }
            }
        
        if (popUpIndex == 4){

            popUpIndex = 0;
        }
            
            

        
    }



    public void Undo ()
    {
        Debug.Log("Undo button was pressed");
    }

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


    private void Start()
    {
        NewGame();
    }

    private void LateUpdate() {
        CheckForTutorial(); 
    }

    public void NewGame ()
    {  
        timer.TimeLeft = 1000f;
        SetScore(0);
        highScoreText.text = LoadHighScore().ToString();
        gameOver.alpha = 0f;
        gameOver.interactable = false; 

        board.ClearBoard();
        board.CreateNewTile();
        board.CreateNewTile();
        board.enabled = true;
        levelMusic.Play();
        timer.TimerOn = false;

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


    private void SaveHighScore ()
    {
        int highscore = LoadHighScore();

        if (score > highscore)
        PlayerPrefs.SetInt("highscore", score);
    }

    private int LoadHighScore ()
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
        if (score >= 500)
            score -= 500;
            SetTimeScale(.25f);
            yield return new WaitForSeconds(powerUpDuration);
            SetTimeScale(1.25f);


    }

    public void Freeze ()
    {
        StartCoroutine(SlowDownTime());
    }
}
