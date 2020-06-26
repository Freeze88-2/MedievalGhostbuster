using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private DummyPlayer    _player;
    [SerializeField] private Image          _healthBar;

    private float _currentHealth;

    void Update()
    {
        _currentHealth = _player.Hp / 100;

        CheckHealth();
    }

    private void CheckHealth()
    {
        _healthBar.fillAmount =
        Mathf.Lerp(_healthBar.fillAmount, _currentHealth, Time.fixedDeltaTime);
    }

}
