using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtManager : MonoBehaviour
{
    public float _maxHealth = 150;
    public float _maxMana = 2000;
    public float _currentHealth;
    public float _currentMana;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private Image _manaBarFill;
    [SerializeField] private Image _Character;
    public static HealtManager Instance;


    public void Awake()
    {
        Instance = this;
        _currentHealth = _maxHealth;
        _currentMana = _maxMana;
        UpdateHealthBar(); // Friss�tj�k a hp s�vot az indul�skor
    }

    public void UpdateHealth(float dmgamount)
    {
        _currentHealth -= dmgamount / 400;

        // Ellen�rizz�k, hogy nincs-e t�l alacsonyra cs�kkentve a hp, �s ha igen, akkor v�grehajtunk valamilyen tov�bbi tev�kenys�get, p�ld�ul meghalunk
        if (_currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / _maxHealth;
        _healthBarFill.fillAmount = targetFillAmount;
        Debug.Log(_maxHealth);
        Debug.Log(_maxMana);
    }

    private void Die()
    {
        BaseHero currentHero = UnitManager.Instance.GetSelectedHero();
        currentHero.transform.Rotate(Vector3.forward * 90);
        StartCoroutine(DisableInputForSeconds(0.1f));
    }

    private IEnumerator DisableInputForSeconds(float seconds)
    {
        // Billenty�zet Input kikapcsol�sa
        while (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(seconds);
        }
    }



    public void UpdateMana(float useamount)
    {
        _currentMana -= useamount / 400;

        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        float targetFillAmount = _currentMana / _maxMana;
        _manaBarFill.fillAmount = targetFillAmount;
    }

    public void IncreaseMaxHealth(float amount)
    {
        _maxHealth += amount;
    }

    public void IncreaseMaxMana(float amount)
    {
        _maxMana += amount;
    }
}
