using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
	public GameObject HeroFolder;
	public GameObject PlayerFolder;
	public HashSet<Hero> HeroesSet;
	public HashSet<Quest> QuestsSet;
	public HashSet<Quest> AvailableQuestsSet;
	public HashSet<Hero> AvailableHeroesSet;
	public HashSet<Quest> QuestsInProgress;
	public HashSet<TownStructure> TownStructureSet;
	public Player THEPLAYER;
	public GameObject QuestFolder;
	public GameObject TownStructureFolder;
	public List<int> InitialGoldReqForTownStructures;
	public Hero HeroToDisplay = null;
	public int NumberOfExactHeroes = 0;
	public LinkedList<Hero> haftq = null;
	bool selectionHeroes = false;
	bool updateSucc = false;
	bool updateFail =  false;


	public bool HeroDisplay = false;
	public List<Hero> UserSelectedHeroes = null;
	//[SerializeField] private Scrollbar Scrollbarvertical=null;
	//[SerializeField] private Scrollbar Scrollbarhorizontal=null;



	/*Buttons for Standard (init) Panel*/
	[SerializeField] private Button BuyButton = null;
	[SerializeField] private Button UpgradeButton = null;
	[SerializeField] private Button HeroesButton = null;
	[SerializeField] private Button QuestsButton = null;

	/*Buttons for Buy Panel*/
	[SerializeField] private Button BuyBuildingsButton = null;


	/*Buttons for Buy Buildings Panel*/
	[SerializeField] private Button BuyWorkshop = null;
	[SerializeField] private Button BuyBlacksmith = null;
	[SerializeField] private Button BuyApothecary = null;
	[SerializeField] private Button BuyTannery = null;
	[SerializeField] private Button BuyChurch = null;
	[SerializeField] private Button BuyMiscStructures = null;
	[SerializeField] private Button DestroyBuilding = null;

	//TODO
	//1. Create a function that all of the buy buildings button can feed into-- DONE
	//2. Create a dynamically sizing list of buttons with a scroll element --DONE
	//3. Fill the AvailableHeroesSet, add a 'name' component, 'class' component, and 'level component'
				/**********/

	/*initializing the various panels*/
	public GameObject initPanel = null;
	public GameObject BuyPanel = null;
	public GameObject BuyBuildingsPanel = null;
	public GameObject HeroPanel = null;
	public GameObject QuestPanel = null;

	public GameObject prefabButton;
	public RectTransform ScrollbarHeroes;
	public RectTransform ScrollbarQuests;


	/*Initializing the various Texts*/
	public Text UpgradeButtonActiveText=null;
	public Text BuildOnPlotText = null;




	// Use this for initialization


	Player PlayerCharacter;

	void Start () {
		HeroFolder = new GameObject();  
		HeroFolder.name = "Heroes";		// The name of a game object is visible in the hHerarchy pane.
		QuestFolder = new GameObject();  
		QuestFolder.name = "Quests";		// The name of a game object is visible in the hHerarchy pane.
		PlayerFolder = new GameObject();  
		PlayerFolder.name = "Player";		// The name of a game object is visible in the hHerarchy pane.
		TownStructureFolder = new GameObject();  
		TownStructureFolder.name = "TownStructures";		// The name of a game object is visible in the hHerarchy pane.
		HeroesSet = new HashSet<Hero>();
		QuestsSet = new HashSet<Quest>();
		AvailableQuestsSet = new HashSet<Quest>();
		AvailableHeroesSet = new HashSet<Hero> ();
		QuestsInProgress = new HashSet<Quest> ();
		TownStructureSet = new HashSet<TownStructure> ();
		THEPLAYER = null;
		initialisePlayer ();
		initialiseHeroes ();
		initialiseQuests ();



		/*Hiding the appropriate text windows*/
		UpgradeButtonActiveText.GetComponent <Text> (); //When the Upgrade button is clicked
		UpgradeButtonActiveText.gameObject.SetActive (false);
		BuildOnPlotText.GetComponent<Text> (); //When goldCheckandBuy is successful
		BuildOnPlotText.gameObject.SetActive(false);

		//-150 x, -30 y
		//UpgradeButtonActiveText.GetComponent<Text>().enabled = false;
		//UpgradeButtonActiveText.gameObject.SetActive (false);


		//Initial requirements for buying a TownStructure
		initializeInitialGoldReqForTownStructures ();






		initPanel = GameObject.Find("Standard Panel"); /*Initial panel*/
		BuyPanel = GameObject.Find ("Buy Panel"); /*Buy Window*/
		BuyBuildingsPanel = GameObject.Find ("List of Buildings Panel"); /*List of buildings to buy*/
		initPanel.SetActive (true); //initpanel sets to true

		/*All of the other panels set to false*/
		BuyPanel.SetActive (false);
		BuyBuildingsPanel.SetActive (false);

		/*Setting up the buttons*/

		/*Standard Panel Button Activation*/
		BuyButton.GetComponent<Button> ();
		BuyButton.onClick.AddListener(() => BuyButtonActive());
		UpgradeButton.GetComponent<Button> ();
		UpgradeButton.onClick.AddListener (() => UpgradeButtonActive ());
		HeroesButton.GetComponent<Button> ();
		HeroesButton.onClick.AddListener (() => HeroesButtonActive ());
		QuestsButton.GetComponent<Button> ();
		QuestsButton.onClick.AddListener (() => QuestsButtonActive ());
				/************/


		/*Buy Panel Button Activation*/
		BuyBuildingsButton.GetComponent<Button> ();
		BuyBuildingsButton.onClick.AddListener (() => BuyBuildingsActive ());


		/*Buy Buildings Button Activation*/
		BuyWorkshop.GetComponent<Button> ();
		BuyWorkshop.onClick.AddListener (() => BuyBuildings (0));
		BuyBlacksmith.GetComponent<Button> ();
		BuyBlacksmith.onClick.AddListener (() => BuyBuildings (1));
		BuyApothecary.GetComponent<Button> ();
		BuyApothecary.onClick.AddListener (() => BuyBuildings (2));
		BuyTannery.GetComponent<Button> ();;
		BuyTannery.onClick.AddListener (() => BuyBuildings (3));
		BuyChurch.GetComponent<Button> ();
		BuyChurch.onClick.AddListener (() => BuyBuildings (4));
		BuyMiscStructures.GetComponent<Button> ();
		BuyMiscStructures.onClick.AddListener (() => BuyBuildings (5));

		//DestroyBuilding.GetComponent<Button> ();



		//BuyButton.GetComponent<Button> ();
		//BuyButton.onClick.AddListener(() => BuyButtonActive());




	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This is ot figure out how much gold for the initial buy of a townStucuture
	public void initializeInitialGoldReqForTownStructures(){
		InitialGoldReqForTownStructures = new List<int>();
		System.Random rnd = new System.Random();

		for (int i = 0; i < 6; i++) {
			InitialGoldReqForTownStructures.Add (rnd.Next (80, 100));

		}

	}






	void initialiseHeroes(){
		
		initialiseHero (0);
		initialiseHero (0);
		initialiseHero (1);
		initialiseHero (1);
	
	}

	void initialisePlayer(){
		GameObject playerObject = new GameObject ();			// Create a new empty game object that will hold a hero.
		Player thePlayer = playerObject.AddComponent<Player> ();			// Add the hero.cs script to the object.
		// We can now refer to the object via this script.
		thePlayer.transform.parent = PlayerFolder.transform;
		//HeroFolder.Add (curHero);
		thePlayer.init(this );							// Initialize the hero script.
		thePlayer.name = "Player 1" ;						// Give the gem object a name in the Hierarchy pane.
		THEPLAYER = thePlayer;
		//print("initialised" + curHero.name);
	}




	void initialiseHero(int heroClass){
		
		GameObject heroObject = new GameObject ();			// Create a new empty game object that will hold a hero.
		Hero curHero = heroObject.AddComponent<Hero> ();			// Add the hero.cs script to the object.
		// We can now refer to the object via this script.
		curHero.transform.parent = HeroFolder.transform;
		//HeroFolder.Add (curHero);



		//heroRig;


		curHero.init(heroClass, this);							// Initialize the hero script.
		curHero.name = "Hero "+ HeroesSet.Count;						// Give the gem object a name in the Hierarchy pane.
		HeroesSet.Add(curHero);
		AvailableHeroesSet.Add (curHero);
		//print("initialised" + curHero.name);
	}

	void initialiseQuests(){
		int TimeNeeded = 80;
		int reqLevel = 1;
		int questCategory = 0;
		int reqXP = 1;
		List<int> reqClasses = null;
		List<Quest> previousAndNextList = null;
		int questType = 0;

		initialiseQuest(this, 0, 0 , 30, 1, 100, null, null);
		initialiseQuest(this, 0, 0 , 100, 2, 130, null, null);
		initialiseQuest(this, 0, 0 , 120, 3, 150, null, null);
		initialiseQuest(this, 0, 0 , 140, 4, 190, null, null);
		initialiseQuest(this, 0, 3 , 240, 3, 1, new List<int> {0, -1}, null);
		//initialiseQuest(this, 0, 0 , 240, 3, 1, null, null);
		//initialiseQuest(this, 0, 0 , 240, 4, 1, null, null);
		//initialiseQuest(this, 0, 0 , 240, 4, 1, null, null);
	}



	void initialiseQuest(GameManager man, int questType, int questCategory,int TimeNeeded, int reqLevel, int reqXP, List<int> reqClasses, List<Quest> previousAndNextList) {

			GameObject questObject = new GameObject ();			// Create a new empty game object that will hold a hero.
			Quest curQuest = questObject.AddComponent<Quest> ();			// Add the hero.cs script to the object.
			// We can now refer to the object via this script.
			curQuest.transform.parent = QuestFolder.transform;

			curQuest.init(this, questType, questCategory, TimeNeeded, reqLevel, reqXP, reqClasses, previousAndNextList);							// Initialize the hero script.

			curQuest.name = "Quest "+ QuestsSet.Count;						// Give the gem object a name in the Hierarchy pane.
			QuestsSet.Add(curQuest);
			
			//print("initialised" + curQuest.name);

		}


	//***********
	//***********
	// This function will check if the user has enough gold to buy the townstructure that was selected through GUI
	//***********
	//***********
	public void goldCheckAndBuy(int TownStructureType){
		// Since we havent initialised this townStrucuture - we need to be able to check for its required Gold
		// For this Im thinking of adding a list indexed by townStructureType and their initial required gold
		int goldRequired = InitialGoldReqForTownStructures[TownStructureType];
		if (THEPLAYER.Gold > goldRequired + 10) {
			buyTownStructure (TownStructureType);
			BuyBuildingsPanel.SetActive (false);
			//print("Successful");
			//BuildOnPlotText.gameObject.SetActive (true);
			initPanel.SetActive(true);
			buyTownStructure (TownStructureType);
		} else {
			// Not enough gold to but the required townStrucuture
			//***********
			// Open a dialog box and tell it to the user. 
			//***********
			print("Not enough gold");
		}
	}




	void buyTownStructure(int TownStructureType){
		GameObject townStructureObject = new GameObject ();
		TownStructure newTownStructure = townStructureObject.AddComponent<TownStructure> ();
		newTownStructure.transform.parent = TownStructureFolder.transform;
		Vector3 positionOnMap = new Vector3(0,0,0);
		newTownStructure.transform.position  = positionOnMap;

		BoxCollider2D townBox = townStructureObject.AddComponent<BoxCollider2D>();
		Rigidbody2D townRig = townStructureObject.AddComponent<Rigidbody2D> ();

		townBox.isTrigger = true;
		townRig.gravityScale = 0f;
		townRig.isKinematic = false;

		// ************
		// The first parameter for the init funciton was set to zero, but I changed it to TOwnStructureType as it should have been
		// ************
		newTownStructure.init (TownStructureType, this, THEPLAYER);

		newTownStructure.name = "TownStructure " + TownStructureSet.Count;
		TownStructureSet.Add (newTownStructure);

		//This part will add the location for it on map

		//Use the following function to get the mouse points;



	}

	// We get the TownStructure from the GUI and Colliders

	public void goldCheckAndUpdate(TownStructure toCheck){
		int goldReq = toCheck.goldReq;
		if (THEPLAYER.Gold > goldReq + 15) {
			updateTownStructure (toCheck);
			StartCoroutine (updateSuccc());
		} else {
			// Gold is not enough to update - print error
			// We would probably need to open up a dialog box here as well
			StartCoroutine (updateFailure());
			print("Not enough gold to upgrade");
		}
	}

	IEnumerator updateSuccc(){
		updateSucc = true;
		yield return new WaitForSeconds (3);
		updateSucc = false;
	}

	IEnumerator updateFailure(){
		updateFail =  true;
		yield return new WaitForSeconds(3);
		updateFail = false;
	}


	void updateTownStructure(TownStructure toUpdate){
		toUpdate.updatelevel ();
	}

	// THIS SECTION (STARTING HERE) WORKS WITH CHECKING AND ASSIGNING QUESTS

	//Will let GUI Function get the Quest that we have to work with
	public void questAssignCheck(Quest x){
		print ("here: " + x.xname);
		if (THEPLAYER.XP < x.reqXP) {
			// At this point, we cannot carry out this quest
			//print("this quest is not available due to lack of user XP);
			//Opena  GUI button and show this to the user.
		} else {
		// At this point we can check other things for the Quest:
			LinkedList<Hero> HeroesAvailableForThisQuest = new LinkedList<Hero>();
			// We find all the heroes available for teh Quet
			foreach (Hero nextAvailable in AvailableHeroesSet) {
				if (x.reqClasses == null) {
					HeroesAvailableForThisQuest.AddLast (nextAvailable);
				} else if(x.reqClasses.Count == 1 && x.reqClasses[0] == -1){
					HeroesAvailableForThisQuest.AddLast (nextAvailable);

				}else {


					foreach (int classNeeded in x.reqClasses) {
						if (classNeeded == nextAvailable.heroClass) {
							HeroesAvailableForThisQuest.AddLast (nextAvailable);
						}
					}
				}
			}

			// Create Buttons for all of these Heroes

			// Now we have the heroes available for this Quest: 
			// At this point, we give the user the ability to select heroes for this quest
			// Here a GUI Button will be needed
			// I have a function getUserSelectedHeroesForThisQuest(List<Hero> AvailableHeroesForThisQuest, int NumberOfExactHeroes)
			// this function returns a list of heroes the user has seleceted for this quest;



			//************* This is the function //*************
			NumberOfExactHeroes = x.reqClasses.Count;
			createButtons(HeroesAvailableForThisQuest);
			//UserSelectedHeroes = getUserSelectedHeroesForThisQuest(List<Hero> AvailableHeroesForThisQuest, int NumberOfExactHeroes);

			//*************
			assignQuest(x, UserSelectedHeroes);
		

		
		}
	}
	void createButtons(LinkedList<Hero> haftq ){
		this.haftq = haftq;
		selectionHeroes = true;


	}



	public void assignQuest(Quest x, List<Hero> UserSelectedHeroes){
		//Remove this quest from available quests
		AvailableQuestsSet.Remove (x);

		//Add this to ongoing quests
		QuestsInProgress.Add(x);

		//Remove the heroes selected from this quest from available heroes
		foreach (Hero thisHero in UserSelectedHeroes){
			AvailableHeroesSet.Remove(thisHero);
		}

		//Signal to Quest x to begin itself;
		x.beginQuest(UserSelectedHeroes);


	}


	void BuyButtonActive(){

		/*Set the functionality for all the buy buttons*/
		initPanel.SetActive (false);
		BuyPanel.SetActive (true);

	}
	void UpgradeButtonActive(){
		initPanel.SetActive(false);
		UpgradeButtonActiveText.gameObject.SetActive (true);
		//print ("Yeah");
		//UpgradeButtonActiveText.fontSize = 22;
	}
	void HeroesButtonActive(){
		initPanel.SetActive (false);
		HeroPanel.SetActive (true);
		//Scrollbarhorizontal.gameObject.SetActive (false);
	//	for(int i = 1; i <= AvailableHeroesSet.Count; i++)
		foreach (Hero curHero in AvailableHeroesSet)
		{
				
			//print ("Reached");
			
				GameObject goButton = (GameObject)Instantiate (prefabButton);
				goButton.transform.SetParent (ScrollbarHeroes, false);
				goButton.transform.localScale = new Vector3 (1, 1, 1);

				Button tempButton = goButton.GetComponent<Button> ();
			tempButton.GetComponentInChildren<Text> ().text = curHero.xname;
				//heroes.name+"   " + heroes.heroClass + "    Lv:" heroes.experienceLevel; 
			/**Note: We need a format script so that the name and level and class are all aligned in the list **/
			//	int tempInt = i;

				tempButton.onClick.AddListener (() => HeroButtonClicked (curHero));


			}
	}

	void HeroButtonClicked(Hero curHero){
		// We have a hero here. 
		//Display its Properties?
		HeroDisplay = true;
		HeroToDisplay = curHero;


	}

	void ButtonClicked(int buttonNo)
	{
			Debug.Log ("Button clicked = " + buttonNo);
	}
		
	
	void QuestsButtonActive(){
		initPanel.SetActive (false);
		QuestPanel.SetActive (true);

		foreach (Quest curQuest in QuestsSet)
			//foreach (Hero heroes in AvailableHeroesSet)
		{

			//print ("Reached");

			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.transform.SetParent (ScrollbarQuests, false);
			goButton.transform.localScale = new Vector3 (1, 1, 1);

			Button tempButton = goButton.GetComponent<Button> ();
			tempButton.GetComponentInChildren<Text> ().text = curQuest.xname;
			//heroes.name+"   " + heroes.heroClass + "    Lv:" heroes.experienceLevel; 
			/**Note: We need a format script so that the name and level and class are all aligned in the list **/
		

			tempButton.onClick.AddListener (() => QuestButtonClicked (curQuest));


		}

	}

	void QuestButtonClicked(Quest quest){
		
		print ("Quest Button Clicked" + quest.xname);
		questAssignCheck (quest);
	}


	void BuyBuildingsActive()
	{
		BuyPanel.SetActive (false);
		BuyBuildingsPanel.SetActive (true);
	}

	void BuyBuildings(int BuildingType)

	{
		//print ("Reached Buy Buildings Function");
		goldCheckAndBuy (BuildingType);

	}



	void OnGUI () {
		GUI.Button (new Rect (10,400,150,40), "Player Gold: " + THEPLAYER.Gold + " \n Player XP: "+THEPLAYER.XP ) ; 
		// Printing goes to the Console pane.  
		// If an object doesn't extend monobehavior, calling print won't do anything.  
		// Make sure "Collapse" isn't selected in the Console pane if you want to see duplicate messages.
		if (HeroDisplay == true) {
			if (GUI.Button (new Rect (10, 100, 250, 80), 
				"Hero Name: "  + HeroToDisplay.xname + "\n" +
				"Hero Type: "  + HeroToDisplay.heroClass + "\n" +
				"Hero Fetch Time: "  + HeroToDisplay.fetchTime + "\n" +
				"Hero Grind Time: " + HeroToDisplay.grindTime + ""
			)) {
				HeroDisplay = false;
				initPanel.SetActive (true);
			}
		}

		if (QuestsInProgress.Count > 0) {
			GUI.Button (new Rect (10,80,150,20), "Active Quests: " ) ; 
			int y = 105;
			foreach (Quest p in QuestsInProgress) {
				string z = p.xname + "\n";
				int i = 1;	
				foreach (Hero h in p.HeroesAssignedToThisQuest){
					z= z+ "Hero " + i + ": " + h.xname + "\n";
					i++;
					}
				GUI.Button (new Rect (10,y,150,20*i), z ) ; 

				y += 20*i + 5;
			}



		}


		if (selectionHeroes == true && NumberOfExactHeroes != 0) {
			int y = 100;
			foreach (Hero HeroToDisplay in haftq) {
				if (GUI.Button (new Rect (10, y, 250, 80), 
					"Hero Name: "  + HeroToDisplay.xname + "\n" +
					"Hero Type: "  + HeroToDisplay.heroClass + "\n" +
					"Hero Fetch Time: "  + HeroToDisplay.fetchTime + "\n" +
					"Hero Grind Time: " + HeroToDisplay.grindTime + ""
				)) {
					//HeroDisplay = false;
					NumberOfExactHeroes--;
					UserSelectedHeroes.Add (HeroToDisplay);
					if (NumberOfExactHeroes == 0) {
						selectionHeroes = false;
						QuestPanel.SetActive (false);
						initPanel.SetActive (true);

					}
				}
			
				y += 85;
			}
				
		}

		if (updateSucc) {
			GUI.Box (new Rect ( 20, 500, 100, 20), "Update Successful");
		}


		if (updateFail) {
			GUI.Box (new Rect ( 20, 500, 200, 20), "Not enough gold to update");
		}
			

	
	
	}






}




