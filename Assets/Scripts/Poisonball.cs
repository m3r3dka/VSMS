using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Rigidbody))]

public class Poisonball : NetworkBehaviour
 {

    Rigidbody rb;
    [SerializeField] public GameObject PoisonblastPref;
    public float speed = 13f;
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
        rb.MovePosition(rb.position + vector*Time.fixedDeltaTime);

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
        }
        GameObject Explosion = Instantiate(PoisonblastPref, gameObject.transform.position, new Quaternion());
        Explosion.transform.Rotate(new Vector3(90,0,0));
        NetworkServer.Spawn(Explosion);
        Destroy(gameObject);
    }
}
