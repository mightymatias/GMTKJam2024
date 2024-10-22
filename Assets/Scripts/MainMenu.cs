using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string MainScene;

    public GameObject optionsScreen;
    public GameObject creditsScreen;
    public GameObject tutorialScreen;
    public GameObject introScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(MainScene);
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsScreen.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorialScreen.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialScreen.SetActive(false);
    }

    public void OpenIntro()
    {
        introScreen.SetActive(true);
    }

    public void CloseIntro()
    {
        introScreen.SetActive(false);
    }

}
