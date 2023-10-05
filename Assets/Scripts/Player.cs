using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public int curHp;
    public int maxHp;

    public float moveSpeed;
    public float jumpForce;

    public float attackRange;
    public int damage;
    private bool isAttacking;

    public Rigidbody rig;
    public Animator anim;
    public bool death;

    public Transform GameOver;
    
    private void Start()
    {
        death = false;
    }
    void Update ()
    {
        Move();

        // jumping
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();

        // check for when we press the attack key
        if(Input.GetMouseButtonDown(0) && !isAttacking)
            Attack();

        // if we're not attacking, update the animator
        if(!isAttacking)
            UpdateAnimator();
    }

    // moves the player around
    void Move ()
    {
        // get the movement inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // create a direction vector
        Vector3 dir = transform.right * x + transform.forward * z;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;

        // set our velocity
        rig.velocity = dir;
    }

    // update the movement related animations
    void UpdateAnimator ()
    {
        anim.SetBool("MovingForwards", false);
        anim.SetBool("MovingBackwards", false);
        anim.SetBool("MovingLeft", false);
        anim.SetBool("MovingRight", false);

        Vector3 localVel = transform.InverseTransformDirection(rig.velocity);

        if(localVel.z > 0.1f)
            anim.SetBool("MovingForwards", true);
        else if(localVel.z < -0.1f)
            anim.SetBool("MovingBackwards", true);
        else if(localVel.x < -0.1f)
            anim.SetBool("MovingLeft", true);
        else if(localVel.x > 0.1f)
            anim.SetBool("MovingRight", true);
    }

    // called when we press the jump button
    void Jump ()
    {
        // if we can jump, add force upwards
        if(CanJump())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // returns true or false for if we can jump or not
    bool CanJump ()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 0.1f))
        {
            return hit.collider != null;
        }

        return false;
    }

    // called when the enemy attacks us
    public void TakeDamage (int damageToTake)
    {
        curHp -= damageToTake;

        // update the UI health bar
        HealthBarUI.instance.UpdateFill(curHp, maxHp);

        // reset the scene when we die
        if(curHp <= 0)
        {
            SoundManager.Instance.PlayDeath();
            GameOver.gameObject.SetActive(true);
            anim.SetBool("death", true);
            // Destroy(this.gameObject, 2.2f);
            this.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            death = true;
            GameOver.DOScale(0.83207f, .2f).SetEase(Ease.Flash);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // called when we press the attack key
    void Attack ()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        SoundManager.Instance.PlaySwrodAttack();
        Invoke("TryDamage", 0.7f);
        Invoke("DisableIsAttacking", 1.5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("potion"))
        {
            curHp += 1;
            Destroy(other.gameObject);
            // update the UI health bar
            HealthBarUI.instance.UpdateFill(curHp, maxHp);
        }
    }
    // called when the animation is at a point of attacking the enemy
    void TryDamage ()
    {
        // check to see if there are any enemies in front of us
        Ray ray = new Ray(transform.position + transform.forward, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, attackRange, 1 << 8);

        // attack each of them
        foreach(RaycastHit hit in hits)
        {
            hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }

    // called after the attack animation
    void DisableIsAttacking ()
    {
        isAttacking = false;
    }
}