using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour
{
    [SerializeField] GameObject Player = null;
    [SerializeField] float shakeTime = 0f, shakeAmount = 0.7f, decreaseFactor = 1f, CameraOffset =0;
    Transform camTrans;
    bool _shake;
    float shakePos, timeRemain, originalz;
    //800x600 image
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        shakePos = Player.transform.position.x;
        if (_shake)
        {
            if(timeRemain > 0)
            {
                shakePos += Random.Range(-shakeAmount, shakeAmount);
                timeRemain -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                timeRemain = 0f;
                _shake = false;
            }
            transform.position = Vector3.Lerp(new Vector3(shakePos, Player.transform.position.y + CameraOffset, transform.position.z), transform.position, .5f);
        }
        else
        {
            transform.position = new Vector3(shakePos, Player.transform.position.y + CameraOffset, transform.position.z);
        }
    }
    public void AttackShake()
    {
        _shake = true;
        timeRemain = shakeTime;
        originalz = Player.transform.position.x;
    }
    public void CutShake(float Time)
    {
        _shake = true;
        timeRemain = Time;
        originalz = Player.transform.position.x;
    }
}
