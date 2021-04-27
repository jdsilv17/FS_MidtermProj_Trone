using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public RoomHandle MyRoom { get; private set; }
    ParticleSystem LoadEffect;
    TextMesh Text;
    bool ActivePoint = false;
    [SerializeField] AudioManager SFX = null;
    // Start is called before the first frame update
    void Start()
    {
        MyRoom = GetComponentInParent<RoomHandle>();
        LoadEffect = GetComponentInChildren<ParticleSystem>();
        Text = GetComponentInChildren<TextMesh>();
        Text.gameObject.SetActive(false);
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FindObjectOfType<RoomManagement>().SetRespawn(this);
            SFX.Play("CP_Save");
            Text.gameObject.SetActive(true);
            LoadEffect.Play();
            collision.GetComponent<PlayerController2D>().CurrentHealth = collision.GetComponent<PlayerController2D>().GetMaxHealth();
            if (collision.GetComponent<PlayerController2D>()._hasShield)
            {
                collision.GetComponent<PlayerController2D>().CurrentShield = collision.GetComponent<PlayerController2D>().GetMaxShield();
            }
            yield return new WaitForSeconds(LoadEffect.main.duration);
            Text.gameObject.SetActive(false);
        }

    }
    public void SetEnabled()
    {
        if (ActivePoint)
        {
            ActivePoint = false;
            GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            ActivePoint = true;
            GetComponent<ParticleSystem>().Play();
        }
    }
    //public IEnumerator DisableText(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //}
}
