using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvLManager : MonoBehaviour
{
    private float maxXp = 1000;
    private float currentXp;
    private int currentRank;
    [SerializeField] private Image xpBar;
    public static LvLManager Instance;
    public Text textrank;


    public void Awake()
    {
        Instance = this;
        currentXp = 0;
        currentRank = 1;
        if (xpBar == null)
        {
            Debug.LogError("xpBar referencia hiányzik!");
        }
        UpdateLvLBar();
    }

    public void UpdateLvL(float addxp)
    {
        currentXp += addxp / 400;


        if (currentXp >= 1000)
        {
            RankUp();
        }

        UpdateLvLBar();
    }

    private void UpdateLvLBar()
    {
        float targetFillAmount = currentXp / maxXp;
        xpBar.fillAmount = targetFillAmount;
        if (textrank == null)
        {
            Debug.Log("Text Null");
        }
        else
        {
            textrank.text = currentRank.ToString();
        }
    }

    private void RankUp()
    {
        Debug.Log("GG");
        currentXp = 0;
        currentRank++;
        HealtManager.Instance.IncreaseMaxHealth(20);
        HealtManager.Instance.IncreaseMaxMana(20);
    }

}
