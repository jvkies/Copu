using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor {

	// float interactCounter; // no field ins Interfaces hmm..

	//void Interact(bool isActivating) ;
	void Trigger(Dictionary<string, string> actions, List<string> entities, GameObject source);
	//void StopTrigger(Dictionary<string, string> actions, List<string> entities);

}
