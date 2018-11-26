using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]

public class ElectricBall : NetworkBehaviour
{
    
    Rigidbody rb;
    [SerializeField] GameObject ExplosionPref;
    [SerializeField] float speed;
    public int dmg = 50;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().HP -= dmg;
            col.GetComponent<PlayerController>().stuned = true;
            col.GetComponent<PlayerController>().speed = 0;
            col.GetComponent<PlayerController>().StunTimer = 0;
            col.GetComponent<PlayerController>().StunLength = 5;
        }
        GameObject Explosion = Instantiate(ExplosionPref, gameObject.transform.position, Quaternion.identity);
        NetworkServer.Spawn(Explosion);
        Destroy(gameObject);
    }
}
