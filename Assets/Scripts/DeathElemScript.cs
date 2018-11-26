using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeathElemScript : NetworkBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float Timer;
    public int heal = 20;
    void Start()
    {}

    void FixedUpdate()
    {
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
            col.GetComponent<PlayerController>().HP += heal;
        }
        Destroy(gameObject);
    }
}
