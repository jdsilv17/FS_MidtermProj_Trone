using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    //gravity modifier
    public float gravMod = 1f;
    //used for movement collision checking on ground
    public float minGroundNormalY = .65f;
    //to store incoming input for where trying to move character
    protected Vector2 targetVelocity;
    //velocity, protected bc inherited
    protected Vector2 velocity;
    //used to check for overlap
    protected ContactFilter2D contactFilter;
    protected const float minMoveDistance = 0.001f;
    //used to get position
    protected Rigidbody2D rb2d;
    //for casting
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected const float shellRadius = 0.01f;
    //used to copy array
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    //checking if player is grounded
    protected bool grounded;
    protected Vector2 groundNormal;
    //getting position
    private void OnEnable()
    {
        rb2d.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        //used to find what layers we're going to check collision against
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }
    protected virtual void ComputeVelocity()
    { }
    private void FixedUpdate()
    {
        //deltatime is time at which they jumped
        velocity += gravMod * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
        grounded = false;
        //position after jump
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);
        //will be used in movement function later
        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    //adding movement vector to position and checking for overlap of colliders
    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            //for checking overlap/collision
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            //for copying array into list
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            //to check the normal of each item in the list and compare to a value
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                //difference between velocity and current normal and whether need to decrease velocity bc of collision
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    //cancels out velocity after collision
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
