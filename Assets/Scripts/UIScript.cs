using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TapticPlugin;

public class UIScript : MonoBehaviour
{
    public GameObject FailedPanel;
    public GameObject GamePanel;
    public GameObject NextPanel;
    public GameObject Character;
    public Slider BarSlider;
    public Text LevelText;
    public Text NextLevelText;
    public Text MaxKoltuk;
    public Text DoluKoltuk;
    int dolukoltukTampon;
    public GameObject Tutorial;

    void Start()
    {
        LevelText.text = PlayerPrefs.GetInt("LevelIndex").ToString();
        NextLevelText.text = (PlayerPrefs.GetInt("LevelIndex")+1).ToString();
        MaxKoltuk.text = Character.transform.GetComponent<CharacterScript>().maxKoltuk.ToString();
        dolukoltukTampon= Character.transform.GetComponent<CharacterScript>().DoluKoltuk;
        BarSlider.maxValue = Character.transform.GetComponent<CharacterScript>().maxKoltuk;
        Application.targetFrameRate = 60;
    }
    public void tutorialOpener()
    {
        Tutorial.SetActive(true);
        Tutorial.transform.GetChild(0).gameObject.SetActive(false);
        Tutorial.transform.GetChild(1).gameObject.SetActive(false);
        Tutorial.transform.GetChild(2).gameObject.SetActive(false);
        Tutorial.transform.GetChild(3).gameObject.SetActive(false);
        Tutorial.transform.GetChild(4).gameObject.SetActive(false);
        Tutorial.transform.GetChild(5).gameObject.SetActive(false);
        Tutorial.transform.GetChild(6).gameObject.SetActive(false);
        Tutorial.transform.GetChild(7).gameObject.SetActive(true);
    }
    public void StartGame()
    {
        Character.transform.GetComponent<CharacterScript>().StartRunFunc();
    }
    public void LevelCompleted()
    {
        GamePanel.SetActive(false);
        FailedPanel.SetActive(false);
        NextPanel.SetActive(true);
    }
    public void LevelFailed()
    {
        GamePanel.SetActive(false);
        FailedPanel.SetActive(true);
        NextPanel.SetActive(false);
    }

    public void NextButton()
    {
        if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("LevelIndex") + 1);
        if (PlayerPrefs.GetInt("LevelIndex") > 10)
        {
            SceneManager.LoadScene("Level" + Random.Range(1, 10));
        }
        else
        {
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("LevelIndex"));
        }
    }
    public void RestartButton()
    {
        if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Tutorial.SetActive(false);
        }
        if (Character.transform.GetComponent<CharacterScript>().DoluKoltuk!=dolukoltukTampon)
        {
            transform.GetComponent<Animator>().SetTrigger("SeatPlus");
            dolukoltukTampon = Character.transform.GetComponent<CharacterScript>().DoluKoltuk;
        }
        DoluKoltuk.text = Character.transform.GetComponent<CharacterScript>().DoluKoltuk.ToString();
        BarSlider.value = Character.transform.GetComponent<CharacterScript>().DoluKoltuk;
    }
}
