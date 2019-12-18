using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider TimeSlider;
    [SerializeField] private TextMeshProUGUI timeScaleText;
    public void ChangeTimeSpeed()
    {
        if (TimeSlider == null || timeScaleText == null) return;

        Time.timeScale = TimeSlider.value;
        timeScaleText.text = TimeSlider.value.ToString("F1") + "x";
    }

    [SerializeField] private TextMeshProUGUI genText;
    public void UpdateGenCounter(int gen)
    {
        genText.text = "generation " + gen;
    }

    public GameObject creatorPanel;

    public void DoneCreating()
    {
        creatorPanel.SetActive(false);
    }

}
