using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private MenuButtonController   _menuButtonController;
    [SerializeField] private Animator               _animator;
    [SerializeField] private AnimatorFunctions      _animatorFunctions;
    [SerializeField] private int thisIndex;

    private int index;

    void Update()
    {
        index = _menuButtonController.index;

        if(index == thisIndex)
        {
            _animator.SetBool("selected", true);
            
            if(Input.GetAxis("Submit") == 1)
                _animator.SetBool("clicked", true);
            else if(_animator.GetBool("clicked"))
            {
                _animator.SetBool("clicked", false);
                _animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            _animator.SetBool("selected", false);
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
