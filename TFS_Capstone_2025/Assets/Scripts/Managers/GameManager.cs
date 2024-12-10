using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;
    public Camera mainCamera;
    public GameObject sliderPrefab;

    public delegate void GameStart();
    public event GameStart GameStartedEvent;
    public delegate void GameOver();
    public event GameOver GameOverEvent;
    public delegate void GameWon();
    public event GameWon GameWonEvent;
    public delegate void GameQuit();
    public event GameQuit GameQuitEvent;
    public delegate void GameReturnToTitle();
    public event GameReturnToTitle GameReturnToTitleEvent;
    public delegate void PauseGame();
    public event PauseGame PauseGameEvent;
    public delegate void ResumeGame();
    public event ResumeGame ResumeGameEvent;
    public delegate void PlayerDead();
    public event PlayerDead PlayerDeadEvent;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStartedEvent += OnGameStarted;
        GameManager.Instance.GameOverEvent += OnGameOver;
        GameManager.Instance.GameWonEvent += OnGameWon; 
        GameManager.Instance.GameQuitEvent += OnQuitGame;
        GameManager.Instance.GameReturnToTitleEvent += OnReturnToTitle;
        GameManager.Instance.PauseGameEvent += OnPauseGame; 
        GameManager.Instance.ResumeGameEvent += OnResumeGame; 
        GameManager.Instance.PlayerDeadEvent += OnPlayerDead;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameStarted()
    {
        //TODO: add game init steps
    }

    public void OnGameOver()
    {
        //TODO: add game end steps - display game over menu
    }

    public void OnGameWon()
    {
        //TODO: add game won/victory steps - display game won menu, play victory music, etc
    }

    public void OnQuitGame()
    {
        Debug.Log("GM Quit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnPauseGame()
    {
        UnlockCursor();
    }

    public void OnReturnToTitle()
    {
        //TODO: load title scene or title menu
    }

    public void OnResumeGame()
    {
        LockCursor();
    }

    public void OnPlayerDead()
    {
        //TODO: check if player wishes to continue playing
        
        //if yes then respawn        
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

}
