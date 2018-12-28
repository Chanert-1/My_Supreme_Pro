using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject king;
    public GameObject dancer;

    public CameraFollow mainCam;

    private GameObject theRole;

    private void Awake()
    {
        switch (SelectRole.index)
        {
            case 0:
                theRole = Instantiate(king,transform.position,transform.rotation);
                break;

            case 1:
                theRole = Instantiate(dancer, transform.position, transform.rotation);
                break;
        }

        mainCam.target = theRole.transform;
    }

 
}
