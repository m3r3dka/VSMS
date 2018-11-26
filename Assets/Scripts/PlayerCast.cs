using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCast : NetworkBehaviour 
{
	PlayerController playerControl;

	[SerializeField] GameObject FireballPref;
	[SerializeField] GameObject DashPref;
	[SerializeField] GameObject TornadoPref;
    [SerializeField] GameObject ElectricballPref;
    [SerializeField] GameObject PoisonballPref;
    [SerializeField] Transform spawnPoint;
	[SerializeField] Transform dustSpawnPoint;
    //List Of Elems
    /*
     * 0 - Fire
     * 1 - Water
     * 2 - Wind
     * 3 - Storm
     * 4 - 
     * 5 - 
     * 6 - 
     * 7 - 
     * 8 - 
     * 9 - 
     * 10 - 
     * 11 - 
     */
    [SerializeField] const string FireballStr = "0 0 0 "	;
	[SerializeField] const string DashStr 	= "2 2 "	;
	[SerializeField] const string TornadoStr 	= "2 3 2 3 ";
    [SerializeField] const string ElectricballStr = "3 3 3 ";
    [SerializeField] const string PoisonballStr = "3 3 3 3 ";
    void Start()
	{
		playerControl = gameObject.GetComponent<PlayerController> ();
	}

	void Update () 
	{
		
	}

	public void Cast(List<int> spell)
	{
		string spellPhrase="";

		for (int i = 0; i < spell.Count; i++) 
		{
			spellPhrase += Convert.ToString(spell[i]) + " ";
		}
        switch (spellPhrase)
        {

            case FireballStr:
                CmdFireballCast();
                break;

            case DashStr:
                CmdDashCast();
                break;

            case TornadoStr:
                CmdTornadoCast();
                break;

            case ElectricballStr:
                CmdElectricballCast();
                break;
            case PoisonballStr:
                CmdPoisonballCast();
                break;
            default:
                break;
        }
	}
    [Command]
    void CmdPoisonballCast()
    {
        GameObject Poisonball = Instantiate(PoisonballPref, spawnPoint.transform.position, spawnPoint.transform.rotation);
        NetworkServer.Spawn(Poisonball);
    }
    [Command]
	void CmdFireballCast()
	{
		GameObject Fireball = Instantiate (FireballPref, spawnPoint.transform.position, spawnPoint.transform.rotation);
		NetworkServer.Spawn (Fireball);
	}
    [Command]
    void CmdElectricballCast()
    {
        GameObject Electricball = Instantiate(ElectricballPref, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z+1), spawnPoint.transform.rotation, transform);
        NetworkServer.Spawn(Electricball);
    }

    [Command]
	void CmdDashCast()
	{
		GameObject DashObj = Instantiate (DashPref, dustSpawnPoint.transform.position, Quaternion.identity);
		NetworkServer.Spawn (DashObj);
		playerControl.CmdDash();
	}

	[Command]
	void CmdTornadoCast()
	{
		GameObject Tornado = Instantiate (TornadoPref, spawnPoint.transform.position, spawnPoint.transform.rotation);
		NetworkServer.Spawn (Tornado);
	}
}
