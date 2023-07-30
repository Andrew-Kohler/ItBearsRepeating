using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    Slider slider;
    [SerializeField] TextMeshProUGUI bossName;
    [SerializeField] TextMeshProUGUI bossNameShadow;
    [SerializeField] TextMeshProUGUI bossSubheader;

    void Start()
    {
        slider = GetComponent<Slider>();
        bossName.text = "";
        bossNameShadow.text = "";
        bossSubheader.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(string bossName, string bossSubhead, float maxHealth)
    {
        this.bossName.text = bossName;
        this.bossNameShadow.text = bossName;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
}
