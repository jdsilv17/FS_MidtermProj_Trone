using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSwitch : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToToggle = null;
    public virtual void Activate() { return; }

    protected void EnableObjects()
    {
        foreach(GameObject obj in objectsToToggle)
        {
            obj.SetActive(true);
        }
    }
    protected void DisableObjects()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerProjectile"))
        {
            Activate();
        }
    }
}
