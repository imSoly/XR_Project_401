using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ����Ƽ �� �̵��� ����
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    protected SceneChanger SceneChanger => SceneChanger.Instance; // �̱��� �ҷ�����

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
            onGameStateChanged?.Invoke(currentState); // �̺�Ʈ�� null�� �ƴ� ��쿡�� �� �̺�Ʈ�� ȣ��
        }
    }

    public void StartGame()
    {
        // ���� ���� ������ ���⿡ �ۼ�
        CurrentState = GameState.Playing;
    }

    public void GameOver()
    {
        // ���� ���� ������ ���⿡ �ۼ�
        CurrentState = GameState.GameOver;
        SceneChanger.LoadEndScene();
    }

    public GameManager() { }
    public static GameManager Instance { get; private set; } // �̱���ȭ

    public PlayerHp playerHp; // �÷��̾� HP
    public Image playerHpUiImage; // �÷��̾� HP UI �̹���
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

    private void OnDestroy() // �� ������Ʈ�� �ı��� ���
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �̺�Ʈ�� �����Ѵ�.
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
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>(); // Tag�� ������Ʈ�� ã�´�.
        playerHpUiImage = GameObject.FindGameObjectWithTag("UIHealthBar").GetComponent<Image>(); // Tag�� UI�� ã�´�.
        playerHp.Hp = 100;
        CurrentState = GameState.Start;
    }

    private void Update()
    {
        {
            playerHpUiImage.fillAmount = (float)playerHp.Hp / 100.0f; // ü�¿� ����ϰ� �۾�
        }
    }
}
