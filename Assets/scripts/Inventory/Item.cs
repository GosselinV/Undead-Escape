using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

	protected string toolTipString;
	protected int encumbrance;
	public int Encumbrance{
		get { return encumbrance; }
	}

	protected ItemType itemType;
	public ItemType Type{
		get { return itemType; }
	}

	protected ItemName itemName;
	public ItemName Name{
		get { return itemName; }
	}
		
	public string ToolTipString{
		get { return toolTipString; }
	}

}
