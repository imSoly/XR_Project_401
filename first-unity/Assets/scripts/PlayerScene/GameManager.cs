using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameManager() { }
    public static GameManager Instance { get; private set; } // 싱글톤화

    public PlayerHp playerHp; // 플레이어 HP
    public Image playerHpUiImage; // 플레이어 HP UI 이미지
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
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>(); // Tag로 오브젝트를 찾는다.
        playerHpUiImage = GameObject.FindGameObjectWithTag("UIHealthBar").GetComponent<Image>(); // Tag로 UI를 찾는다.
    }

    private void Update()
    {
        {
            playerHpUiImage.fillAmount = (float)playerHp.Hp / 100.0f; // 체력에 비례하게 작업
        }
    }
}
