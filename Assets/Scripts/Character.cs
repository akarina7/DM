using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Vector2 direction;

    private Animator myAnimator;

    private Rigidbody2D myRigidBody;

    protected bool IsAttacking = false;

    protected Coroutine attackRoutine;
    
    [SerializeField]
    protected Transform hitBox;
    
    [SerializeField]
    protected Stat health;

    [SerializeField] private float initHealth;

    public bool IsMoving
    {
        get { return direction.x != 0 || direction.y != 0; }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        myRigidBody.velocity = direction.normalized * speed;
        
    }

    public void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");
            
            myAnimator.SetLayerWeight(1,1);
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);
            StopAttack(); //remove this if we want will to move and attack at the same time
        }
        else if (IsAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else
        {
            ActivateLayer("IdleLayer");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName),1);
    }

    public void StopAttack()
    {
        StopCoroutine(attackRoutine);
        IsAttacking = false;
        myAnimator.SetBool("attack", IsAttacking);
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("die");
        }
    }
}
