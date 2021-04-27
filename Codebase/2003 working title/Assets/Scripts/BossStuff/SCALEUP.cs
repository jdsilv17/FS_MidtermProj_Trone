using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCALEUP : MonoBehaviour
{
    SpriteRenderer circle = null;
    public bool scaling = true;
    Vector3 StartSize = Vector3.zero;
    [SerializeField] Vector3 endSize = Vector3.zero;
    [SerializeField] float speed= 1;
    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<SpriteRenderer>();
        StartSize = transform.localScale;
    }
    private void Update()
    {
        if (!scaling)
        {
            StartCoroutine(scale());
        }
    }
    // Update is called once per frame
    IEnumerator scale()
    {
        scaling = true;
        circle.gameObject.transform.localScale = Vector3.Lerp(circle.gameObject.transform.localScale, endSize, .0009f);
        yield return new WaitForSeconds(speed);
        scaling = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2D>().TakeDamage(collision.GetComponent<PlayerController2D>().CurrentHealth + collision.GetComponent<PlayerController2D>().CurrentShield);
        }
    }
    public void Restart()
    {
        transform.localScale = StartSize;
    }
}
