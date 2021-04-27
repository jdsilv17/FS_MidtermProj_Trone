using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : BaseEnemy
{
    #region Editable fields
    //[SerializeField] GameObject Player = null;
    [SerializeField] float chargeSpeed = 2, chaseTime = 5f, baseChaseTime = 5f;
    #endregion
    bool Chasing = false;
    Vector2 target;

    private void OnEnable()
    {
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Platform");
        health = starthealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (Chasing)
        {
            Chase();
        }
        else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rayCastLength, playerLayer))
        {
            Chasing = true;
        }
        else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), rayCastLength, playerLayer))
        {
            Chasing = true;
        }
    }
    void Chase()
    {
        if(transform.position.x < target.x && transform.eulerAngles != Vector3.zero)
        {
                transform.eulerAngles = transform.eulerAngles - new Vector3(0, 180, 0);
        }
        else if (transform.position.x > target.x && transform.eulerAngles == Vector3.zero)
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
        }
        chaseTime -= Time.deltaTime;
        if (Vector2.Distance(transform.position, Player.transform.position) <= 10f && chaseTime > 0)
        {
            target = Player.transform.position;
        }
        else
        {
            chaseTime -= Time.deltaTime;
        }
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down + (Vector2.right * .7f)), 5f, groundLayer))
        {
            Move();
        }
        if (chaseTime <= 0f)
        {
            Chasing = false;
            chaseTime = baseChaseTime;
        }
    }
    void Move()
    {
        target = new Vector2(target.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * chargeSpeed);
    }
}
