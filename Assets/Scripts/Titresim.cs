using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Titresim : MonoBehaviour
{
    public Sprite openVibration;
    public Sprite closeVibration;
    public GameObject vibrationButton;
    void Start()
    {
        if (PlayerPrefs.HasKey("onOrOffVibration"))
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 0)
            {
                vibrationButton.GetComponent<Image>().sprite = closeVibration;
            }
            else if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
            {
                vibrationButton.GetComponent<Image>().sprite = openVibration;
            }
        }
        else
        {
            vibrationButton.GetComponent<Image>().sprite = openVibration;
            PlayerPrefs.SetInt("onOrOffVibration", 1);
        }
    }

    public void vibrationButtonFunc()
    {
        if (PlayerPrefs.GetInt("onOrOffVibration") == 0)
        {
            Debug.Log("titresim acık");
            vibrationButton.GetComponent<Image>().sprite = openVibration;
            PlayerPrefs.SetInt("onOrOffVibration", 1);
        }
        else if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
        {
            Debug.Log("titresim kapalı");
            vibrationButton.GetComponent<Image>().sprite = closeVibration;
            PlayerPrefs.SetInt("onOrOffVibration", 0);
        }
    }


}
