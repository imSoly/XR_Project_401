using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float characterSpeed = 0;
    public GameDirector characterRender;

    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        Debug.Log("Fade");
        Color c = GetComponent<Renderer>().material.color; // material 접근

        for(float fadeOffset = 1.0f; fadeOffset >= 0; fadeOffset -= 0.1f)
        {
            Debug.Log("For");
            Debug.Log(characterRender.GetComponent<Renderer>().material.color);
            c.b = fadeOffset; // Blue 값 감소
            c.g = fadeOffset; // Green 값 감소

            characterRender.GetComponent<Renderer>().material.color = c;

            yield return new WaitForSeconds(1.0f); // for문을 1초마다 돌아가게 함
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            characterSpeed = 5.0f;
        }

        transform.Translate(0, 0, characterSpeed * Time.deltaTime);

        characterSpeed *= 0.99f;
    }
}
