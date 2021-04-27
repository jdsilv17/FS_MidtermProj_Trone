using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandle : MonoBehaviour
{
    [SerializeField]List<GameObject> Items = new List<GameObject>();
    [SerializeField] string RoomKey = null;

    private void Start()
    {
        //if (PlayerPrefs.HasKey(RoomKey))
        //{
        //    PlayerHasSeen = true;
        //}
        //if (PlayerHasSeen)
        //{
        //    MinimapIcon.color = HasUpgrade;
        //}
    }
    private void Update()
    {
        //if(GetComponentsInChildren<Pickup>().Length == 0 && PlayerHasSeen)
        //{
        //    MinimapIcon.color = Finished;
        //}
        //else if (PlayerHasSeen)
        //{
        //    MinimapIcon.color = HasUpgrade;
        //}
    }
    void Awake()
    {
        //On start grab all children destroy them so that when enabled it will create them again.
        foreach(Transform item in this.transform)
        {
            if(item.tag == "Enemy" || item.tag == "Health")
            {
                GameObject temp = item.gameObject;
                Items.Add(temp);
                item.gameObject.SetActive(false);
            }
        }
    }
    public void Unload()
    {
        //gameObject.SetActive(false);
        foreach(Transform item in this.transform)
        {
            if (item.gameObject.activeSelf && (item.tag == "Enemy" || item.tag == "Health"))
            {
                Destroy(item.gameObject);
            }
        }
    }
    public void Load()
    {
        //gameObject.SetActive(true);
        PlayerPrefs.SetString(RoomKey, "Seen");
        PlayerPrefs.Save();
        foreach (GameObject item in Items)
        {
            GameObject clone = Instantiate(item, transform, true);
            clone.SetActive(true);
        }
    }
}
