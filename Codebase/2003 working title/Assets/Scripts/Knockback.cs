///////////////////////////////////////////////////////////////////////////////
// File name:		Knockback.cs
//
// Purpose:		    Knockback/Add a force to an object when it collides with another**
//
// IMPORTANT:       **Currently is specified for PlayerController2D.cs and objects tagged "Spikes"**
//                  **Currently requires use of either OnCollision/OnTrigger methods**
//
// Related Files:	PlayerController2D.cs
//
// Author:			Justin DaSilva
//
// Created:			5/22/20
//
// Last Modified:	5/28/20
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D _rb = null;
    private PlayerController2D _player = null;
    private bool _knockback = false;

    public Vector2 _forceToAdd = Vector2.zero;
    public enum TypeOfCollider
    {
        NonTrigger,
        Trigger
    }
    public TypeOfCollider _colliderTypeOfOther = 0;

    //[SerializeField] AudioManager SFX = null;



    public ContactPoint2D ContactPoint2D { get; set; }
    public Vector2 ContactPos { get; set; }

    void Start()
    {
        _player = GetComponent<PlayerController2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_knockback)
        {
            _knockback = false;
            switch (_colliderTypeOfOther)
            {
                case TypeOfCollider.NonTrigger:
                    DoKnockback(_forceToAdd, ContactPoint2D);
                    break;
                case TypeOfCollider.Trigger:
                    DoKnockback(_forceToAdd, ContactPos);
                    break;
                default:
                    break;
            }
            /// Get knocked back for a certain amount of time
            /// then reverse the knockback
        }
    }

    /// <summary>
    /// (Can only use with OnTrigger methods) Adds force to an object based on the position of the contact point
    /// </summary>
    /// <param name="force">The force to apply</param>
    /// <param name="contactPos">The position to apply the force at</param>
    public void DoKnockback(Vector2 force, Vector2 contactPos)
    {
        _rb.velocity = Vector2.zero;
        if (contactPos.x > transform.position.x)
        {
            _rb.AddForce(new Vector2(-force.x, force.y), ForceMode2D.Impulse);
        }
        else if (contactPos.x < transform.position.x)
        {
            _rb.AddForce(new Vector2(force.x, force.y), ForceMode2D.Impulse);
        }
        else if (contactPos.y > transform.position.y)
        {
            _rb.AddForce(new Vector2(force.x, force.y * 0.5f), ForceMode2D.Impulse);
        }
        else if (contactPos.y < transform.position.y)
        {
            _rb.AddForce(new Vector2(force.x, force.y), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// (Can only use with OnCollision methods) Adds force to an object at a specific point of contact 
    /// </summary>
    /// <param name="force">The force to apply</param>
    /// <param name="contactPnt">The point of contact that the collision occured</param>
    public void DoKnockback(Vector2 force, ContactPoint2D contactPnt)
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForceAtPosition(force * contactPnt.normal, contactPnt.point, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            ContactPos = collision.ClosestPoint(transform.position);
            _knockback = true;
            _player.TakeDamage(10);
            //SFX.Play("SP_Damage");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            ContactPoint2D = collision.GetContact(0);
            _knockback = true;
            _player.TakeDamage(10);
            //SFX.Play("SP_Damage");
        }
    }
}
