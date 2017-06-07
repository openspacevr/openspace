using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentalHealth : ScriptableObject {

	public string[] problems = new string[] 
	{
		"Cope with ADHD",
		"Overcome addiction",
		"Relieve anxiety",
		"Cope with bipolar disorder",
		"Cope with a breakup",
		"Deal with bullying",
		"Manage chronic pain",
		"Cope with college life",
		"Reduce depression",
		"Cope with disability",
		"Escape domestic violence",
		"Overcome eating disorders",
		"Get more exercise",
		"Cope with family stress",
		"Cope with financial stress",
		"Work on forgiving",
		"Get unstuck",
		"Reduce grief",
		"LGBTQ+ issues",
		"Defeat loneliness",
		"Manage my emotions",
		"Cope with OCD",
		"Overcome trauma",
		"Manage panic attacks",
		"Better parenting",
		"Relieve perinatal mood disorder",
		"Healthy relationships",
		"Overcome self harm",
		"Get better sleep",
		"Manage social anxiety",
		"Manage my weight",
		"Cope with work stress",
	};

	public string[] Problems
	{
		get
		{ 
			return problems;
		}
		set
		{
			problems = value;
		}
	}
}
