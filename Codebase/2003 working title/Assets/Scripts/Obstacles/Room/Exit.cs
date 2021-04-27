using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] public Transform ExitLoc;
    [SerializeField] int TargetExit = 0, TargetScene = 0;
    [SerializeField] public RoomHandle MyRoom = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<LocationTracker>().door = TargetExit;
            //SceneManager.LoadScene(TargetScene);
            LoadingSceneManager.LoadScene(TargetScene);
        }
    }
}
