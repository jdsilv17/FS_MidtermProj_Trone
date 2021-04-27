using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableContine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     if(PlayerPrefs.GetInt("DoubleJump") == 1 && PlayerPrefs.GetInt("NumberOfKeys") >= 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
