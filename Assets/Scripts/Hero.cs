using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

    private const int ROUGE = 0;
    private const int WARRIOR = 1;
    private const int CASTER = 2;
    private const int RANGER = 3;
    private const int HEALER = 4;
	private GameManager parentGameManager;
	

	//Feilds for class specific strengths and weaknesses:


    //The three hero fields we discussed for this class - the variable names should be relatively self explanitory
	public int numWins;
	public int experienceLevel;
    //This is from the above category
	public int heroClass;

    //Optional list of quests for a hero
    private List<Quest> quests;
	//List of quests completed by the hero:
	private List<Quest> questsCompletedByHero;

	//Fetch and grind times for heroes

	public int fetchTime;
	public int grindTime;
	public string xname = "";

		public int i = 1;
	public int j = 1;

    // Here we initialize the above variables and require an integer representation of "class" to be
    //passed as a parameter





	public void init (int Class, GameManager m) {

        this.numWins = 0;
		this.parentGameManager = m;
        this.numWins = 0;
	//	this.parentGameManager = m;
        this.experienceLevel = 1;
        this.heroClass = Class;
        //Why do we initialize a list
		this.quests = new List<Quest>();
		this.questsCompletedByHero = new List<Quest> ();
		this.fetchTime = 20;
		this.grindTime = 20;
		this.setBonuses ();
		this.setXname();



	}

		void setXname(){
		if (this.heroClass == 0) {
			this.xname = "ROGUE"  + parentGameManager.HeroesSet.Count + ": "+ this.fetchTime;
			i++;
		} else {
			this.xname = "WARRIOR" + parentGameManager.HeroesSet.Count + ": "+ this.fetchTime;
			j++;
		}
	}

	void setBonuses(){
		if (this.heroClass == ROUGE && this.experienceLevel <= 4) {
			this.fetchTime = -10;
			this.grindTime = +10;
		} else if (this.heroClass == WARRIOR && this.experienceLevel <= 4) {
			this.grindTime = -10;
			this.fetchTime = +10;
		}
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


    //General getters/setters for the parameters
	public int addWin(Quest q){
		this.questsCompletedByHero.Add (q);
		//If the hero is below level four or has 14 wins they will level up.
		if (this.experienceLevel < 4 ||this.numWins == 14) {
			this.levelUP ();
		}
		return this.questsCompletedByHero.Count;
	}

    public int getXP()
    {
		return this.numWins;
    }



    public void addQuest(Quest q)
    {
        this.quests.Add(q);
    }

    public int getType()
    {
        return this.heroClass;
    }

	//Level Up method for hero, more will be added
	void levelUP(){
		this.experienceLevel += 1;
		if (this.experienceLevel == 4) {

			this.promoteHero();

		}
	}

	void promoteHero(){
		switch (this.heroClass) 
		{
		case ROUGE:
			this.fetchTime -= 10;
			break;
		case WARRIOR:
			this.grindTime -= 10;
			break;
		}
	}


}
