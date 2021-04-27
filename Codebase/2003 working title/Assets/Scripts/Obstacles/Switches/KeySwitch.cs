using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySwitch : BaseSwitch
{
    [SerializeField] int ReqKeys = 0;
    [SerializeField] string GateName = null;
    TextMesh text = null;
    bool Opened = false;
    [SerializeField] AudioManager SFX = null;
    private void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        Opened = PlayerPrefs.HasKey(GateName);
        if (Opened)
        {
            text.color = Color.green;
            text.text = "Opened";
            DisableObjects();
        }
    }
    public override void Activate()
    {
        if(FindObjectOfType<KeyTracker>().GetKeyCount() >= ReqKeys && !Opened)
        {
            if(text != null)
            {
                text.color = Color.green;
                text.text = "Opened";
            }
            Opened = true;
            PlayerPrefs.SetString(GateName, GateName);
            PlayerPrefs.Save();
            DisableObjects();
            SFX.Play("SW_Activate");
        }
        else
        {
            StartCoroutine(KeyReq());
        }
    }
    IEnumerator KeyReq()
    {
        string temp = text.text;
        string temp1 = FindObjectOfType<KeyTracker>().GetKeyCount().ToString();
        text.text = temp1 + " / " + ReqKeys.ToString();
        yield return new WaitForSeconds(2);
        text.text = temp;
    }
}
