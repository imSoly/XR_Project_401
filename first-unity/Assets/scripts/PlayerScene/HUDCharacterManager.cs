using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCharacterManager : MonoBehaviour
{
    public Text hudText;
    public GameObject character;
    public Vector3 offset;

    // Start is called before the first frame update
    public GameObject HudTextUp;
    public GameObject canvasObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(character != null)
        {
            // 캐릭터 머리 위에 텍스트 표시
            Vector3 characterHeadPosition = character.transform.position;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterHeadPosition);
            hudText.transform.position = screenPosition + offset;
        }
        
    }

    public void UpdateHUDText(string newText, GameObject target, Vector3 TargetOffset)
    {
        Vector3 TargetPosition= target.transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(TargetPosition);
        GameObject temp = (GameObject)Instantiate(HudTextUp);
        temp.transform.SetParent(canvasObject.transform, false);
        temp.transform.position = screenPosition + TargetOffset;
        temp.GetComponent<HUDMove>().textUI.text = newText;
    }
}
