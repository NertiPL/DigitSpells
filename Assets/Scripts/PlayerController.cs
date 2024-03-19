using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;

    public float stamina = 100f;
    public float staminaCost = 20f;

    public float hp = 100f;

    public float dashPower= 20f;

    Vector2 maxStaminaBarSize;
    Vector2 maxStaminaPos;

    Vector2 maxHpBarSize;
    Vector2 maxHpPos;

    bool canDash=true;
    public bool isDashing = false;

    public Transform topOfStaff;

    public LayerMask enemies;

    public Animator animator;

    GameObject currentAttack;

    void Start()
    {
        rb= GetComponent<Rigidbody>();

        maxStaminaBarSize = GameManager.instance.staminaBar.rectTransform.sizeDelta;
        maxStaminaPos = GameManager.instance.staminaBar.rectTransform.position;

        maxHpBarSize = GameManager.instance.healthBar.rectTransform.sizeDelta;
        maxHpPos = GameManager.instance.healthBar.rectTransform.position;
    }

    void Update()
    {

        Movement();

        BarUpdater();

        CheckDeath();
    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity += transform.forward * -1 * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity += transform.right * -1 * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += transform.right * 1 * speed * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina>=staminaCost&& canDash)
        {
            Dash();          
        }

        Attack();

    }

    void Dash()
    {
        CancelInvoke("RegenStamina");
        stamina -= staminaCost;
        canDash = false;
        isDashing = true;

        GetComponent<Collider>().excludeLayers=enemies;

        rb.AddForce(rb.velocity * dashPower, ForceMode.Impulse);

        Invoke("CanDashAgain", 2f);
        Invoke("StoppedDashing", 1f);
    }
    void BarUpdater()
    {
        //stamina
        GameManager.instance.staminaBar.rectTransform.sizeDelta = new Vector2(maxStaminaBarSize.x * stamina / 100, maxStaminaBarSize.y);
        GameManager.instance.staminaBar.rectTransform.position = new Vector2((maxStaminaBarSize.x * stamina / 100)/2 +19, maxStaminaPos.y);

        //health
        GameManager.instance.healthBar.rectTransform.sizeDelta = new Vector2(maxHpBarSize.x * hp / 100, maxHpBarSize.y);
        GameManager.instance.healthBar.rectTransform.position = new Vector2((maxHpBarSize.x * hp / 100) / 2 + 19, maxHpPos.y);
    }

    void CanDashAgain()
    {
        canDash = true;
        InvokeRepeating("RegenStamina", 5f, 0.5f);
    }
    void StoppedDashing()
    {
        isDashing = false;
        GetComponent<Collider>().excludeLayers = 0;
    }
    

    void RegenStamina()
    {
        stamina += 5f;

        if (stamina >= 100)
        {
            stamina = 100;
            CancelInvoke("RegenStamina");
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(GameManager.instance.chosenSpells[0].name);
            var attack = Instantiate(GameManager.instance.chosenSpells[0].prefab, topOfStaff.position, topOfStaff.rotation);
            attack.GetComponent<MonoBehaviour>().enabled = false;
            animator.Play("StaffAnimation");    
            currentAttack = attack;            
        }

        if(currentAttack != null)
        {
            currentAttack.transform.position = topOfStaff.position;
        }
        
    }

    public void GetHit(float dmg)
    {
        hp -= dmg;
    }

    void CheckDeath()
    {
        if(hp <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void midAnimShoot()
    {
        currentAttack.GetComponent<MonoBehaviour>().enabled = true;
        currentAttack = null;
    }


}
