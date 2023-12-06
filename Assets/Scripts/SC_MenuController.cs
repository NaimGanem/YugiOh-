using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SC_MenuLogic;

public class SC_MenuController : MonoBehaviour
{
    public SC_MenuLogic curMenuLogic;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private AudioSource BGaudio;


    private void Start()
    {
        BGaudio.volume = 1;
        ChangeVolume();
    }
    #region volume
    public void ChangeVolume()
    {
        volumeText.text = musicSlider.value.ToString("F1");
        BGaudio.volume = musicSlider.value;
    }
    #endregion

    public void Btn_Play()
    {
        SC_MenuLogic.Instance.Btn_PlayLogic();
    }

    public void Btn_BackLogic()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_BackLogic();
    }
    
   
    public void Btn_PlayLogic()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_PlayLogic();
    }

    public void Btn_ChangeScreen(string _ScreenName)
    {
        if (curMenuLogic != null)
        {
            try
            {
                Screens _toScreen = (Screens)Enum.Parse(typeof(Screens), _ScreenName);
                curMenuLogic.ChangeScreen(_toScreen);
            }
            catch (Exception e)
            {
                Debug.LogError("Fail to convert: " + e.ToString());
            }
        }
    }

    public void Controller_Cv()
    {
        if (curMenuLogic != null)
        {
            curMenuLogic.Cv();
        }
    }

}
