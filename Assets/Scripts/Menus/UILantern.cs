using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILantern : MonoBehaviour
{
    [SerializeField] private List<GameObject> _allGhosts 
    = new List<GameObject>();
    [SerializeField] private List<GameObject> _allAbilities 
    = new List<GameObject>();
    
    [Header("Lantern")]
    [SerializeField] private Lantern.LanternThrow _lantern;

    private (GhostColor, GhostColor) _colorsInLantern;
    private GhostColor _ghostColor1;
    private GhostColor _ghostColor2;
    private bool _isEmpty;
    private bool _halfEmpty;

    //Ghost Combinations
    private (GhostColor, GhostColor) _RR;
    private (GhostColor, GhostColor) _RG;
    private (GhostColor, GhostColor) _RB;
    private (GhostColor, GhostColor) _GG;
    private (GhostColor, GhostColor) _GR;
    private (GhostColor, GhostColor) _GB;
    private (GhostColor, GhostColor) _BB;
    private (GhostColor, GhostColor) _BR;
    private (GhostColor, GhostColor) _BG;
    private (GhostColor, GhostColor) _NONE;

    //////////

    private void Start()
    {
        _isEmpty    = true;
        _halfEmpty  = true;

        _colorsInLantern = (GhostColor.None, GhostColor.None);
        _ghostColor1 = GhostColor.None;
        _ghostColor2 = GhostColor.None;

        SetUpAbilities();
        ResetAbilityIcons();
    }

    void Update()
    {
        Debug.Log("color down " + _colorsInLantern.Item1);
        Debug.Log("ghostcolor1 " + _ghostColor1);
        Debug.Log("color up " + _colorsInLantern.Item2);
        Debug.Log("ghostcolor2 " + _ghostColor2);
        Debug.Log("lantern state " + _isEmpty);
        Debug.Log(" ");

        _colorsInLantern = _lantern.GetLaternColors();
        _ghostColor1 = _colorsInLantern.Item1;
        _ghostColor2 = _colorsInLantern.Item2;

        LanternState();
        CheckForGhosts();
        CheckForAbility();
    }

    //////////

    private void SetUpAbilities()
    {
        _RR = (GhostColor.Red, GhostColor.Red);
        _RG = (GhostColor.Red, GhostColor.Green);
        _RB = (GhostColor.Red, GhostColor.Blue);
        _GG = (GhostColor.Green, GhostColor.Green);
        _GR = (GhostColor.Green, GhostColor.Red);
        _GB = (GhostColor.Green, GhostColor.Blue);
        _BB = (GhostColor.Blue, GhostColor.Blue);
        _BR = (GhostColor.Blue, GhostColor.Red);
        _BG = (GhostColor.Blue, GhostColor.Green);
        _NONE = (GhostColor.None, GhostColor.None);
    }

    private void ResetAbilityIcons()
    {
        _allAbilities[0].SetActive(false);
        _allAbilities[1].SetActive(false);
        _allAbilities[2].SetActive(false);
        _allAbilities[3].SetActive(false);
        _allAbilities[4].SetActive(false);
        _allAbilities[5].SetActive(false);
    }

    private void CheckForAbility()
    {
        if(_colorsInLantern == _NONE)
        {
            ResetAbilityIcons();
        }

        if(_colorsInLantern == _RR)
        {
            _allAbilities[0].SetActive(true);
            _allAbilities[1].SetActive(false);
            _allAbilities[2].SetActive(false);
            _allAbilities[3].SetActive(false);
            _allAbilities[4].SetActive(false);
            _allAbilities[5].SetActive(false);
        }
        else if(_colorsInLantern == _GG)
        {
            _allAbilities[0].SetActive(false);
            _allAbilities[1].SetActive(true);
            _allAbilities[2].SetActive(false);
            _allAbilities[3].SetActive(false);
            _allAbilities[4].SetActive(false);
            _allAbilities[5].SetActive(false);
        }
        else if(_colorsInLantern == _BB)
        {
            _allAbilities[0].SetActive(false);
            _allAbilities[1].SetActive(false);
            _allAbilities[2].SetActive(true);
            _allAbilities[3].SetActive(false);
            _allAbilities[4].SetActive(false);
            _allAbilities[5].SetActive(false);
        }
        else if(_colorsInLantern == _RG || _colorsInLantern == _GR)
        {
            _allAbilities[0].SetActive(false);
            _allAbilities[1].SetActive(false);
            _allAbilities[2].SetActive(false);
            _allAbilities[3].SetActive(true);
            _allAbilities[4].SetActive(false);
            _allAbilities[5].SetActive(false);
        }
        else if(_colorsInLantern == _RB || _colorsInLantern == _BR)
        {
            _allAbilities[0].SetActive(false);
            _allAbilities[1].SetActive(false);
            _allAbilities[2].SetActive(false);
            _allAbilities[3].SetActive(false);
            _allAbilities[4].SetActive(true);
            _allAbilities[5].SetActive(false);            
        }
        else if(_colorsInLantern == _GB || _colorsInLantern == _BG)
        {
            _allAbilities[0].SetActive(false);
            _allAbilities[1].SetActive(false);
            _allAbilities[2].SetActive(false);
            _allAbilities[3].SetActive(false);
            _allAbilities[4].SetActive(false);
            _allAbilities[5].SetActive(true);             
        }
    }

    private void LanternState()
    {
        if (_colorsInLantern.Item1 == GhostColor.None &&
            _colorsInLantern.Item2 == GhostColor.None)
        {
            _isEmpty    = true;
            _halfEmpty  = false;

            _ghostColor1 = GhostColor.None;
            _ghostColor2 = GhostColor.None;
        }
        else if (_colorsInLantern.Item1 != GhostColor.None &&
                _colorsInLantern.Item2 == GhostColor.None)
        {
            _isEmpty    = false;
            _halfEmpty  = true;

            _ghostColor2 = GhostColor.None;
        }
        else if (_colorsInLantern.Item1 != GhostColor.None &&
                _colorsInLantern.Item2 != GhostColor.None)
        {
            _isEmpty    = false;
            _halfEmpty  = false;
        }
    }

    private void CheckForGhosts()
    {
        //Checking for bottom slot RGB ([0-2])
        if(_isEmpty == false && _ghostColor1 != GhostColor.None)
        {
            if (_ghostColor1 == GhostColor.Red)
            {
                _allGhosts[0].SetActive(true);

                _allGhosts[1].SetActive(false);
                _allGhosts[2].SetActive(false);
            }
            else if (_ghostColor1 == GhostColor.Green)
            {
                _allGhosts[1].SetActive(true);

                _allGhosts[0].SetActive(false);
                _allGhosts[2].SetActive(false);
            }
            else if (_ghostColor1 == GhostColor.Blue)
            {
                _allGhosts[2].SetActive(true);
                
                _allGhosts[0].SetActive(false);
                _allGhosts[1].SetActive(false);
            }
        }
        else if (_isEmpty == true)
        {
            _allGhosts[0].SetActive(false);
            _allGhosts[1].SetActive(false);
            _allGhosts[2].SetActive(false);
        }

        //Checking for bottom slot RGB ([3-5])
        if(_isEmpty == false && _ghostColor2 != GhostColor.None)
        {
            if (_ghostColor2 == GhostColor.Red)
            {
                _allGhosts[3].SetActive(true);

                _allGhosts[4].SetActive(false);
                _allGhosts[5].SetActive(false);
            }
            else if (_ghostColor2 == GhostColor.Green)
            {
                _allGhosts[4].SetActive(true);

                _allGhosts[3].SetActive(false);
                _allGhosts[5].SetActive(false);
            }
            else if (_ghostColor2 == GhostColor.Blue)
            {
                _allGhosts[5].SetActive(true);

                _allGhosts[3].SetActive(false);
                _allGhosts[4].SetActive(false);
            }
        }
        else if (_isEmpty == true)
        {
            _allGhosts[3].SetActive(false);
            _allGhosts[4].SetActive(false);
            _allGhosts[5].SetActive(false);
        }
    }
}
