using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Item {

	protected int additionalActions = 0;
	public int AdditionalActions{
		get { return additionalActions; }
	}

	protected int numUsableCarry = 0;
	public int NumUsableCarry{
		get { return numUsableCarry; }
	}

	protected int additionalEncumbrance = 0;
	public int AdditionalEncumbrance{
		get { return additionalEncumbrance; }
	}

}
