using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeScript : MonoBehaviour
{
    public void GoBackToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void StartTutorial()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene("CampaignScene");
    }
}
