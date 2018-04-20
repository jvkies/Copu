using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressureable  {

	void AddPressure(float amount);
	void RemovePressure(float amount);
}
