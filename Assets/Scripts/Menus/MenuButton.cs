using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private MenuButtonController menuButtonController;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorFunctions animatorFunctions;
    [SerializeField] private int thisIndex;

    private int index;

    void Update()
    {
        index = menuButtonController.index;

        if(index == thisIndex)
        {
            animator.SetBool("selected", true);
            
            if(Input.GetAxis("Submit") == 1)
                animator.SetBool("clicked", true);
            else if(animator.GetBool("clicked"))
            {
                animator.SetBool("clicked", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }

        CheckLoadAndQuit();
    }

    private void CheckLoadAndQuit()
    {
                if (index == 0 && thisIndex == 0 && Input.GetButtonDown("Submit"))
        {
            Debug.Log("I Load!");
            SceneManager.LoadScene(1);
        }
        else if (index == 4 && thisIndex == 4 && Input.GetButtonDown("Submit"))
        {
            Debug.Log("I Quit!");
            Application.Quit();
        }
    }
}
