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

    public float dashPower= 20f;

    Vector2 maxStaminaBarSize;
    Vector2 maxStaminaPos;

    bool canDash=true;
    void Start()
    {
        rb= GetComponent<Rigidbody>();

        maxStaminaBarSize = GameManager.instance.staminaBar.rectTransform.sizeDelta;
        maxStaminaPos = GameManager.instance.staminaBar.rectTransform.position;
    }

    void Update()
    {

        Movement();

        BarUpdater();
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

        rb.AddForce(rb.velocity * dashPower, ForceMode.Impulse);

        Invoke("CanDashAgain", 2f);
    }
    void BarUpdater()
    {
        GameManager.instance.staminaBar.rectTransform.sizeDelta = new Vector2(maxStaminaBarSize.x * stamina / 100, maxStaminaBarSize.y);
        GameManager.instance.staminaBar.rectTransform.position = new Vector2((maxStaminaBarSize.x * stamina / 100)/2 +19, maxStaminaPos.y);
    }

    void CanDashAgain()
    {
        canDash = true;
        InvokeRepeating("RegenStamina", 5f, 0.5f);
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

    // TUUUUUU
    void Attack()
    {
        
    }
}
