using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSpikes : MonoBehaviour
{
    [SerializeField]
    float timer = 3f;
    SpriteRenderer _childsr = null;
    // Start is called before the first frame update
    void Start()
    {
        _childsr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            _childsr.enabled = true;
            timer = 3f;
        }
    }

}
