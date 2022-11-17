using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour    
{
    public GameObject Panel;
    bool active;


    //Open and closes the shop with an animation
    public void OpenAndCloseShop()
    {


        if (Panel != null)
        {
            Animator animator = Panel.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }
    }

    //disables the panel if the game is active.
    public void OpenAndClosePanel()
    {
        if (active == false)
        {
            Panel.transform.gameObject.SetActive(true);
            active = true;
        }
        else
        {
            Panel.transform.gameObject.SetActive(false);
            active = false;
        }
    }
}
