using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSystem : MonoBehaviour
{
    private Vector2 initialPos;
    // Update is called once per frame
    void Update()
    {
        // Swipe 방향 검사
        if (Input.GetMouseButtonDown(0)) initialPos = Input.mousePosition;
        if (Input.GetMouseButtonUp(0)) Calculate(Input.mousePosition);

    }

    void Calculate(Vector3 finalPos)
    {
        float disX = Mathf.Abs(initialPos.x - finalPos.x);
        float disY = Mathf.Abs(initialPos.y - finalPos.y);

        if(disX > 0 || disY > 0)
        {
            if(disX > disY)
            {
                if (initialPos.x > finalPos.x) Debug.Log("Left");
                else Debug.Log("Right");
            }
            else
            {
                if (initialPos.y > finalPos.y) Debug.Log("Down");
                else Debug.Log("Up");
            }
        }
    }
}
