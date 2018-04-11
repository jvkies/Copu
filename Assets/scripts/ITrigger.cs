using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger  {

	void Trigger(List<string> entities, float mass);
	void StopTrigger(List<string> entities, float mass);
}
