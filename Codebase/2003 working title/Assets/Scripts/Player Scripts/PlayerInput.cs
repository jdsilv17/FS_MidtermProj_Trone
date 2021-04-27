///////////////////////////////////////////////////////////////////////////////
// File name:		PlayerScript
//
// Purpose:		    To listen for player input
//
// Related Files:	Controller2D
//
// Author:			Justin DaSilva
//
// Created:			4/11/20
//
// Last Modified:	5/21/20
///////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (Controller2D_Test))]
public class PlayerInput : MonoBehaviour
{
    private PlayerController2D _controller2D;

    private void Start()
    {
        _controller2D = GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        ///////////////////////////////////////////////////////
        // Player Movement USING RB
        ///////////////////////////////////////////////////////
        _controller2D._moveInput = ((_controller2D.IsInteractable || _controller2D._canMove) && Time.timeScale != 0f) ? Input.GetAxis("Horizontal") : 0f;

        ///////////////////////////////////////////////////////
        // If Input is enabled, listen for JUMP input
        ///////////////////////////////////////////////////////
        if (Input.GetButtonDown("Jump"))
        {
            _controller2D._pressedJump = true;

            if (_controller2D._hasWallJump && _controller2D._isWallSliding)
            {
                _controller2D._canWallJump = true;
                //_controller2D._isWallJumping = true;
            }
        }

        ///////////////////////////////////////////////////////
        // Call Attack() method
        ///////////////////////////////////////////////////////
        if (Input.GetButtonDown("Fire1") && Input.GetAxis("Vertical") > 0)
        {
            _controller2D.AttackUp();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            _controller2D.Attack();
        }

        ///////////////////////////////////////////////////////
        // DASH
        ///////////////////////////////////////////////////////
        if (Input.GetButtonDown("Fire2"))
        {
            if (_controller2D._hasDash && !_controller2D._isDashing)
            {
                _controller2D._canDash = true;
            }
        }

        ///////////////////////////////////////////////////////
        // Wrench Throw
        ///////////////////////////////////////////////////////
        if (Input.GetButtonDown("Fire3"))
        {
            _controller2D.FireProjectile();
        }

    }
}
