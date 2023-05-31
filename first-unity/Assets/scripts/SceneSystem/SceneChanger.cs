using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ����Ƽ �� �̵��� ����
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public float transitionDuration = 1f;

    public CanvasGroup canvasGroup;
    public SceneChanger() { }
    public static SceneChanger Instance { get; private set; } // �̱���ȭ

    public void Awake()
    {
        if (Instance)
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
    }

    private void OnDestroy() // �� ������Ʈ�� �ı��� ���
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �̺�Ʈ�� �����Ѵ�.
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvasGroup = FindObjectOfType<CanvasGroup>();
        if(canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void PlayTransitionAnimation(System.Action callback)
    {
        if (canvasGroup != null)
        {
            canvasGroup.DOFade(0f, transitionDuration).OnComplete(() =>
            {
                // ���� ���� 1 -> 0���� 1�ʵ��� �̵��ϰ�, �Ϸ�Ǹ� ��ϵ� callback���� �̵�
                callback.Invoke();
            });
        }
        else
        {
            callback.Invoke();
        }
    }

    public void LoadLobbyScene()
    {
        PlayTransitionAnimation(() => {
            SceneManager.LoadScene("LobbyScene");
        });
    }
    public void LoadGameScene()
    {
        PlayTransitionAnimation(() => {
            SceneManager.LoadScene("Game_Scene");
        });
    }

    public void LoadEndScene()
    {
        PlayTransitionAnimation(() => {
            SceneManager.LoadScene("EndScene");
        });
    }
}
