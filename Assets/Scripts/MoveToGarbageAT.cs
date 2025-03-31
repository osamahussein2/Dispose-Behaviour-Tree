using NodeCanvas.Framework;
using NodeCanvas.Tasks.Conditions;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class MoveToGarbageAT : ActionTask {

		private GameObject spawnedGarbage;

        public BBParameter<GarbageCollectorData> garbageCollectorData;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() 
		{
			// Reset the timer from found garbage class back to 0
			FoundGarbageCT.timer = 0.0f;

			// Set the private gameobject to find the garbage somewhere in the scene
			spawnedGarbage = GameObject.FindWithTag("Garbage");

			// However, end this action if the garbage object isn't found in the scene
			if (!spawnedGarbage)
			{
				EndAction(true);
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() 
		{
			// Move towards the spawned garbage object
			agent.transform.position += (spawnedGarbage.transform.position - agent.transform.position) 
				* garbageCollectorData.value.garbageCollectorSpeed * Time.deltaTime;

			// Make the camera follow the garbage collector around by adding camera offset
            Camera.main.transform.position = agent.transform.position + garbageCollectorData.value.cameraOffset;

            // Get the distance between the garbage collector and the garbage itself
            float distanceFromGarbage = Vector3.Distance(agent.transform.position, spawnedGarbage.transform.position);

			/* If the distance from garbage is less than the garbage pickup distance, make the garbage collector pick
			up the garbage */
            if (distanceFromGarbage <= garbageCollectorData.value.garbagePickupDistance)
			{
				// Attach the spawned garbage to the garbage collector
				spawnedGarbage.transform.SetParent(agent.transform);

				// End this action
				EndAction(true);
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}