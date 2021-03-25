using System.Collections;
using System.Collections.Generic;
//using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceeneloadscript : MonoBehaviour
{
    int currentSceeneIndex;
    void Start()
    {
        //if (!FB.IsInitialized)
        //    FB.Init(OnInitComplete);
        //else
        //    FB.ActivateApp();

        //GameAnalyticsSDK.GameAnalytics.Initialize();

        if (PlayerPrefs.HasKey("LevelIndex"))
        {
            if (PlayerPrefs.GetInt("LevelIndex") > 13)
            {
                SceneManager.LoadScene("Level" + Random.Range(4, 10));
            }
            else
            {

                SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("LevelIndex"));
            }
        }
        else
        {
            PlayerPrefs.SetInt("LevelIndex", 1);
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("LevelIndex"));
        }
    }

    //void OnInitComplete()
    //{
    //    if (FB.IsInitialized)
    //    {
    //        FB.ActivateApp();
    //        FB.LogAppEvent(AppEventName.ActivatedApp);
    //    }
    //    else
    //        Debug.Log("OnInitComplete Failed");
    //}
}
