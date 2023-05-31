using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScene : MonoBehaviour
{
    protected GameManager GameManager => GameManager.Instance; // �̱��� �ҷ�����
    public Button gameStartButton; // ������ ��ư ����
    public Button gameOverButton; // ������ ��ư ����

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
