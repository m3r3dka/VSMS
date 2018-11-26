using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]

public class PoisonBall : NetworkBehaviour
{

    Rigidbody rb;
    [SerializeField] GameObject ExplosionPref;
    public float speed = 20f;
    public int dmg = 100;
    [SerializeField] float lifeTime;
    [SerializeField] float Timer;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 vector = rb.transform.forward * speed;
        rb.MovePosition(rb.position + vector * Time.fixedDeltaTime);

        Timer += Time.deltaTime;

        if (Timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            col.transform.GetComponent<PlayerController>().HP -= dmg;
        }
        if (col.collider.tag == "Floor")
        {
            GameObject Explosion = Instantiate(ExplosionPref, gameObject.transform.position, Quaternion.identity);
            Explosion.transform.Rotate(90, 0, 0);
            NetworkServer.Spawn(Explosion);
            Destroy(gameObject);
        }
    }
}
