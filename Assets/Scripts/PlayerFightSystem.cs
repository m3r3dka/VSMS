using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightSystem : MonoBehaviour 
{
	PlayerCast pc;
	int element = 0 ;

	private List<int> spell = new List<int>();

	[SerializeField] Image[] im;
	[SerializeField] Sprite[] Elems;
    //List Of Elems
    /*
     * 0 - Dark
     * 1 - Fire
     * 2 - Wind
     * 3 - Storm
     * 4 - Light
     * 5 - Water
     * 6 - Earth
     * 7 - Grass
     */

    [SerializeField] Sprite[] Prints;
	[SerializeField] Sprite defSprite;

	void Start()
	{
		pc = gameObject.GetComponent<PlayerCast> ();
	}
	void Update () 
	{
		if (spell.Count > 4) 
		{
			spell.Clear();
		}

		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) 
		{
			pc.Cast (spell);
			spell.Clear();
		}

		if (Input.GetKeyDown(KeyCode.Tab)) 
		{
			element++;
			if (element % 2 == 0) 
			{
				element = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			switch (element) 
			{
			case 0:
				spell.Add(0);
				break;
			case 1:
				spell.Add(4);
				break;
			case 2:
				spell.Add(8);
				break;
			default:
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			switch (element) 
			{
			case 0:
				spell.Add(1);
				break;
			case 1:
				spell.Add(5);
				break;
			case 2:
				spell.Add(9);
				break;
			default:
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			switch (element) 
			{
			case 0:
				spell.Add(2);
				break;
			case 1:
				spell.Add(6);
				break;
			case 2:
				spell.Add(10);
				break;
			default:
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			switch (element) 
			{
			case 0:
				spell.Add(3);
				break;
			case 1:
				spell.Add(7);
				break;
			case 2:
				spell.Add(11);
				break;
			default:
				break;
			}
		}

		Draw();
	}

	void Draw()
	{
		im [0].GetComponent<Image> ().sprite = Elems [element];
		if (spell.Count == 0) 
		{
			int j = 0;
			while (j < 4) 
			{ 
				j += 1;
				im [j].GetComponent<Image> ().sprite = defSprite;

			}
		}
		for (int i = 0, j = 0; i < spell.Count; i++) 
		{
			j = i+1;
			if (i + 1 > 4)
				break;
			else
				im [j].GetComponent<Image> ().sprite = Prints [spell [i]];
			while (j < 4) 
			{ 
				j += 1;
				im [j].GetComponent<Image> ().sprite = defSprite;

			}
		}
	}
}
