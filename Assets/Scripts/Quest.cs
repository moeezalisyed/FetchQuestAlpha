using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest : MonoBehaviour {

    //Constant (immutable) variables representing time availability of this quest.



	// These are the possible QuestTypes:
    public const int LIMITEDAVAIL = 1;

    public const int ANYTIME = 0;
	//Possibilities for questtypes end here

    //Same as above but for questCategory

	//These are questcategories
    public const int FETCH = 0;

    public const int GATHER = 1;

    public const int DELIVER = 2;

    public const int GRIND = 3;

    public const int SPECIAL = 4;

	public string xname = "";
	//Quest categories end here.


    //Previous and next quest in chain, used for linked list style implementation of quest chains

	public Quest previous;

	public Quest next;

    //Boolean value for whether or not this quest is the final in a chain

    public bool isFinalInChain;

    //Ints for quest time,

	public int goldReward;
	public int XPReward;
	public int HeroXPReward;



    public int QuestType,QuestCategory, TimeNeeded, reqLevel, reqXP;

    //List of integer represeentations of the required classes

	public List<int> reqClasses;
	public List<Hero> HeroesAssignedToThisQuest;

    private GameManager manager;

	/*
    Init for Quest object, takes in the numerical values listed in the design doc aas parameters
    to instantiate the above fields. Additionally, we pass in a list of two quests, representing previous and next in the chain.
    */
	public void init(GameManager man, int questType, int questCategory,int TimeNeeded, int reqLevel, int reqXP, List<int> reqClasses, List<Quest> previousAndNextList) {
        this.TimeNeeded = TimeNeeded;
        this.reqLevel = reqLevel;
        this.manager = man;
        this.QuestCategory = questCategory;
        this.reqXP = reqXP;
        this.reqClasses = reqClasses;
        this.QuestType = questType;
        //If the quest is limited availability, we want to set its previous quest as specified by the parameter

		if (questType == 0) {
			this.xname = "FETCH: " + TimeNeeded + " secs";
		}

		if (questType == LIMITEDAVAIL)
        {
            this.previous = previousAndNextList[0];

            //Now, we can infer the isFinalInChain value by checking if the second value of
            //previousAndNextList is null. If it is, the value is true, false otherwise.

            if (previousAndNextList[1] == null)
            {
                this.isFinalInChain = true;
            }
            else
            {
                this.isFinalInChain = false;
            }
        }
    }


	public void beginQuest(List<Hero> UserGivenHeroesForThisQuest){
		this.HeroesAssignedToThisQuest = UserGivenHeroesForThisQuest;
		foreach (Hero thisHero in this.HeroesAssignedToThisQuest) {
			this.TimeNeeded += thisHero.fetchTime;
			this.TimeNeeded += thisHero.grindTime;
		}
		//print ("quest Startedx:" +this.xname);
		StartCoroutine ("questRoutine");


	}

	//Want to call endQuest TimeNeeded seconds after beginQuest is called 

	IEnumerator questRoutine(){
		//print ("quest Started:" +this.xname);
		yield return new WaitForSeconds (this.TimeNeeded);
		this.endQuest();
	}


	public void endQuest(){
		print ("quest Ended");
		manager.THEPLAYER.Gold += this.goldReward;
		manager.THEPLAYER.XP += this.XPReward;
		foreach (Hero thisHero in this.HeroesAssignedToThisQuest) {
			thisHero.addWin (this);
			manager.AvailableHeroesSet.Add (thisHero);

		}
		manager.QuestsInProgress.Remove (this);
		if (this.QuestType == ANYTIME) {
			manager.AvailableQuestsSet.Add (this);
		}

	
	}


}
