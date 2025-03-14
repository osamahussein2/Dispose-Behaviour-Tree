using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions {

	public class FoundGarbageCT : ConditionTask {

		// I need this to be static to reset the timer in another script
		public static float timer;

		private float maxSpawnTime;

		// Use the blackboard parameters I created
		public BBParameter<List<float>> garbageSpawnTime;
		public BBParameter<GameObject> garbage;
		public BBParameter<List<Vector3>> garbageSpawnPoint;

		private Vector3 randomSpawnPoint;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			// Set the timer to 0 first
			timer = 0.0f;

			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() 
		{
			// Increment the timer to spawn the garbage
            timer += Time.deltaTime;

            // Randomize the max spawn time
            maxSpawnTime = Random.Range(garbageSpawnTime.value[0], garbageSpawnTime.value[1]);

			// Randomize the garbage spawn point
			randomSpawnPoint = new Vector3(Random.Range(garbageSpawnPoint.value[0].x, garbageSpawnPoint.value[1].x),
				Random.Range(garbageSpawnPoint.value[0].y, garbageSpawnPoint.value[1].y), 
				Random.Range(garbageSpawnPoint.value[0].z, garbageSpawnPoint.value[1].z));
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable() {
			
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck() 
		{
			// Instantiate the garbage in the scene after the timer exceeds the randomized spawn time
			if (timer >= maxSpawnTime)
			{
				Object.Instantiate(garbage.value, randomSpawnPoint, Quaternion.identity);
			}

			// End the condition task once a garbage object is found in the scene using its tag
			return GameObject.FindWithTag("Garbage");
		}
	}
}