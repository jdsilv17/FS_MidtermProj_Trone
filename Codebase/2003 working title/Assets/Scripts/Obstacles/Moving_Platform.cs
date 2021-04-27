using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    [SerializeField]
    float speed = 2;
    [SerializeField]
    Transform[] destinations = null;

    Vector3 Target = Vector3.zero;
    Vector3 Home = Vector3.zero;
    int Iter = 0;
    float step = 0;

    private void Start()
    {
        Target = destinations[0].position;
        Home = transform.position;
        //step = speed * Time.deltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, Target, step);

        if (Vector3.Distance(transform.position, Target) <= 0.01f)
        {
            UpdateTarget();
        }
    }


    private void UpdateTarget()
    {
        Iter++;
        if (Iter >= destinations.Length)
        {
            Iter = -1;
            Target = Home;
        }
        else
        {
            Target = destinations[Iter].position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
