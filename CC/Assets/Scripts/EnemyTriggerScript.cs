﻿using UnityEngine;
using System.Collections;

public class EnemyTriggerScript : MonoBehaviour {

	public GameObject enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Transform GetTriggerTransform()
	{
		return enemy.transform;
	}
}
