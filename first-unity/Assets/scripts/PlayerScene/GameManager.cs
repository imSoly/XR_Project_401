using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 유니티 씬 이동을 위해
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    protected SceneChanger SceneChanger => SceneChanger.Instance; // 싱글톤 불러오기

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    }

    public event Action<GameState> onGameStateChanged;

    public GameState currentState = GameState.Start;

    public GameState CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            onGameStateChanged?.Invoke(currentState); // 이벤트가 null이 아닌 경우에만 이 이벤트를 호출
        }
    }

    public void StartGame()
    {
        // 게임 시작 로직을 여기에 작성
        CurrentState = GameState.Playing;
    }

    public void GameOver()
    {
        // 게임 오버 로직을 여기에 작성
        CurrentState = GameState.GameOver;
        SceneChanger.LoadEndScene();
    }

    public GameManager() { }
    public static GameManager Instance { get; private set; } // 싱글톤화

    public PlayerHp playerHp; // 플레이어 HP
    public Image playerHpUiImage; // 플레이어 HP UI 이미지
    public Button BtnSample;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        } 
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        Init();
    }

    private void OnDestroy() // 이 오브젝트가 파괴될 경우
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트를 삭제한다.
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game_Scene")
        {
            Init();

        }
    }

    private void Init()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>(); // Tag로 오브젝트를 찾는다.
        playerHpUiImage = GameObject.FindGameObjectWithTag("UIHealthBar").GetComponent<Image>(); // Tag로 UI를 찾는다.
        playerHp.Hp = 100;
        CurrentState = GameState.Start;
    }

    private void Update()
    {
        {
            playerHpUiImage.fillAmount = (float)playerHp.Hp / 100.0f; // 체력에 비례하게 작업
        }
    }
}
