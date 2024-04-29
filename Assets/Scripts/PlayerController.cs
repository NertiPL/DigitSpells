using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;


    public float sensitivity = 100f;
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

    public Animator animator;

    GameObject currentAttack;

    public CinemachineVirtualCamera vCam;

    public bool canUseAbility=true;

    //bools for cooldown on each button

    public List<bool> btnsOnCooldown;

    IEnumerable cooldownCoroutine;

    void Start()
    {
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

    void Update()
    {

        Movement();

        BarUpdater();

        CheckDeath();

        CorrectHp();
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

        Rotation();
        Attack();

    }

    void Rotation()
    {
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.fixedDeltaTime *sensitivity);


            vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset += new Vector3(0, 0, -1*Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity);
            vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset += new Vector3(0,  -1*Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity,0);

            if (vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z > -0.1 ){
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, -0.1f);
            }
            else if(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z < -5)
            {
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, -5f);
            }

            if (vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y > 3.9)
            {
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,3.9f, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z);
            }
            else if(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y < 1.8f)
            {
                vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, 1.8f, vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z);
            }


        }
    }

    void Dash()
    {
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
            if (Input.GetKeyDown(KeyCode.Q) && !btnsOnCooldown[0])
            {
                AttackFormula(0);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                AttackFormula(1);

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
        GameObject attack;

        if (GameManager.instance.chosenSpells[chosenSpellsIndex].onTopOnStaff)
        {
            attack = Instantiate(GameManager.instance.chosenSpells[chosenSpellsIndex].prefab, topOfStaff.position, topOfStaff.rotation);
        }
        else
        {
            attack = Instantiate(GameManager.instance.chosenSpells[chosenSpellsIndex].prefab, transform.position + transform.forward, transform.rotation);
        }

        attack.GetComponent<MonoBehaviour>().enabled = false;
        animator.Play("StaffAnimation");
        currentAttack = attack;
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
        hp -= dmg;
    }

    void CheckDeath()
    {
        if(hp <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void MidAnimShoot()
    {
        currentAttack.GetComponent<MonoBehaviour>().enabled = true;
        currentAttack = null;
    }

    public void EndAnimShoot()
    {
        canUseAbility = true;
    }


}
