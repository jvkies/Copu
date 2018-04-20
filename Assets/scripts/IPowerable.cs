using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerable {

	//void ConductElectricity (bool isAddingPower, string color, Dictionary<string, string> _actions, GameObject source);
	void AddPower (GameObject source, string color, List<string> _powerOrigins);
	void AddPower (GameObject source, string color, string powerOrigin);
	void RemovePower (GameObject source, List<string> _powerOrigins);
	void RemovePower (GameObject source, string powerOrigin);

}
