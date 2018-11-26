using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerMotor : MonoBehaviour 
{
	[SerializeField] Camera cam;

	Rigidbody rb;
	Vector3 velocity = Vector3.zero;
	Vector3 rotation = Vector3.zero;
	Vector3 camRotation = Vector3.zero;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		PerformMove ();
		PerformRotate ();
	}

	//////////////////////////////////Movement

	public void Move(Vector3 velocity)
	{
		this.velocity = velocity;
	}

	void PerformMove()
	{
		if (velocity != Vector3.zero)
			rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
	}
		
	//////////////////////////////////Rotation

	public void Rotate(Vector3 rotation)
	{

		this.rotation = rotation;
	}

	public void camRotate(Vector3 camRotation)
	{

        this.camRotation = camRotation;
	}

	void PerformRotate()
	{
        rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));
		if (cam != null)
			cam.transform.Rotate (-camRotation);
	}


}
