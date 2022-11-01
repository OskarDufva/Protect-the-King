using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignSceneScript : MonoBehaviour
{
    public void GoBackToGameModes()
    {
        SceneManager.LoadScene("GameModes");
    }
}
