using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Will_Movement : MonoBehaviour
{
    public Rigidbody2D rbody;
    public float speed;
    public Animator anim;
//    public VectorValue startingPosition;
    [SerializeField]
//    private Stat health; //originally private but made public static to access it in health consumable.cs
//    private float initialHealth = 100;
    protected bool isAttacking = false;
    protected Coroutine attackRoutine;
    [SerializeField] private GameObject[] attackPrefab;
    
    public Transform MyTarget{ get; set; }
    
//    //debugging
//    private Transform target;
 //   void Start()
 //   {
 //       transform.position = startingPosition.initialValue;
//        health.Initialize(initialHealth,initialHealth);

//        target = GameObject.Find("Target").transform;
 //   }
    
    public bool stopMovement()
    {
        bool inAction;
        if (DialogueManager.instance.inDialogue)
        {
            inAction = true;
        }
        else
        {
            inAction = false;
        }

        return inAction;
    }

    private void Update()
    {
        //health below is debugging
//        if (Input.GetKeyDown(KeyCode.O))
//        {
//            health.MyCurrentValue -= 10;
//        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shootWeapon();
        }

        if (stopMovement()) return;
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movement != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            
            StopAttack();
        }

        else
        {
            anim.SetBool("isWalking", false);
        }

        rbody.MovePosition(rbody.position + movement * Time.deltaTime * speed);
    }

    public IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(1); //hardcoded

        if (MyTarget != null)
        {
            Shoot s = Instantiate(attackPrefab[0], transform.position, Quaternion.identity).GetComponent<Shoot>();
            //s.MyTarget = MyTarget;
            s.Initialize(MyTarget, 20);
        }

        StopAttack();
        
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            anim.SetBool("attack", isAttacking);
        }
    }

    //if you change the weight to 1 the attack will work but otherwise won't work,
    //so haven't actually called the following function
    public void HandleLayers()
    {
        if (isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(anim.GetLayerIndex(layerName), 1);
    }
    
    void shootWeapon()
    {
        if (MyTarget != null && !isAttacking)
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }

//    private bool InLineofSight()
//    {
//        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
//        
//    }
}
