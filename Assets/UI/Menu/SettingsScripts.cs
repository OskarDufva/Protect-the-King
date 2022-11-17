using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; //Need to use Unitys audio system to control the audio
using UnityEngine.UI; //Need to use Unitys UI system because dropdowns specifically the resolution dropdown is a UI element

public class SettingsScripts : MonoBehaviour
{
    public AudioMixer audioMixer; //Needs to connect to the audio mixer in order to control all sound.

    public TMPro.TMP_Dropdown resolutionDropdown; //Needs to connect to the dropdown for resolutions in order to add all the available resolutions to the dropdown

    Resolution[] resolutions; //variable for the resolutions array

    void Start ()
    {
        resolutions = Screen.resolutions; // needs an array of resolutions 
        resolutionDropdown.ClearOptions(); // will clear the original options so that we can replace them with the screens resolutions

        List<string> options = new List<string>(); // need to turn the resolutions into strings

        int currentResolutionIndex = 0; // there so that we can know later on which will be the correct resolution
        for (int i = 0; i < resolutions.Length; i++) // will add the length of all possible resolutions to the list of resolutions
        {
            string option = resolutions[i].width + "x" + resolutions[i].height; // will name the given resolution to it's height x width
            options.Add(option); // will make an option in the options list

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) // will decide which is the correct resolution for the screen
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options); // will add the optionslist to the resolutionslist
        resolutionDropdown.value = currentResolutionIndex; // will make the correct resolution the resolution that the if function in the for loop decided
        resolutionDropdown.RefreshShownValue(); // will update the shown value on the dropdown to the correct resolution
    }

    public void SetResolution (int resolutionIndex) // has to be public so that we can call it from the Dropdown
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    //will add your screens available resolutions to the dropdown and change resolutions for the game depending on which resolution you chose

    public void SetVolume (float Volume) // has to be public so that we can call it from the slider
    {
        audioMixer.SetFloat("Volume", Volume); // will update the audio mixers audio depending on how full the volume slider is
    }
    //this function will change the games AudioMixer when you drag in the slider

    public void SetQuality (int qualityIndex) // has to be public so that we can call it from the Dropdown, needs also to be an int so that we can easily just change it between low/medium/high
    {
        QualitySettings.SetQualityLevel(qualityIndex); // will set the games quality to what you picked
    }
    //this function will update the quality for the game when you 

    public void SetFullscreen (bool isFullscreen) // has to be public so that we can call it from the toggle
    {
        Screen.fullScreen = isFullscreen;
    }
    //this function will toggle between having the game in Fullscreen or not when you hit the button
}
