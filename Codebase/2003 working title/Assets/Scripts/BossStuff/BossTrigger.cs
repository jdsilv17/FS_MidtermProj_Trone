using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : BaseEnemy
{
    [SerializeField] int KeyReq = 0, numOfSwings = 0;
    //[SerializeField] float ShakeTime = 0;
    [SerializeField] Sprite[] breakeffect = null;
    [SerializeField] GameObject BossLevel = null, PreLevel = null;
    [SerializeField] AudioManager Music = null;
    public override void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if (FindObjectOfType<KeyTracker>().GetKeyCount() >= KeyReq)
        {
            Hurt();
        }
    }
    void Hurt()
    {
        if(numOfSwings+1 >= breakeffect.Length)
        {
            Music.Stop("PONR");
            StartCoroutine(EndTrigger());
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = breakeffect[numOfSwings];
            numOfSwings++;
        }
    }
    IEnumerator EndTrigger()
    {
        FindObjectOfType<BasicCameraController>().CutShake(5);
        FindObjectOfType<PlayerController2D>().GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        BossLevel.SetActive(true);
        FindObjectOfType<SCALEUP>().scaling = false;
        PreLevel.SetActive(false);
        FindObjectOfType<PlayerController2D>().IsInteractable = false;
        yield return new WaitForSeconds(5);
        FindObjectOfType<PlayerController2D>().IsInteractable = true;
        Music.Play("Countdown");
        GameObject.Destroy(gameObject);
    }
    
}
