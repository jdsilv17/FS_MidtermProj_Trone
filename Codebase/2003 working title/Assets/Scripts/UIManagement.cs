using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagement : MonoBehaviour
{
    [SerializeField] PlayerController2D Player = null;
    [SerializeField] Canvas PlayerHud = null, PauseHud = null, Settings = null, DeathHud = null, CurrHud = null, UpgradeHud = null, AutoSave = null, Map = null;
    [SerializeField] Animator deathAnim = null;
    [SerializeField] bool _Paused = false;
    [SerializeField] bool _GameOver = false;
    [SerializeField] GameObject[] UpgradePanels = null;
    #region InternalUseOnly
    // Start is called before the first frame update
    void Awake()
    {
        CurrHud = PlayerHud;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GameOver)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if(CurrHud == Map)
                {
                    OpenMap();
                }
                else
                {
                    PauseGame();
                }
            }
            if (Input.GetButtonDown("Map") && !_Paused)
            {
                OpenMap();
            }
        }
    }
    public void SettingsSwitch()
    {
        if(CurrHud == Settings)
        {
            CurrHud = PauseHud;
        }
        else
        {
            CurrHud = Settings;
        }
    }
    //Handles Pause
    public void PauseGame()
    {
        if (CurrHud == PauseHud || CurrHud == Settings)
        {
            _Paused = !_Paused;
            Player.IsInteractable = true;
            TimeFreeze();
            UpdateDisp(PlayerHud);
        }
        else
        {
            _Paused = !_Paused;
            Player.IsInteractable = false;
            TimeFreeze();
            UpdateDisp(PauseHud);
        }
    }

    void OpenMap()
    {
        if(CurrHud == Map)
        {
            Player.IsInteractable = true;
            TimeFreeze();
            UpdateDisp(PlayerHud);
        }
        else
        {
            Player.IsInteractable = false;
            TimeFreeze();
            UpdateDisp(Map);
        }
    }
    public void RestartGame()
    {

        Player.gameObject.SetActive(true);
        FindObjectOfType<RoomManagement>().Respawn();
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Time.timeScale = 1;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _GameOver = false;
        _Paused = false;
        Player.IsInteractable = true;
        Player._isDying = false;
        Player._isInvincible = false;
        Player.CurrentHealth = Player.GetMaxHealth();
        deathAnim.SetBool("PlayerDead", false);
        UpdateDisp(PlayerHud);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void UpgradePopUpWindow(int UpgradeNum)
    {
        //if upgrade picked up
        //PauseGame();
        //which upgrade
        //show information for specific upgrade

        _Paused = !_Paused;
        Player.IsInteractable = false;
        TimeFreeze();
        UpdateDisp(UpgradeHud);
        UpgradePanels[UpgradeNum].SetActive(true);
    }
    public void ClosePanels()
    {
        _Paused = !_Paused;
        Player.IsInteractable = true;
        TimeFreeze();
        foreach (GameObject panels in UpgradePanels)
        {
            panels.SetActive(false);
        }
        UpdateDisp(PlayerHud);
    }
    #endregion InternalUseOnly
    public void PlayerDead()
    {
        _GameOver = true;
        //deathAnim.GetComponent<SpriteRenderer>().sprite = Player.GetComponent<SpriteRenderer>().sprite;
        Player.gameObject.SetActive(false);
        deathAnim.transform.position = Player.transform.position;
        deathAnim.transform.localScale = Player.transform.localScale;
        deathAnim.SetBool("PlayerDead", true);
        UpdateDisp(DeathHud);
    }
    void UpdateDisp(Canvas newDisp)
    {
        CurrHud.gameObject.SetActive(false);
        CurrHud = newDisp;
        CurrHud.gameObject.SetActive(true);
    }
    void TimeFreeze()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    public IEnumerator Saving()
    {
        AutoSave.enabled = true;
        yield return new WaitForSecondsRealtime(2);
        AutoSave.enabled = false;
    }
}
