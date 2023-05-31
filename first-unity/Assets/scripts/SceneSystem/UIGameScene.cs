using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScene : MonoBehaviour
{
    protected GameManager GameManager => GameManager.Instance; // 싱글톤 불러오기
    public Button gameStartButton; // 선언한 버튼 설정
    public Button gameOverButton; // 선언한 버튼 설정

    // Start is called before the first frame update
    void Start()
    {
        gameStartButton.onClick.AddListener(OnGameStartButtonClick);
        gameOverButton.onClick.AddListener(OnGameOverButtonClick);
    }

    private void OnGameStartButtonClick()
    {
        GameManager.StartGame();
    }

    private void OnGameOverButtonClick()
    {
        GameManager.GameOver();
    }
}
