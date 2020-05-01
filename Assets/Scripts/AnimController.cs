using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator            _characterAnim;
    private PlayerMovement      _player;

    //private int                 _chainAttacksAnim;

    // Start is called before the first frame update
    private void Start()
    {
        _characterAnim = GetComponentInChildren<Animator>();
        _player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        AnimChangeOnWalk();
        AnimChangeOnAttack();
    }

    private void AnimChangeOnWalk()
    {
        if (Input.GetAxis("Horizontal") > 0
                || Input.GetAxis("Horizontal") < 0)
        {
            _characterAnim.Play("Male_Sword_Walk");
        }
        else if (Input.GetAxis("Vertical") > 0
                || Input.GetAxis("Vertical") < 0)
        {
            _characterAnim.Play("Male_Sword_Walk");
        }
    }

    private void AnimChangeOnAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _characterAnim.SetTrigger("IsAttack");
        }
    }
}
