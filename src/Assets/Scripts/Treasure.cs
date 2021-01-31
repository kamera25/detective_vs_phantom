using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTreasureType
{
	Ring,
	Bracelet,
	Crown,
};
public class TreasureData
{
	public eTreasureType type;
	public int score;
	public TreasureData(eTreasureType type)
	{
		this.type = type;
		switch(type) {
		case eTreasureType.Ring:
			this.score = 1;
			break;
		case eTreasureType.Bracelet:
			this.score = 2;
			break;
		case eTreasureType.Crown:
			this.score = 3;
			break;
		}
	}
}

public class Treasure : MonoBehaviour
{
	public TreasureData data { get; set; }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.Rotate(0.0f, 0.5f, 0.0f);
    }
}
