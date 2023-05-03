using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameManager() { }
    public static GameManager Instance { get; private set; } // �̱���ȭ

    public PlayerHp playerHp; // �÷��̾� HP
    public Image playerHpUiImage; // �÷��̾� HP UI �̹���
    public Button BtnSample;

    private void Start()
    {
        this.BtnSample.onClick.AddListener(() =>
        {
            Debug.Log("Button Check");
        });
    }

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

        Init();
    }

    private void Init()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>(); // Tag�� ������Ʈ�� ã�´�.
        playerHpUiImage = GameObject.FindGameObjectWithTag("UIHealthBar").GetComponent<Image>(); // Tag�� UI�� ã�´�.
    }

    private void Update()
    {
        {
            playerHpUiImage.fillAmount = (float)playerHp.Hp / 100.0f; // ü�¿� ����ϰ� �۾�
        }
    }
}
