///////////////////////////////////////////////////////////////////////////////
// File name:		Controller2D.cs
//
// Purpose:		    To implement the player functionality
//
// Related Files:	PlayerInput.cs
//
// Author:			Justin DaSilva/Amara Gitomer
//
// Created:			3/25/20
//
// Last Modified:	5/28/20
///////////////////////////////////////////////////////////////////////////////
using System.Collections;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private Rigidbody2D _rb = null;
    private Collider2D _collider2D = null;
    private Collider2D _cirCollider2D = null;
    private SpriteRenderer _spriteRenderer = null;
    private int _count = 1;
    private float _currentGravity = 5f, _jumpVelocity = 0f;
    private bool _knockedback = false, _wasDashing = false, _wasDoubleJumping = false, _tookDamage = false;
    private float _jumpTimeCounter = 0f, _dashTimeCounter = 0f, _knockbackTimeCounter = 0f, _rememberPressedJump = 0f;

    [Header("Player")]
        [SerializeField] private int _StartHealth = 0;
        [SerializeField] private float _speed = 0f;
        [SerializeField] private Transform _feetPos = null, _feetPos2 = null, _facingPos = null, _attackPoint = null, _attackUpPoint = null;
        [SerializeField] private float _checkRadius = 0;
        public int _attackDamage = 0;
        [SerializeField] private Vector2 _attackHitBox = Vector2.zero, _attackKnockback = Vector2.zero;
        [SerializeField] private GameObject _upAttackWrench = null;
        [SerializeField] private float _knockbackTime = 0f;
        [Tooltip ("The min and max velocity the player can travel in a vertical direction")]
        [SerializeField] private float _minVerticalVelocity = 0f, _maxVerticalVelocity = 0f;
        [SerializeField] private LayerMask _platformLayerMask = 0, _enemyLayerMasks = 0;
        [SerializeField] private Animator _animator = null;
        [SerializeField] private GameObject _dashOrb = null;
    
    [Header ("Jump")]
        [SerializeField] private float _jumpHeight = 0f, _dubJumpVelocity = 0f;
        [SerializeField] private float _jumpTime = 0f, _jumpTimeToApex = 0f, _rememberPressedJumpTime = 0f;
    
    [Header ("Wall Jump")]
        [SerializeField] private Vector2 _wallJump = Vector2.zero;
        [SerializeField] private float _maxWallSlideSpeed = 0;
    
    [Header("Dash")]
        [SerializeField] private float _dashSpeed = 0f, _dashTime = 0f;

    [Header ("Shield")]
        [SerializeField] private int _MaxShield = 0, _WrenchCost = 0;
        [SerializeField] private float _ShieldDelay = 0f, _ShieldChargeRate = 0f, _shieldCooldown = 0f;
        public bool _chargingShield = false;

    [Header ("Wrench Throw")]
        [SerializeField] private GameObject _wrenchProjectile = null;
        [Tooltip("The amount of time the player is frozen when a Wrench Throw is fired")]
        [SerializeField] private float _wrenchThrowFreezeTime = 0f;

    [Header("Ability Checks")]
        public bool _hasDash = false, _hasDoubleJump = false, _hasWallJump = false, _hasShield = false, _hasWrenchThrow = false;
        public bool _isJumping = false, _isDashing = false, _isWallJumping = false;
        public bool _canDoubleJump = false, _canDash = false, _canWallJump = false;
    
    [Header ("Collision Checks")]
        public bool _isGrounded = false, _isOnWall = false, _isWallSliding = false, _isBackOnWall = false;
    
    // Properties
    public int CurrentHealth { get; set; }
    public int CurrentShield { get; set; }
    public bool IsInteractable { get; set; }
    public AudioManager PlayerSound = null;
    
    [System.NonSerialized] public float _moveInput = 0;
    [System.NonSerialized] public bool _pressedJump = false, _isDying = false, _isInvincible = false, _canMove = true;

    private void OnEnable()
    {
        _shieldCooldown = 0;
        _chargingShield = false;
        GetComponent<TrailRenderer>().enabled = false;
        _dashOrb.SetActive(false);
        IsInteractable = true;
        _isDying = false;
        _isInvincible = false;
        CurrentHealth = _StartHealth;
        _dashTimeCounter = _dashTime;
        _knockbackTimeCounter = _knockbackTime;
        _isDashing = false;
        _wasDashing = false;
        _knockedback = false;
        _tookDamage = false;
        _rememberPressedJump = _rememberPressedJumpTime;
        _currentGravity = 5f;
        PlayerSound.StopAll();
    }

    private void Awake()
    {
        _rb = transform.GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _cirCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _dashOrb.SetActive(false);
        GetComponent<TrailRenderer>().enabled = false;
        _tookDamage = false;

        _hasDoubleJump = PlayerPrefs.GetInt("DoubleJump", 0) == 1 ? true: false;
        _hasDash = PlayerPrefs.GetInt("Dash", 0) == 1 ? true: false;
        _hasWallJump = PlayerPrefs.GetInt("WallJump", 0) == 1 ? true: false;
        _hasShield = PlayerPrefs.GetInt("Shield", 0) == 1 ? true : false;
        _hasWrenchThrow = PlayerPrefs.GetInt("WrenchThrow", 0) == 1 ? true: false;

        if (_hasShield)
            ActivateShield();
        CurrentHealth = _StartHealth;

        if (!_isGrounded)
        {
            _rb.velocity = new Vector2(0, -0.02f);
        }

        // Calculate Jump time and velocity
        _jumpTimeToApex = (_jumpHeight * 2f) / (-Physics2D.gravity.y * _rb.gravityScale);
        _jumpVelocity = (-Physics2D.gravity.y * _rb.gravityScale) * _jumpTimeToApex;
    }

    private void Update()
    {
        ///////////////////////////////////////////////////////
        // Animation Conditions
        ///////////////////////////////////////////////////////
        _animator.SetFloat("_speed", Mathf.Abs(_rb.velocity.x));
        _animator.SetFloat("VerticalVelocity", _rb.velocity.y);
        _animator.SetBool("WallJumping", _isWallJumping);
        _animator.SetBool("_isGrounded", _isGrounded);

        if (!_tookDamage && _spriteRenderer.color != new Color(1f, 1f, 1f, 1f))
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        ///////////////////////////////////////////////////////
        // Collision detection
        ///////////////////////////////////////////////////////
        bool isGrounded = Physics2D.Raycast(_feetPos.position, Vector3.down, _checkRadius, _platformLayerMask);
        bool isGrounded2 = Physics2D.Raycast(_feetPos2.position, Vector3.down, _checkRadius, _platformLayerMask);
        if (isGrounded || isGrounded2)
            _isGrounded = true;
        else
            _isGrounded = false;
        _isOnWall = Physics2D.Raycast(_facingPos.position, transform.right, _checkRadius, _platformLayerMask);

        ///////////////////////////////////////////////////////
        // Regenerate Shield
        ///////////////////////////////////////////////////////
        if (_shieldCooldown > 0 && _hasShield)
        {
            _shieldCooldown -= Time.deltaTime;
        }
        else if (_shieldCooldown <= 0 && _hasShield && !_chargingShield && CurrentShield != _MaxShield)
        {
            _chargingShield = true;
            StartCoroutine(ChargeShield());
        }

        ///////////////////////////////////////////////////////
        // Check if the player changes direction
        ///////////////////////////////////////////////////////
        if (IsInteractable || !_isDashing)
        {
            if (_moveInput > 0) // Facing right
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (_moveInput < 0) // Facing Left
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        ///////////////////////////////////////////////////////
        // Add linear Drag/Air Resistence when falling
        ///////////////////////////////////////////////////////
        float verticalVelocity = Mathf.Clamp(_rb.velocity.y, _minVerticalVelocity, _maxVerticalVelocity);
        _rb.velocity = new Vector2(_rb.velocity.x, verticalVelocity);

        ///////////////////////////////////////////////////////
        // Check for wall sliding
        // Force player to slide down the wall after certain amount of time
        ///////////////////////////////////////////////////////
        if (_hasWallJump && _isOnWall && !_isGrounded)
        {
            if (_rb.velocity.y < 0)
            {
                _spriteRenderer.flipX = true;
                _isWallSliding = true;
                if (_rb.velocity.y < -_maxWallSlideSpeed)
                {
                    _rb.velocity = Vector2.up * -_maxWallSlideSpeed;
                }
            }
        }
        else if (!_isOnWall || _isGrounded)
        {
            _spriteRenderer.flipX = false;
            _isWallSliding = false;
        }
        _animator.SetBool("WallSliding", _isWallSliding);

        ///////////////////////////////////////////////////////
        // Variable Jump
        // While they HOLD down the jump button check if they have jump time
        ///////////////////////////////////////////////////////
        if (_isJumping && Input.GetButton("Jump"))
        {
            if (_jumpTimeCounter > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpVelocity);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
        }

        ///////////////////////////////////////////////////////
        // Jump Coyote Time period
        ///////////////////////////////////////////////////////
        if (!_isGrounded)
        {
            _rememberPressedJump -= Time.deltaTime;
        }
        else if (_rememberPressedJump == 0f || _isJumping || _isGrounded)
        {
            _rememberPressedJump = _rememberPressedJumpTime;
        }

        ///////////////////////////////////////////////////////
        // Dash
        ///////////////////////////////////////////////////////
        if (_isDashing)
        {
            if (_dashTimeCounter > 0)
            {
                _rb.gravityScale = 0;
                _dashTimeCounter -= Time.deltaTime;
            }
            else
            {
                IsInteractable = true;
                _isInvincible = false;
                _rb.gravityScale = _currentGravity;
                _isDashing = false;
                _dashTimeCounter = _dashTime;
                GetComponent<TrailRenderer>().enabled = false;
                _dashOrb.SetActive(false);
                _wasDashing = true;
            }
        }

        ///////////////////////////////////////////////////////
        // Knockback Timer
        ///////////////////////////////////////////////////////
        if (_knockedback)
        {
            if (_knockbackTimeCounter > 0)
            {
                _knockbackTimeCounter -= Time.deltaTime;
            }
            else
            {
                _knockedback = false;
                _knockbackTimeCounter = _knockbackTime;
            }
        }
    }

    private void FixedUpdate()
    {
        // DASH
        if (!IsInteractable && _canDash && _canMove)
            PlayerSound.Play("P_Error");
        else if (_wasDashing && (_count == 1))
        {
            _count = 0;
            StartCoroutine(WaitForFinish());
        }
        else if (!_wasDashing && !_isDashing && _canDash && !_isOnWall)
        {
            _count = 1;
            _canDash = false;
            Dash();
        }
        else if (_canDash)
            _canDash = false;

        // MOVE
        if (!_isDashing && !_isOnWall)
            Move();

        // JUMP
        if (!IsInteractable && _pressedJump)
            PlayerSound.Play("P_Error");
        else if (_pressedJump && (_isGrounded || (_rememberPressedJump > 0f)))
        {
            Jump();
        }
        else if (_pressedJump && _canDoubleJump && !_canWallJump)
        {
            DoubleJump();
            StartCoroutine(WaitForFinish());
            _pressedJump = false;
        }
        else if (_pressedJump && !_isJumping)
            _pressedJump = false;

        // WALLJUMP
        if (_canWallJump)
        {
            if (IsInteractable)
            {
                StartCoroutine(WallJump());
                _isWallJumping = true;
            }
            else
            {
                _canWallJump = false;
            }
        }

        // KNOCKBACK
        if (_knockedback)
            Knockback(_attackKnockback, _attackPoint.position);
    }


    #region ABILITIES
    /// <summary>
    /// Sets the player's rigidbody's velocity
    /// </summary>
    private void Move()
    {
        if (!IsInteractable && !_canMove)
            return;

        _rb.velocity = new Vector2(_moveInput * _speed, _rb.velocity.y);
        if (_isGrounded && (_rb.velocity.x != 0f))
        {
            GetComponent<ParticleSystem>().Play();
        }
    }

    /// <summary>
    /// Sets the player's velocity when the player dashes
    /// </summary>
    private void Dash()
    {
        if (!IsInteractable && _canMove)
            return;

        IsInteractable = false;
        _isInvincible = true;
        _isDashing = true;
        _rb.velocity = new Vector2(transform.right.x * _dashSpeed, 0f);
        GetComponent<TrailRenderer>().enabled = true;
        _dashOrb.SetActive(true);
        PlayerSound.Play("P_Dash");
    }

    /// <summary>
    /// Sets the player's vertical velocity when jumping and initializes the double jump check
    /// </summary>
    private void Jump()
    {
        if (!IsInteractable)
            return;

        _isJumping = true;
        _animator.SetBool("_isJumping", _isJumping);
        PlayerSound.Play("P_Jump");

        if (_hasDoubleJump)
        {
            _canDoubleJump = true;
        }
        _jumpTimeCounter = _jumpTime;
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpVelocity);

        _pressedJump = false;
    }

    /// <summary>
    /// Sets the player's vertical velocity when Double Jumping
    /// </summary>
    private void DoubleJump()
    {
        if (!IsInteractable)
            return;

        _canDoubleJump = false;
        _wasDoubleJumping = true;
        GetComponent<TrailRenderer>().enabled = true;
        PlayerSound.Play("P_DoubleJump");
        _rb.velocity = Vector2.zero;
        _rb.velocity = new Vector2(_rb.velocity.x, _dubJumpVelocity);
    }

    ///<summary> 
    /// Applies a Force used for wall jump 
    ///</summary>
    private IEnumerator WallJump()
    {
        IsInteractable = false;
        PlayerSound.Play("P_WallJump");
        Vector2 forceToAdd = new Vector2(_wallJump.x * -transform.right.x, _wallJump.y);
        _rb.velocity = Vector2.Lerp(_rb.velocity, forceToAdd, 1f);

        yield return new WaitForSeconds(0.15f);

        if (_hasDoubleJump)
        {
            _canDoubleJump = true;
        }
        IsInteractable = true;
        _isWallJumping = false;
    }

    /// <summary>
    /// Plays Attack animation
    /// Checks for collision b/w _attackPoint and enemy layer mask
    /// </summary>
    public void Attack()
    {
        if (!IsInteractable)
        {
            PlayerSound.Play("P_Error");
            return;
        }

        _animator.SetTrigger("Attack");
        PlayerSound.Play("P_Attack");
        Collider2D hitEnemy = Physics2D.OverlapBox(_attackPoint.position, _attackHitBox, 0f, _enemyLayerMasks);
        if (hitEnemy != null)
        {
            _knockedback = true;
            FindObjectOfType<BasicCameraController>().AttackShake();
            hitEnemy.gameObject.GetComponent<EnemyHandle>().TakeDamage(_attackDamage, _attackPoint.position);
        }
    }

    /// <summary>
    /// Plays Up Attack animation
    /// Instantiates a wrench object which damages enemies
    /// </summary>
    public void AttackUp()
    {
        if (!IsInteractable)
        {
            PlayerSound.Play("P_Error");
            return;
        }

        _animator.SetTrigger("UpAttack");
        PlayerSound.Play("P_Attack");
        Collider2D hitEnemy = Physics2D.OverlapBox(_attackUpPoint.position, _attackHitBox, 0f, _enemyLayerMasks);
        GameObject wrench = Instantiate(_upAttackWrench, _attackUpPoint.position, _upAttackWrench.transform.rotation, transform);
        wrench.GetComponent<ProjectileController>().InitAnimation();
        if (hitEnemy != null)
        {
            FindObjectOfType<BasicCameraController>().AttackShake();
            hitEnemy.gameObject.GetComponent<EnemyHandle>().TakeDamage(_attackDamage, _attackUpPoint.position);
        }
    }

    /// <summary>
    /// Fires a wrench projectile from the player
    /// Decreases player's Shield amount
    /// </summary>
    public void FireProjectile()
    {
        if (!IsInteractable)
        {
            PlayerSound.Play("P_Error");
            return;
        }

        if (_hasWrenchThrow)
        {
            if (CurrentShield - _WrenchCost > 0)
            {
                _animator.SetTrigger("Attack");
                PlayerSound.Play("P_WrenchThrow");
                CurrentShield -= _WrenchCost;
                GameObject wrench = Instantiate(_wrenchProjectile, _attackPoint.position, _wrenchProjectile.transform.rotation, transform);
                wrench.GetComponent<ProjectileController>().Init();
                wrench.GetComponent<ProjectileController>().InitAnimation();
                FindObjectOfType<BasicCameraController>().AttackShake();
                StartCoroutine(FreezeTime(_wrenchThrowFreezeTime));
            }
            else
                return;
        }
        else
            return;
    }
    #endregion

    /// <summary>
    /// Waits for end of player's dash and double jump
    /// </summary>
    /// <returns> 
    /// Waits for 2 secs
    /// </returns>
    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(0.2f);
        if (_wasDashing)
        {
            _wasDashing = false;
        }
        else if (_wasDoubleJumping)
        {
            GetComponent<TrailRenderer>().enabled = false;
            _wasDoubleJumping = false;
        }
    }

    /// <summary>
    /// Disable player movement for a specified amount of time
    /// </summary>
    /// <param name="waitTime">
    /// The amount of time to freeze the player
    /// </param>
    private IEnumerator FreezeTime(float waitTime)
    {
        IsInteractable = false;
        _canMove = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        IsInteractable = true;
        _canMove = true;
        _rb.isKinematic = false;
    }

    /// <summary>
    /// Sets the player to be invincible to for 1 sec
    /// </summary>
    private IEnumerator InvincibilityTime()
    {
        _isInvincible = true;
        IsInteractable = false;
        _canMove = true;
        yield return new WaitForSecondsRealtime(1f);
        IsInteractable = true;
        _isInvincible = false;
        _canMove = false;
        _tookDamage = false;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    /// <summary>
    /// Increases and Descreases the alpha of the player sprite
    /// </summary>
    /// <returns></returns>
    private IEnumerator Flicker()
    {
        //_spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.time, 1f));
        float alpha;
        for (alpha = 1f; alpha > 0f; alpha -= Time.deltaTime * 3)
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        for (alpha = 0f; alpha < 1f; alpha += Time.deltaTime * 6)
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        if (_tookDamage)
        {
            StartCoroutine(Flicker());
        }
        else
            yield return 0;
    }

    //private void LowerAlpha(float alpha)
    //{
    //    if (alpha <= 0f)
    //        return;
        
    //    alpha -= Time.deltaTime;
    //    _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
    //}

    //private void IncreaseAlpha(float alpha)
    //{
    //    if (alpha >= 1f)
    //        return;

    //    alpha += Time.deltaTime;
    //    _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
    //}

    private void OnDrawGizmosSelected()
    {
        // Draw Attack Points
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireCube(_attackPoint.position, _attackHitBox);
        if (_attackUpPoint == null)
            return;
        Gizmos.DrawWireCube(_attackUpPoint.position, _attackHitBox);

        // Draw Raycasts
        Gizmos.color = Color.red;
        if (_feetPos == null)
            return;
        Gizmos.DrawRay(_feetPos.position, Vector3.down * _checkRadius);
        if (_feetPos2 == null)
            return;
        Gizmos.DrawRay(_feetPos2.position, Vector3.down * _checkRadius);
        if (_facingPos == null)
            return;
        Gizmos.DrawRay(_facingPos.position, transform.right * _checkRadius);
    }

    /// <summary>
    /// Adds a force to the player that pushes them back when they attack
    /// </summary>
    /// <param name="knockbackForce">
    /// The force to add
    /// </param>
    /// <param name="contactPos">
    /// The position from which the player attacked from
    /// </param>
    private void Knockback(Vector2 knockbackForce, Vector2 contactPos)
    {
        _rb.velocity = Vector2.zero;

        if (contactPos.x > transform.position.x)
        {
            _rb.AddForce(new Vector2(-knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);
        }
        else if (contactPos.x < transform.position.x)
        {
            _rb.AddForce(new Vector2(knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Disables the player
    /// </summary>
    public void GameOver()
    {
        FindObjectOfType<UIManagement>().PlayerDead();
    }

    #region Shield
    /// <summary>
    /// Charges the players shield over time if it is lower than the max shield amount
    /// </summary>
    IEnumerator ChargeShield()
    {
        while(CurrentShield < _MaxShield && _shieldCooldown <= 0)
        {
            yield return new WaitForSeconds(1/_ShieldChargeRate);
            CurrentShield++;
            if(CurrentShield == _MaxShield)
            {
                PlayerSound.Play("P_ShieldRegen");
            }
        }
        
            _chargingShield = false;
    }

    /// <summary>
    /// Activates the Shield ability when the player picks up the shield ability
    /// </summary>
    public void ActivateShield()
    {
        CurrentShield = _MaxShield;
        _hasShield = true;
        FindObjectOfType<HealthShield>().GotShield();
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Performs calculation for damage taken
    /// If the players health drops to 0 it calls the Die() method
    /// </summary>
    /// <param name="damageTaken">
    /// The amount of damage the player takes
    /// </param>
    public void TakeDamage(int damageTaken)
    {
        if (!_isInvincible)
        {
            _shieldCooldown = _ShieldDelay;
            _tookDamage = true;
            if(CurrentShield > 0 && _hasShield)
            {
                CurrentShield -= damageTaken;
                if(CurrentShield < 0)
                {
                    TakeDamage(Mathf.Abs(CurrentShield));
                    CurrentShield = 0;
                }
            }
            else
            {
                CurrentHealth -= damageTaken;
            }

            if (CurrentHealth < 1)
            {
                _isDying = true;
                GameOver();
                return;
            }
            PlayerSound.Play("P_Damaged");
            FindObjectOfType<BasicCameraController>().AttackShake();
            StartCoroutine(Flicker());
            StartCoroutine(InvincibilityTime());
        }
        else
            return;
    }

    /// <summary>
    /// Is used by the UI to set the max of the health bar
    /// </summary>
    /// <returns>
    /// Returns the players Start/Max Health
    /// </returns>
    public int GetMaxHealth()
    {
        return _StartHealth;
    }

    /// <summary>
    /// Is used to set the max of the shield bar
    /// </summary>
    /// <returns>
    /// the players max Shield amount
    /// </returns>
    public int GetMaxShield()
    {
        return _MaxShield;
    }

    /// <summary>
    /// Is called on by RoomManagement to tell the player to update it's collsions
    /// It ignores collisions with objects tagged "Enemy"
    /// </summary>
    public void UpdateCollisions()
    {
        ///////////////////////////////////////////////////////
        // Collision detection
        ///////////////////////////////////////////////////////
        GameObject[] _gameObject = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in _gameObject)
        {
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), _collider2D, true);
                Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), _cirCollider2D, true);
            }
        }
    }
    #endregion
}