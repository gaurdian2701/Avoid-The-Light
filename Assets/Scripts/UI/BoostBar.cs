using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField] private Image boostBar;
    [SerializeField] private float boostFillRate;

    public static Action<bool> BoostEnabled;

    private void Awake()
    {
        PlayerController.FillBar += HandleBoostBar;
    }

    private void OnDestroy()
    {
        PlayerController.FillBar -= HandleBoostBar;
    }
    private void HandleBoostBar()
    {
        boostFillRate *= -1f;
        InitiateBoostBarFill();
    }

    private void InitiateBoostBarFill()
    {
        CancelInvoke(nameof(FillBoostBar));
        InvokeRepeating(nameof(FillBoostBar), 0.1f, 0.1f);
    }
    private void FillBoostBar()
    {
        if(boostBar.fillAmount >= 0f &&  boostBar.fillAmount <= 1f)
            boostBar.fillAmount += boostFillRate;

        PlayerBoostControl();
    }

    private void PlayerBoostControl()
    {
        if (CheckBoostBarIsEmpty())
        {
            BoostEnabled.Invoke(false);
            CancelInvoke(nameof(FillBoostBar));
            Invoke(nameof(EnablePlayerBoost), 0.2f);
        }
    }
    private void EnablePlayerBoost() => BoostEnabled.Invoke(true);
    private bool CheckBoostBarIsEmpty()
    {
        if (boostBar.fillAmount <= 0f)
            return true;

        return false;
    }
}
