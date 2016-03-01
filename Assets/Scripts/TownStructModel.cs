using UnityEngine;
using System.Collections;

public class TownStructModel : MonoBehaviour {

	private TownStructure own;

	private Material mat;

	// Use this for initialization
	public void init(TownStructure owner, int TownStructure){
		this.own = owner;


		transform.parent = own.transform;					// Set the model's parent to the gem.
		System.Random rnd = new System.Random();
		int x = rnd.Next (-7, 7);
		int y = rnd.Next (-7, 7);

		transform.localPosition = new Vector3(x,y,-1);		// Center the model on the parent.
		name = "Struct Model";									// Name the object.

		mat = GetComponent<Renderer>().material;								// Get the material component of this quad object.
		mat.mainTexture = Resources.Load<Texture2D>("TextureFold/testtownstructure"+TownStructure);	// Set the texture.  Must be in Resources folder.
			mat.color = new Color(1f,1f,1);											// Set the color (easy way to tint things).
		mat.shader = Shader.Find ("Sprites/Default");	

	}
}
