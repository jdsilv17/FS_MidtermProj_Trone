using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirecttargetDoor : MonoBehaviour
{
    LocationTracker loctrack = null;
    private void Start()
    {
        loctrack = FindObjectOfType<LocationTracker>();
    }
    public void RedirectTargetDoor()
    {
        if (loctrack != null)
        {
            loctrack.door = 0;
        }
    }
}
