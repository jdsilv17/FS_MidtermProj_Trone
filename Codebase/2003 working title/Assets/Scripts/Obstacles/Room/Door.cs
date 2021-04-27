using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region OldCode
    //[SerializeField] public RoomHandle MyRoom;
    //[SerializeField] public Transform MyExit;
    //[SerializeField] Door Target;
    //RoomManagement Manager;
    //private void Start()
    //{
    //    MyRoom = GetComponentInParent<RoomHandle>();
    //    Manager = FindObjectOfType<RoomManagement>();
    //    if(Target != null)
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
    //public void SetTarget(Door Partner)
    //{
    //    Target = Partner;
    //}
    #endregion
    [SerializeField] public RoomHandle UpOrLeft = null, DownOrRight = null;
    [SerializeField] bool UpDown = false;
    RoomManagement Manager = null;
    Vector2 Player = Vector2.zero;
    void Start()
    {
        Manager = FindObjectOfType<RoomManagement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController2D>())
        {
            Player = collision.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.GetComponent<PlayerController2D>())
        {
            if (UpDown)
            {
                if (Player.y > other.transform.position.y)
                {
                    Manager.UpdateRoom(DownOrRight);
                }
                else if (other.transform.position.y > Player.y)
                {
                    Manager.UpdateRoom(UpOrLeft);
                }
            }

            else
            {
                if (Player.x < other.transform.position.x)
                {
                    Manager.UpdateRoom(DownOrRight);
                }
                else if (Player.x > other.transform.position.x)
                {
                    Manager.UpdateRoom(UpOrLeft);
                }
            }
                
        }


    }
}
