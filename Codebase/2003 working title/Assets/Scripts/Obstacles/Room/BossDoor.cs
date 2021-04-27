using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoor : MonoBehaviour
{
    #region OldCode
    //[SerializeField] public RoomHandle MyRoom;
    //[SerializeField] public Transform MyExit;
    //[SerializeField] BossDoor Target;
    //RoomManagement Manager;
    //private void Start()
    //{
    //    MyRoom = GetComponentInParent<RoomHandle>();
    //    Manager = FindObjectOfType<RoomManagement>();
    //    if (Target != null)
    //    {
    //        Target.SetTarget(this);
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        Manager.UpdateRoom(Target);
    //    }
    //}
    //public void SetTarget(BossDoor Partner)
    //{
    //    Target = Partner;
    //}
    [SerializeField] int BossScene = 0;
    //LoadingSceneManager loadingManager = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene(BossScene);
        }
    }
    #endregion

}
