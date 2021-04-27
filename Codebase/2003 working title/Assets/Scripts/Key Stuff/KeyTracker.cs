using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTracker : MonoBehaviour
{
    int NumberOfKeys = 0;
    [SerializeField] int maxKeys = 0;
    [SerializeField] Image KeySprite = null, Bar = null;
    [SerializeField] Text MaxKeys = null, CurrentKeys = null;
    private void Start()
    {
        NumberOfKeys = PlayerPrefs.GetInt("NumberOfKeys");
        if(NumberOfKeys != 0)
        {
          
            Activate();
            CurrentKeys.text = NumberOfKeys.ToString();
        }
    }
    void Activate()
    {
        KeySprite.gameObject.SetActive(true);
        MaxKeys.gameObject.SetActive(true);
        CurrentKeys.gameObject.SetActive(true);
        Bar.gameObject.SetActive(true);
        MaxKeys.text = maxKeys.ToString();
    }
    public void KeyPickedUp()
    {
        if(NumberOfKeys == 0)
        {
            Activate();
        }
        NumberOfKeys++;
        CurrentKeys.text = NumberOfKeys.ToString();
        PlayerPrefs.SetInt("NumberOfKeys", NumberOfKeys);
        PlayerPrefs.Save();
    }
    public int GetKeyCount()
    {
        return NumberOfKeys;
    }
}
