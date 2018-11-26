using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDMG : MonoBehaviour {

    public int dmg = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().HP -= dmg;
        }
    }
}
