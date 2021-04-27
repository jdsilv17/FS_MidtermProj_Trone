using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    Animator anim = null;
    [SerializeField] float timeToRise = 0f;
    [SerializeField] float fallTime = 0f;
    [SerializeField] bool upOrDown = true;
    float timer = 0f;
    float fall = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        timer = timeToRise;
        fall = fallTime;
    }

    private void Update()
    {
        if (!upOrDown)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0f)
            {
                anim.SetBool("Rise", true);
                upOrDown = true;
            }
        }

        if (upOrDown)
        {
            if (/*timer <= 0f &&*/ fall > 0f)
            {
                fall -= Time.deltaTime;
                if (fall <= 0f)
                {
                    anim.SetBool("Fall", true);
                    upOrDown = false;
                    timer = timeToRise;
                    fall = fallTime;
                }
            }
        }
    }


    private IEnumerator Untrigger()
    {
        yield return new WaitForSeconds(2f);
    }
    private void StopSpikes()
    {
        anim.SetBool("Rise", false);
        anim.SetBool("Fall", false);
        anim.SetTrigger("Empty");
    }
}
