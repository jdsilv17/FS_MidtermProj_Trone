using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCon : MonoBehaviour
{
    [SerializeField] AudioManager _AudioManager = null;
    public Text KeyCodeCount = null;
    int KeyCount = 0;

    private void Start()
    {
        YouWin();
    }
    private void YouWin()
    {
        KeyCount = PlayerPrefs.GetInt("NumberOfKeys");
        KeyCodeCount.text = KeyCount.ToString() + " / 16";
        _AudioManager.StopAll();
        _AudioManager.Play("PONR");
    }
}
