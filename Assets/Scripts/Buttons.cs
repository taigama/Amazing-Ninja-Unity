using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {

    public GameObject pnStart;
    public GameObject pnRestart;
    public GameObject pnGuide;

    public Sprite sprSoundOn;
    public Sprite sprSoundOff;

    public Image img;
	
    public void ClickStart()
    {
        pnGuide.SetActive(true);
        pnStart.SetActive(false);
        GameController.SoundStart();
    }

    public void ClickRestart()
    {
        GameController.Restart();
        pnRestart.SetActive(false);
        pnGuide.SetActive(true);
        GameController.SoundStart();
    }

    public void ClickHome()
    {
        GameController.Restart();
        pnRestart.SetActive(false);
        pnStart.SetActive(true);
    }
   
    public void SoundToggle()
    {
        var sound = ShakeCamera.getSound();
        var btnSound = GetComponent<Button>();

        

        if (sound.enabled)
        {
            sound.enabled = false;
            img.sprite = sprSoundOff;
        }
        else
        {
            sound.enabled = true;
            img.sprite = sprSoundOn;
        }

    }
}