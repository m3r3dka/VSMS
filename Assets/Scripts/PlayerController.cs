using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour 
{
	public PlayerMotor motor;


    public float speed = 5f;
	public int HP = 100;
    public int mHP = 100;

    public bool stuned = false;
    public float StunTimer;
    public float StunLength;

    public float dashSpeed ;
	public float DashTimer;
	public float DashLength ;
	public bool indash = false;

	public float DeathTimer = 0f;
	public float DeathLength=10f;
	public bool isDead = false;

	[SerializeField] GameObject deathElemPref;
	[SerializeField] float mouseSencivity;

	void Start()
	{
		motor = GetComponent <PlayerMotor>();
    }

	void Update()
	{
		if (HP <= 0 && !isDead)
			CmdDeath ();

		DashTimer += Time.deltaTime;
		if (DashLength <= DashTimer && indash) 
		{
			speed -= dashSpeed;
			indash = false;
		}
        StunTimer += Time.deltaTime;

        if (StunLength <= StunTimer && stuned)
            stuned = false;

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

		float yRotation = Input.GetAxisRaw ("Mouse X");
		Vector3 rotation = new Vector3 (0f, yRotation, 0f) * mouseSencivity;

		float xRotation = Input.GetAxisRaw ("Mouse Y");
		Vector3 camRotation = new Vector3 (xRotation, 0f, 0f) * mouseSencivity;

		motor.Rotate (rotation);
		motor.camRotate (camRotation);
        }
    }
	[Command]
	public void CmdDash()
	{
		DashTimer = 0f;
		indash = true;
		speed += dashSpeed;
	}

	[Command]
	public void CmdDeath ()
	{
		isDead = true;
		Destroy (gameObject);
		GameObject deathElem = Instantiate (deathElemPref, gameObject.transform.position + new Vector3 (0f, 0f, 0f), new Quaternion(-90,0,0,0));
		NetworkServer.Spawn (deathElem);
	}
}
