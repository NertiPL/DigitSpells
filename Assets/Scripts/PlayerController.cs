using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    public float maxVelocity;

    //public float sensitivity = 100f;
    public float speed = 10f;

    public float ogSpeed;

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

    public Animator animator;

    GameObject currentAttack;

    public CinemachineVirtualCamera vCam;

    public bool canUseAbility=true;

    //bools for cooldown on each button

    public List<bool> btnsOnCooldown;

    IEnumerable cooldownCoroutine;

    bool isWalking;

    bool isAttacking = false;
    bool isOnTopOfStaff = false;

    void Start()
    {
        ogSpeed = speed;
        rb= GetComponent<Rigidbody>();

        maxStaminaBarSize = GameManager.instance.staminaBar.rectTransform.sizeDelta;
        maxStaminaPos = GameManager.instance.staminaBar.rectTransform.position;

        maxHpBarSize = GameManager.instance.healthBar.rectTransform.sizeDelta;
        maxHpPos = GameManager.instance.healthBar.rectTransform.position;

        btnsOnCooldown = new List<bool>();
        for(int i = 0; i < 8; i++)
        {
            btnsOnCooldown.Add(false);
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }
    void Update()
    {
        BarUpdater();

        CheckDeath();

        CorrectHp();

        Rotation();

        StickToTopOfStaff();
    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            GameManager.instance.SFX.clip = GameManager.instance.soundEffects[5];
            GameManager.instance.SFX.Play();
            rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
            if (!isWalking && !isAttacking)
            {
                isWalking = true;
                animator.Play("Walk");
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            GameManager.instance.SFX.clip = GameManager.instance.soundEffects[5];
            GameManager.instance.SFX.Play();
            rb.velocity += transform.forward * -1 * speed * Time.fixedDeltaTime;
            if (!isWalking && !isAttacking)
            {
                isWalking = true;
                animator.Play("Walk");
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            GameManager.instance.SFX.clip = GameManager.instance.soundEffects[5];
            GameManager.instance.SFX.Play();
            rb.velocity += transform.right * -1 * speed * Time.fixedDeltaTime;
            if (!isWalking && !isAttacking)
            {
                isWalking = true;
                animator.Play("Walk");
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            GameManager.instance.SFX.clip = GameManager.instance.soundEffects[5];
            GameManager.instance.SFX.Play();
            rb.velocity += transform.right * 1 * speed * Time.fixedDeltaTime;
            if (!isWalking && !isAttacking)
            {
                isWalking = true;
                animator.Play("Walk");
            }
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !isAttacking)
        {
            isWalking = false;
            animator.Play("Idle");
        }

        VelocitySuppressor();

        if (Input.GetKey(KeyCode.LeftShift) && stamina>=staminaCost&& canDash)
        {
            Dash();          
        }

        Attack();

    }

    void VelocitySuppressor()
    {
        if (rb.velocity.x > maxVelocity)
        {
            rb.velocity = new Vector3(maxVelocity, rb.velocity.y, rb.velocity.z);
        }
        if (rb.velocity.y > maxVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxVelocity, rb.velocity.z);
        }
        if (rb.velocity.z > maxVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxVelocity);
        }
        if (rb.velocity.x < -maxVelocity)
        {
            rb.velocity = new Vector3(-maxVelocity, rb.velocity.y, rb.velocity.z);
        }
        if (rb.velocity.y < -maxVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxVelocity, rb.velocity.z);
        }
        if (rb.velocity.z < -maxVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxVelocity);
        }
    }

    void Rotation()
    {
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * Time.deltaTime * GameManager.instance.sensitivityX *2, 0));


            vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset += new Vector3(0, 0, -1*Input.GetAxis("Mouse Y") * Time.deltaTime * GameManager.instance.sensitivityY);
            vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset += new Vector3(0,  -1*Input.GetAxis("Mouse Y") * Time.deltaTime * GameManager.instance.sensitivityY, 0);

            if (vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z > -0.1 ){
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, -0.1f);
            }
            else if(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z < -5)
            {
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, -5f);
            }

            if (vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y > 7f)
            {
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,7f, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z);
            }
            else if(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y < 1.8f)
            {
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, 1.8f, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z);
            }


        }
    }

    void Dash()
    {
        animator.Play("Dash");
        GameManager.instance.SFX.clip = GameManager.instance.soundEffects[6];
        GameManager.instance.SFX.Play();
        CancelInvoke("RegenStamina");
        stamina -= staminaCost;
        canDash = false;
        isDashing = true;

        GetComponent<Collider>().excludeLayers=GameManager.instance.enemies;

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

    void CorrectHp() 
    {
        if(hp > 100)
        {
            hp = 100;
        }
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
        if(canUseAbility)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !btnsOnCooldown[0] && GameManager.instance.chosenSpells[0] != null)
            {
                AttackFormula(0);
            }

            if (Input.GetKeyDown(KeyCode.E) && !btnsOnCooldown[1] && GameManager.instance.chosenSpells[1] != null)
            {
                AttackFormula(1);

            }
            if (Input.GetKeyDown(KeyCode.R) && !btnsOnCooldown[2] && GameManager.instance.chosenSpells[2] != null)
            {
                AttackFormula(2);

            }
            if (Input.GetKeyDown(KeyCode.F) && !btnsOnCooldown[3] && GameManager.instance.chosenSpells[3] != null)
            {
                AttackFormula(3);

            }
            if (Input.GetKeyDown(KeyCode.C) && !btnsOnCooldown[4] && GameManager.instance.chosenSpells[4]!=null)
            {
                AttackFormula(4);

            }

            if (currentAttack != null)
            {
                currentAttack.transform.position = topOfStaff.position;
            }
        }
        
    }

    void AttackFormula(int chosenSpellsIndex)
    {
        canUseAbility = false;
        Debug.Log(GameManager.instance.chosenSpells[chosenSpellsIndex].name);

        GameManager.instance.miniGamePanel.GetComponent<MathMinigameScript>().SlideUp(chosenSpellsIndex, GameManager.instance.chosenSpells[chosenSpellsIndex].difficulty);

        Cooldown(chosenSpellsIndex, GameManager.instance.chosenSpells[chosenSpellsIndex].cd);
    }

    public void ProceedAttack(int chosenSpellsIndex)
    {
        isAttacking = true;
        GameObject attack;

        if (GameManager.instance.chosenSpells[chosenSpellsIndex].onTopOnStaff)
        {
            isOnTopOfStaff = true;
            attack = Instantiate(GameManager.instance.chosenSpells[chosenSpellsIndex].prefab, topOfStaff.position, topOfStaff.rotation);
        }
        else
        {
            isOnTopOfStaff=false;
            attack = Instantiate(GameManager.instance.chosenSpells[chosenSpellsIndex].prefab, transform.position + transform.forward, transform.rotation);
        }

        attack.GetComponent<MonoBehaviour>().enabled = false;
        animator.Play("Attack");
        currentAttack = attack;
        GameManager.instance.SFX.clip = GameManager.instance.soundEffects[0];
        GameManager.instance.SFX.Play();
    }

    void StickToTopOfStaff()
    {
        if(isAttacking && currentAttack!=null && isOnTopOfStaff)
        {
            currentAttack.transform.position=topOfStaff.position;
            currentAttack.transform.rotation=transform.rotation;
        }
    }

    public void StartCooldown(int whichBtnOnCdId, float timeOfCd)
    {
        cooldownCoroutine = Cooldown(whichBtnOnCdId, timeOfCd);
        StartCoroutine(cooldownCoroutine.GetEnumerator());
    }

    IEnumerable Cooldown(int whichBtnOnCdId, float timeOfCd)
    {
        Debug.Log("a");
        btnsOnCooldown[whichBtnOnCdId] = true;
        yield return new WaitForSeconds(timeOfCd);
        btnsOnCooldown[whichBtnOnCdId] = false;
    }

    public void GetHit(float dmg)
    {
        GameManager.instance.SFX.clip = GameManager.instance.soundEffects[1];
        GameManager.instance.SFX.Play();
        hp -= dmg;
    }

    void CheckDeath()
    {
        if(hp <= 0)
        {
            GameManager.instance.SFX.clip = GameManager.instance.soundEffects[7];
            GameManager.instance.SFX.Play();
            GameManager.instance.GameOver();
        }
    }

    public void MidAnimShoot()
    {
        currentAttack.GetComponent<MonoBehaviour>().enabled = true;
        currentAttack = null;
        isAttacking = false;
    }

    public void EndAnimShoot()
    {
        canUseAbility = true;
    }


}
