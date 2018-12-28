using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    public Image line;

    private AsyncOperation ao;

    public Text text;

    private string txt;

    private byte textColor;

    private int progressValue;
    private float changeValue;
	// Use this for initialization
	void Start () {
        StartCoroutine(change());
	}
	
	// Update is called once per frame
	void Update ()
    {

		if(ao.progress<0.9f && ao != null)
        {
            progressValue = (int)ao.progress;
        }
        else
        {
            progressValue = 100;
        }

        if (changeValue < progressValue)
            changeValue++;

        if (changeValue == 100)
        {
            if(Input.anyKeyDown)
                ao.allowSceneActivation = true;

            textColor = (byte)Mathf.Lerp(textColor, 0, 0.03f);
            text.color = new Color32(255,255,255, textColor);

            if (textColor <= 1)
                textColor = 255;

            txt = "按任意键开始游戏";


        }
        else
        {
            txt = changeValue + "%";
        }

        line.fillAmount = changeValue/100;

        text.text = txt;

        
	}

    IEnumerator change()
    {
        ao = SceneManager.LoadSceneAsync("Game");
        ao.allowSceneActivation = false;
        yield return ao;
    }
}
