using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour
{
    PlayerMotor motor;

    public float speed = 5f;
    public float HP = 100;
    public float maxHP = 100;

    public bool stuned = false;
    public float StunTimer;
    public float StunLength;

    public float dashSpeed;
    public float DashTimer;
    public float DashLength;
    public bool indash = false;

    public float DeathTimer = 0f;
    public float DeathLength = 10f;
    public bool isDead = false;

    public float ResTimer;
    public float ResLength;
    [SerializeField] GameObject deathElemPref;
    [SerializeField] GameObject ResBeamPref;
    [SerializeField] float mouseSencivity;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        ResTimer += Time.deltaTime;
        if (HP > maxHP) HP = maxHP;
        transform.Find("Canvas").Find("HP").Find("HPbar").GetComponent<Image>().fillAmount = HP / maxHP;
        transform.Find("Canvas").Find("HP").Find("Text").GetComponent<Text>().text = (HP / maxHP * 100).ToString() + "%";

        if (HP <= 0 && !isDead)
            CmdDeath();

        DashTimer += Time.deltaTime;
        
        if (DashLength <= DashTimer && indash)
        {
            speed -= dashSpeed;
            indash = false;
        }
        StunTimer += Time.deltaTime;

        if (StunLength <= StunTimer && stuned)
        {
            speed = 5f;
            stuned = false;
        }
        if (!stuned)
        {
            //////////////////Movement

            float xMove = Input.GetAxisRaw("Horizontal");
            float yMove = Input.GetAxisRaw("Vertical");

            Vector3 MoveHorizontal = transform.right * xMove;
            Vector3 MoveVertical = transform.forward * yMove;

            Vector3 velocity = (MoveHorizontal + MoveVertical).normalized * speed;

            motor.Move(velocity);

            //////////////////Rotation

            float yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0f, yRotation, 0f) * mouseSencivity;

            float xRotation = Input.GetAxisRaw("Mouse Y");
            Vector3 camRotation = new Vector3(xRotation, 0f, 0f) * mouseSencivity;

            motor.Rotate(rotation);
            motor.camRotate(camRotation);
        }
    }
    public void Dash()
    {
        if (!indash)
        {
            DashTimer = 0f;
            indash = true;
            speed += dashSpeed;
        }
    }

    [Command]
    public void CmdDeath()
    {
        DeathTimer = 0;

        isDead = true;
        gameObject.SetActive(false);
        GameObject deathElem = Instantiate(deathElemPref, gameObject.transform.position, Quaternion.identity);
        deathElem.transform.SetPositionAndRotation(new Vector3(0f, 0.3f, 0f), Quaternion.identity);
        deathElem.transform.Rotate(-90, 0, 0);
        NetworkServer.Spawn(deathElem);

        if (DeathLength <= DeathTimer)
        {
            Destroy(gameObject);
        }
    }
    [Command]
    public void CmdResurect()
    {
        ResTimer = 0;
        if (ResLength <= ResTimer)
        {
            isDead = false;
            gameObject.SetActive(true);
            GameObject ResBeam = Instantiate(ResBeamPref, gameObject.transform.position, Quaternion.identity);
            NetworkServer.Spawn(ResBeam);
            HP = maxHP / 2;
        }
        Destroy(deathElemPref);
    }
}
