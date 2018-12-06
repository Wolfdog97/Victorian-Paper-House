using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaCupPuzzleChecker : PuzzleConditional
{
	public TeaCup cup;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		conditionMet = cup.sugarAmount == 2 && cup.hasTea;
	}
}
