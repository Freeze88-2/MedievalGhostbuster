using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private Animator   GetAnimator;
    [SerializeField] private AudioClip  _attackSound;
    private Collider                    GetCollider;
    private List<IEntity>               _damagedGhosts;
    private AudioSource                 _audio;
    private float                       _timer;

    private void Start() 
    {
        _audio                          = GetComponent<AudioSource>();
        GetCollider                     = GetComponent<Collider>();
        GetAnimator                     = GetComponentInParent<Animator>();
        _timer                          = 0.0f;
        _damagedGhosts                  = new List<IEntity>();
    }

    private void Update() 
    {
        // GetAnimator.GetParameter(2);
        // Debug.Log(GetAnimator.GetParameter(2).name);
        // Debug.Log(GetAnimator.GetBool("Attack"));

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            if (Input.GetButtonDown("Fire1") && !GetCollider.enabled && _timer == 0.0f)
            {
                GetCollider.enabled = true;

                //PlaySound(_audio);

                //_damagedGhosts.Clear();
            }
        }

        if (GetCollider.enabled)
        {
            _timer += Time.deltaTime % 60;
        }
        if (_timer > 0.35f && GetCollider.enabled)
        {
            PlaySound(_audio);
            GetCollider.enabled = false;
            _timer = 0.0f;
            _damagedGhosts.Clear();    
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("GhostEnemy"))
        {
            IEntity bulliedGhost = other.GetComponent<IEntity>();
            
            if (!_damagedGhosts.Contains(bulliedGhost))
            {
                bulliedGhost.DealDamage(25.0f);
                _damagedGhosts.Add(bulliedGhost);
            }
        }
    }

    private void PlaySound(AudioSource _audio)
    {
        _audio.clip = _attackSound;
        _audio.volume = Random.Range(0.15f, 0.35f);
        _audio.Play();

    }
}
