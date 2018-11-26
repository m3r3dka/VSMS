using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Rigidbody))]

public class Tornado : NetworkBehaviour
{
	Rigidbody rb;
	//[SerializeField] Transform Explosion;
	public float speed = 7f;
	[SerializeField] float lifeTime;
	[SerializeField] float Timer;
    public float str = 10f;
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
        
		rb.MovePosition (rb.position + Vector3.zero * Time.fixedDeltaTime);
        if (col.tag == "Player")
        {
            print("sas");
            col.gameObject.GetComponent<Rigidbody>().AddForce(0, str, 0);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            print("llllllol");
            collision.collider.gameObject.GetComponent<Rigidbody>().AddForce(0,str,0);
        }
    }

}
