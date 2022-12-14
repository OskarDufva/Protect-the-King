using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeScript : MonoBehaviour
{
    public Animator transition;
    public Animator loadingAnim;
    public float transitionTime = 5f;
    string currentScene;

    void Start()
    {
        currentScene = (SceneManager.GetActiveScene()).name;

        if (currentScene == "TutorialScene")
        {
            transition.SetTrigger("Start");
            transition.SetTrigger("stopLoad");
        }
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTutorial()
    {
        StartCoroutine(LoadLevel("TutorialScene"));
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene("CampaignScene");
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("FadeIn");
        loadingAnim.SetTrigger("startLoad");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}