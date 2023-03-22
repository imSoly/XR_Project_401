using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    float rotSpeed = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            rotSpeed = 10000;
        }

        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);

        rotSpeed *= 0.99f; // 1%씩 속도 감소
    }
}
