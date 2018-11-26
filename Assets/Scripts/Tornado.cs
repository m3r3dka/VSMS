using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class Tornado : MonoBehaviour {
	Rigidbody rb;
	//[SerializeField] Transform Explosion;
	public float speed = 7f;
    public float strength = 50f;
    [SerializeField] float lifeTime;
	[SerializeField] float Timer;

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () 
	{
		Vector3 vector = rb.transform.forward * speed;
		rb.MovePosition (rb.position + vector * Time.fixedDeltaTime);

		Timer += Time.deltaTime;

		if (Timer >= lifeTime) 
		{
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider col)
	{
        if (col.tag == "Player")
        {
            rb.MovePosition(rb.position + Vector3.zero * Time.fixedDeltaTime);
            col.GetComponent<Rigidbody>().AddForce(transform.up * strength);
        }
	}
}
