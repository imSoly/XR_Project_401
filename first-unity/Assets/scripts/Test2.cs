using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour
{
    // Declare member variables
    public int hp = 180;
    public Text textHpUi;
    public Text textStateUi;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /* ==========================================================
         * UI
         * ========================================================== */
        textHpUi.text = hp.ToString();

        if (hp <= 50)
        {
           textStateUi.text ="Run!";
        }
        else if (hp >= 200)
        {
            textStateUi.text = "Attack!";
        }
        else
        {
            textStateUi.text = "Defense!";
        }

        /* ==========================================================
         * Input
         * ========================================================== */


        if (Input.GetMouseButtonDown(0)) // Click mouse left button
        {
            hp += 10;
        }

        if (Input.GetMouseButtonDown(1)) // Click mouse right button
        {
            hp -= 10;
        }
    }
}
