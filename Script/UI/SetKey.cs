using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetKey : MonoBehaviour
{

    public Text[] text;

    [HideInInspector]
    public Text[] inputValue;

    private Button[] allButton;

    private int theSelect;

    private bool changeText;

    private byte textColor = 255;

    private void Awake()
    {
        allButton = new Button[text.Length];
        inputValue = new Text[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            allButton[i] = text[i].GetComponentInChildren<Button>();
            inputValue[i] = allButton[i].GetComponentInChildren<Text>();
            switch (text[i].text)
            {
                case "拔刀":
                    inputValue[i].text = "" + KeyCode.Q.ToString();
                    break;

                case "互动":
                    inputValue[i].text = "" + KeyCode.E.ToString();
                    break;

                case "行走":
                    inputValue[i].text = "" + KeyCode.LeftControl.ToString();
                    break;

                case "奔跑":
                    inputValue[i].text = "" + KeyCode.LeftShift.ToString();
                    break;

                case "轻攻击":
                    inputValue[i].text = "" + KeyCode.Mouse0.ToString();
                    break;

                case "重攻击":
                    inputValue[i].text = "" + KeyCode.Mouse1.ToString();
                    break;

                case "锁定":
                    inputValue[i].text = "" + KeyCode.R.ToString();
                    break;

                case "前进":
                    inputValue[i].text = "" + KeyCode.W.ToString();
                    break;

                case "后退":
                    inputValue[i].text = "" + KeyCode.S.ToString();
                    break;

                case "向左":
                    inputValue[i].text = "" + KeyCode.A.ToString();
                    break;

                case "向右":
                    inputValue[i].text = "" + KeyCode.D.ToString();
                    break;

                case "跳舞":
                    inputValue[i].text = "" + KeyCode.T.ToString();
                    break;
            }
        }


    }

    void Update()
    {
        if (changeText)
        {
            inputValue[theSelect].text = "待输入";


            textColor = (byte)Mathf.Lerp(textColor, 0, 0.03f);
            inputValue[theSelect].color = new Color32(0, 0, 0, textColor);

            if (textColor <= 1)
                textColor = 255;

            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        inputValue[theSelect].color = new Color32(0, 0, 0, 255);
                        inputValue[theSelect].text = "" + keyCode.ToString();
                        foreach (Text t in inputValue)
                        {
                            if (t != inputValue[theSelect] && t.text == inputValue[theSelect].text)
                            {
                                t.text = "";
                            }
                        }
                        changeText = false;
                        break;
                    }
                }
            }
        }
    }

    public void changeKey()
    {
        Button b = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        for (int j = 0; j < allButton.Length; j++)
        {
            if (allButton[j] == b)
            {
                theSelect = j;
                changeText = true;
                break;
            }
        }

    }

}
