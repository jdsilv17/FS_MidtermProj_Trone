using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioManager>().Play("Song");
    }
}
