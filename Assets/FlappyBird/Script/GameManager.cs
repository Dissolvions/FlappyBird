using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 public static GameManager instance { get; private set;}

    [Header("Start Screen")]
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject playButton;

    [Header("Score")]
    [SerializeField] private TMP_Text score;

    [Header("Game ready section")]
    [SerializeField] private GameObject gameReadyPanel;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverScore;
    [SerializeField] private TMP_Text gameOverBestScore;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PipeSpawner pipeSpawner;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeAmount = 0.05f;
    private const string BEST_SCORE_KEY = "BestScore";

    private GameState gameState = GameState.Home;

    public GameState GameState => gameState;
    private Camera mainCamera;

    private int currentScore;
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            score.text = currentScore.ToString();
        }
    }
    
    public int BestScore
    {
        get => PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        set
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY, value);
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        mainCamera = Camera.main; 
    }

    private void Start()
    {
        gameState = GameState.Home;
        logo.SetActive(true);
        playButton.SetActive(true);

        gameOverPanel.SetActive(false);
        gameReadyPanel.SetActive(false);
        score.gameObject.SetActive(false);
    }
    public void PlayButtonClick()
    {
        gameState = GameState.GetReady;
        CurrentScore = 0;

        logo.SetActive(false);
        playButton.SetActive(false);

        gameOverPanel.SetActive(false);
        gameReadyPanel.SetActive(true);
        score.gameObject.SetActive(true);

        ResetGame();
    }

    private void ResetGame()
    {
        playerController.ResetPlayer();
        pipeSpawner.ResetSpawner();

    }

    public void GamePlay()
    {
        gameState = GameState.Playing;
        gameReadyPanel.SetActive(false);

    }

    public void GameOver()
    {
        gameState = GameState.GameOver;

        StartCoroutine(ShakeCamera());
        score.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        playButton.SetActive(true);

        gameOverScore.text = currentScore.ToString();
        if(CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }
        gameOverBestScore.text = BestScore.ToString();
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float timer = 0;
        
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);

            mainCamera.transform.position = originalPos + new Vector3(x, y, 0);

            yield return null;

        }
        mainCamera.transform.position = originalPos;
    }
    public void AddScore()
    {
        if (gameState != GameState.Playing) return;

        CurrentScore++;

        AudioManager.instance.Score();
    }
}
