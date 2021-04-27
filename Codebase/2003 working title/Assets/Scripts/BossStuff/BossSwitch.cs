using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwitch : MonoBehaviour
{
    [SerializeField] GameObject PreFight = null, Fight = null;
    bool Triggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!Triggered)
            {
                Triggered = true;
                GetComponent<ParticleSystem>().Play();
                StartCoroutine(EndTrigger());
            }
        }
    }
    IEnumerator EndTrigger()
    {
        FindObjectOfType<BasicCameraController>().CutShake(5);
        GetComponent<AudioManager>().Stop("Song");
        GetComponent<AudioManager>().Play("Explode");
        FindObjectOfType<PlayerController2D>().GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Fight.SetActive(true);
        FindObjectOfType<SCALEUP>().scaling = false;
        PreFight.SetActive(false);
        FindObjectOfType<PlayerController2D>().IsInteractable = false;
        yield return new WaitForSeconds(5);
        FindObjectOfType<PlayerController2D>().IsInteractable = true;
        GameObject.Destroy(gameObject);
    }
}
