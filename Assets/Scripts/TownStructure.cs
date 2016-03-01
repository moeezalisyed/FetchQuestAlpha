using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownStructure : MonoBehaviour {
    //Constant values for building type
    private const int WORKSHOP = 0;
    private const int BLACKSMITH = 1;
    private const int APOTHECARY = 2;
    private const int TANNERY = 3;
    private const int CHURCH = 4;
    private const int MISCSTRUCTURE = 5;
	private TownStructModel model = null;

	public int updateLevel = 1;
    private List<Hero> employees;

    private int structureType;

	// Created goldreq as a function of goldgenerated. Everytime you need goldGeneration*3 in order to upgrade
	public int goldReq;

    private int goldGeneration;
	public int timer;
	//public Vector3 TownStructurePosition = null;


	private Player owner;

	public GameManager parentGameManager;
	// Use this for initialization



	// Use this for initialization


	private GameManager structManager;

	// Use this for initialization


	public void init (int structType, GameManager m, Player owner) {
		this.structureType = structType;

		this.structManager = m;

		this.owner = owner;


		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<TownStructModel>();						// Add a gemModel script to control visuals of the gem.
		model.init(this, structType);	


		//transform.localPosition = new Vector3 (20, 10, 0);
		//var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

		//mat = GetComponent<Renderer>().material;								// Get the material component of this quad object.
		//mat.mainTexture = Resources.Load<Texture2D>("Textures/testTownStructure0");	// Set the texture.  Must be in Resources folder.
		//	mat.color = new Color(1f,1f,1);											// Set the color (easy way to tint things).
		//mat.shader = Shader.Find ("Sprites/Default");						// Tell the renderer that our textures have transparency. 
		//modelObject.AddComponent<mat> ();



		// Initialized GoldGeneration as this fucntion; I dont know what the better way to design this is. 
		goldGeneration = 100 + (15 * structType % 2);


		StartCoroutine ("addGeneratedGold");
		goldReq = goldGeneration * 3;
	}

	public void updatelevel(){
		int newLevel = updateLevel + 1;
		if (newLevel > 5) {
			newLevel = 5;
		}
		this.goldGeneration = (goldGeneration / updateLevel) * newLevel;
		goldReq = goldGeneration * 3;
		this.updateLevel = newLevel;

	}

	void OnMouseUp () {

		structManager.goldCheckAndUpdate (this);
		//print ("hello");
	}

	


	IEnumerator addGeneratedGold(){
		while (true) {
			structManager.THEPLAYER.Gold += goldGeneration;
			print (goldGeneration);
			yield return new WaitForSeconds (40);
		}
	}




	// Update is called once per frame
	void Update () {

	}
}
