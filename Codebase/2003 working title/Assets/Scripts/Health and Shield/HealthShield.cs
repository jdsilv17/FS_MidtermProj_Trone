using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthShield : MonoBehaviour
{
    [SerializeField] private Image BarImage = null, HealthBar = null, HealthIcon = null, ShieldBar = null, ShieldIcon = null;
    [SerializeField] private Gradient HealthGrad = null, ShieldGrad = null;
    [SerializeField] private Sprite[] Sprites = null;
    PlayerController2D player = null;
    float maxHealth = 0, maxShield = 0;
    float healthPer = 0, shieldPer = 0;
    private void Start()
    {
        player = FindObjectOfType<PlayerController2D>();
        maxHealth = player.GetMaxHealth();
        maxShield = player.GetMaxShield();
    }
    // Update is called once per frame
    void Update()
    {

        if (player._hasShield)
        {
            if(BarImage.sprite != Sprites[1])
            {
                BarImage.sprite = Sprites[1];
            }
            shieldPer = player.CurrentShield / maxShield;
            ShieldBar.fillAmount = shieldPer;
            ShieldBar.color = ShieldGrad.Evaluate(shieldPer);
            ShieldIcon.color = ShieldGrad.Evaluate(shieldPer);
        }

        healthPer = player.CurrentHealth / maxHealth;
        HealthBar.fillAmount = healthPer;
        HealthBar.color = HealthGrad.Evaluate(healthPer);
        HealthIcon.color = HealthGrad.Evaluate(healthPer);
    }
    public void GotShield()
    {
        BarImage.sprite = Sprites[1];
        ShieldBar.enabled = true;
        ShieldIcon.enabled = true;
    }
    public void GotWrench()
    {
        BarImage.sprite = Sprites[2];
    }
}
