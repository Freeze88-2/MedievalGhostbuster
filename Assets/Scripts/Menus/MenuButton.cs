﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private MenuButtonController   _menuButtonController;
    [SerializeField] private Animator               _animator;
    [SerializeField] private AnimatorFunctions      _animatorFunctions;
    [SerializeField] private GameObject             _mainMenu;
    [SerializeField] private GameObject             _credits;
    [SerializeField] private int thisIndex;

    private int index;
    
    public bool showCredits;
    public bool gameIsLoaded;

    private void Start()
    {
        gameIsLoaded = false;  
        showCredits = false; 

        _credits.SetActive(false);
        _mainMenu.SetActive(true);
    }

    private void Update()
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
        ShowCredits();
        LoadSave();
    }

    private void CheckLoadAndQuit()
    {
        if (index == 0 && thisIndex == 0 && Input.GetButtonDown("Submit"))
        {
            //Debug.Log("I Load!");
            SceneManager.LoadScene(1);
        }
        else if (index == 3 && thisIndex == 3 && Input.GetButtonDown("Submit"))
        {
            //Debug.Log("I Quit!");
            Application.Quit();
        }
    }

    private void ShowCredits()
    {
        if (index == 2 && thisIndex == 2 && Input.GetButtonDown("Submit")
        && showCredits == false)
        {
            showCredits = true;
            _mainMenu.SetActive(false);
            _credits.SetActive(true);
        }
        
        if (showCredits == true)
        {
            _mainMenu.SetActive(false);
            _credits.SetActive(true);
        }

    }

    private void LoadSave()
    {
        if (index == 1 && thisIndex == 1 && Input.GetButtonDown("Submit"))
        {
            transform.parent = null;
            DontDestroyOnLoad(this);
            gameIsLoaded = true;
            SceneManager.LoadScene(1);
        }
    }

    public void NewShowCredits(bool b)
    {
        showCredits = b;
    }
}
