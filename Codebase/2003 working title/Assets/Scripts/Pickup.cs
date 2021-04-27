using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pickup : MonoBehaviour
{
    [SerializeField] public bool DoubleJump = false, Dash = false, Walljump = false, Shield = false, WrenchThrow = false, Health = false, Key = false;
    [SerializeField] string PowerUpType = null;
    bool pickedup = false;
    [SerializeField] AudioManager PS = null;

    // // // // //
    //  This is where upgrades are handled...
    // // // // //

    void Start()
    {
        if (PlayerPrefs.GetInt(PowerUpType) == 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // this is where it finds  the player that collides with the upgrade
        if(collision.CompareTag("Player") && !pickedup)
        {
           PlayerController2D player = collision.GetComponent<PlayerController2D>();

            if (DoubleJump)
            {
                player._hasDoubleJump = true;
                PlayerPrefs.SetString(PowerUpType, PowerUpType);
                PS.Play("UP_PickedUp");
                FindObjectOfType<UIManagement>().UpgradePopUpWindow(0);
                PlayerPrefs.SetInt("DoubleJump", 1);
                PlayerPrefs.Save();
            }
            if (Dash)
            {
                player._hasDash = true;
                PlayerPrefs.SetString(PowerUpType, PowerUpType);
                PS.Play("UP_PickedUp");
                FindObjectOfType<UIManagement>().UpgradePopUpWindow(1);
                PlayerPrefs.SetInt("Dash", 1);
                PlayerPrefs.Save();
            }
            if (Walljump)
            {
                player._hasWallJump = true;
                PlayerPrefs.SetString(PowerUpType, PowerUpType);
                PS.Play("UP_PickedUp");
                FindObjectOfType<UIManagement>().UpgradePopUpWindow(2);
                PlayerPrefs.SetInt("WallJump", 1);
                PlayerPrefs.Save();
            }
            if (Shield)
            {
                PlayerPrefs.SetString(PowerUpType, PowerUpType);
                PS.Play("UP_PickedUp");
                player.ActivateShield();
                PlayerPrefs.SetInt("Shield", 1);
                PlayerPrefs.Save();
                FindObjectOfType<UIManagement>().UpgradePopUpWindow(3);
            }
            if (WrenchThrow)
            {
                player._hasWrenchThrow = true;
                PS.Play("UP_PickedUp");
                PlayerPrefs.SetString(PowerUpType, PowerUpType);
                FindObjectOfType<UIManagement>().UpgradePopUpWindow(4);
                PlayerPrefs.SetInt("WrenchThrow", 1);
                PlayerPrefs.Save();
            }
            if (!Health && !Key)
            {
                PS.Play("UP_PickedUp");
            }
            if (Key)
            {
                FindObjectOfType<KeyTracker>().KeyPickedUp();
                PlayerPrefs.SetInt(PowerUpType, 1);
                PS.Play("CK_PickedUp");
            }
            if (Health)
            {
                player.CurrentHealth = player.GetMaxHealth();
                PS.Play("P_Heal");
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ParticleSystem>().Play();
            pickedup = true;
            Destroy(this, GetComponent<ParticleSystem>().main.duration);
            //yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
            //GameObject.Destroy(gameObject);

        }
    }

}
