using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public int hp = 180;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            hp += 10;
        }

        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭
        {
            hp -= 10;
        }

        if (hp <= 50)
        {
            Debug.Log("Run!");
        }
        else if (hp >= 200)
        {
            Debug.Log("Attack!");
        }
        else
        {
            Debug.Log("Defense!");
        }
    }
}
