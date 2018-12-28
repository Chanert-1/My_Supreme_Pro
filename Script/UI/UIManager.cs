using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    private AudioSource au;

    private SetKey sk;

    private AudioSource Childau;

    public AudioClip buttonSound;

    public AudioClip lelo;

    public AudioClip zexue;

    public GameObject[] UI;

    public Transform[] Position;

    public Camera cam;

    private bool toFirst;

    private bool toSecond;

    private bool toThird;

    private bool toFourth;

    private bool musicOn = true;

    private bool buttonSoundOn = true;

    public GameObject playDoor;

    private bool doorOpen;

    private void Awake()
    {
        UI[0].SetActive(true);
        for (int i = 1; i < UI.Length; i++)
        {
            UI[i].SetActive(false);
        }

        au = GetComponent<AudioSource>();

        Childau = transform.Find("BackgoundMusic").gameObject.GetComponent<AudioSource>();

        sk = this.GetComponent<SetKey>();
    }

    private void Update()
    {

        toWhich(toFirst, 1, 2, 3, 0, 0);

        toWhich(toSecond, 0, 2, 3, 1, 1);

        toWhich(toThird, 1, 0, 3, 2, 2);

        toWhich(toFourth, 1, 0, 2, 3, 3);




        if (doorOpen && playDoor != null)
        {
            playDoor.transform.rotation =
                Quaternion.Lerp(playDoor.transform.rotation, Quaternion.Euler(0, 90, 0), 0.05f);
            UI[1].SetActive(false);
            Invoke("gotoPlay", 1f);
        }

    }

    void gotoPlay()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, Position[4].position, 0.25f);

        if (Vector3.Distance(cam.transform.position, Position[4].position) < 1f)
            SceneManager.LoadScene("LoadGame");
    }

    private void toWhich(bool toWhich, int other1, int other2, int other3, int which, int Po)
    {

        if (toWhich)
        {
            if (UI[which].activeSelf == false)
            {
                UI[other1].SetActive(false);
                UI[other2].SetActive(false);
                UI[other3].SetActive(false);
                UI[which].SetActive(true);
            }

            cam.transform.position = Vector3.Lerp(cam.transform.position, Position[Po].position, 0.03f);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Position[Po].rotation, 0.05f);
        }

    }

    //bug方法
    private void changeToWhich(bool other1, bool other2, bool toWhich)
    {
        if (buttonSoundOn)
            au.PlayOneShot(buttonSound);

        other1 = false;
        other2 = false;
        toWhich = true;
    }

    //按钮方法
    public void changeToFirst()
    {
        if (buttonSoundOn)
            au.PlayOneShot(buttonSound);

        toSecond = false;
        toThird = false;
        toFourth = false;
        toFirst = true;
    }


    public void changeToSecond()
    {
        if (buttonSoundOn)
            au.PlayOneShot(buttonSound);

        toFourth = false;
        toFirst = false;
        toThird = false;
        toSecond = true;

    }

    public void changeToThird()
    {
        if (buttonSoundOn)
            au.PlayOneShot(buttonSound);

        toFourth = false;
        toFirst = false;
        toSecond = false;
        toThird = true;

    }

    public void changeToFourth()
    {
        if (buttonSoundOn)
            au.PlayOneShot(buttonSound);

        toFirst = false;
        toSecond = false;
        toThird = false;
        toFourth = true;
    }



    public void MusicOnOff()
    {
        musicOn = !musicOn;
        if (musicOn)
            Childau.UnPause();
        else
            Childau.Pause();
    }

    public void MusicSound(float number)
    {
        Childau.volume = number;
    }

    public void buttonSoundOnOff()
    {
        buttonSoundOn = !buttonSoundOn;
    }

    public void startGame()
    {
        PlayInput.keyName = new string[sk.inputValue.Length];
        for (int i = 0; i < sk.inputValue.Length; i++)
        {
            PlayInput.keyName[i] = sk.inputValue[i].text.ToLower();
        }

        
        doorOpen = true;
    }
}
