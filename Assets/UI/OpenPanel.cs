using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour    
{
    public GameObject Panel;
    bool active;


    public void OpenAndClose()
    {
        //if(active == false)
        //{
        //    Panel.transform.gameObject.SetActive(true);
        //    active = true;
        //}
        //else
        //{
        //    Panel.transform.gameObject.SetActive(false);
        //    active = false; 
        //}

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
}
