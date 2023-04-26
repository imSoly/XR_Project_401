using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCharacterManager : MonoBehaviour
{
    public Text hubText;
    public GameObject character;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 characterHeadPosition = character.transform.position + offset;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterHeadPosition);
        hubText.transform.position = screenPosition + offset;
    }
}
