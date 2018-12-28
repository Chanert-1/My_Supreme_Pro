using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectRole : MonoBehaviour
{

    public Image[] roles;

    public Image[] rolesImage;

    public Image mask;

    [HideInInspector]
    public static int index;



    // Update is called once per frame
    void Update()
    {


        if (mask.transform.position != roles[index].transform.position - new Vector3(0, 60, 0))
            mask.transform.position = roles[index].transform.position - new Vector3(0, 60, 0);



        if (Input.GetKeyDown(KeyCode.A))
        {
            index--;
            if (index < 0)
            {
                index = roles.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            index++;
            if (index > roles.Length - 1)
            {
                index = 0;
            }

        }



    }
}
