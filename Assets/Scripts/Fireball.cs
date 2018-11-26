using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Rigidbody))]

public class Fireball : NetworkBehaviour {

	Rigidbody rb;
	[SerializeField] GameObject ExplosionPref;
	public float speed = 15f;
    public int dmg = 100;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().HP -= dmg;
            GameObject Explosion = Instantiate(ExplosionPref, gameObject.transform.position, Quaternion.identity);
            NetworkServer.Spawn(Explosion);
            Destroy(gameObject);
        }
        else if (col.tag != "NoneCollision")
        {
            GameObject Explosion = Instantiate(ExplosionPref, gameObject.transform.position, Quaternion.identity);
            NetworkServer.Spawn(Explosion);
            Destroy(gameObject);
        }

    }
}
