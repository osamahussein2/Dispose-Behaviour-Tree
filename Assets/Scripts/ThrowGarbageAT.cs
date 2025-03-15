using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class ThrowGarbageAT : ActionTask {

        public BBParameter<GameObject> garbageBin;

		public BBParameter<float> moveToGarbageBinSpeed;
        public BBParameter<float> garbageBinDistance;

		private GameObject spawnedGarbage;

        public BBParameter<Vector3> cameraOffset;

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
            // Make the camera follow the garbage collector around by adding camera offset
            Camera.main.transform.position = agent.transform.position + cameraOffset.value;

            // Get the distance between the garbage collector and the garbage bin
            float distanceFromBin = Vector3.Distance(agent.transform.position, garbageBin.value.transform.position);

            /* If the distance from garbage bin is less than the garbage bin distance, make the garbage collector throw
			out the garbage */
            if (distanceFromBin <= garbageBinDistance.value)
            {
                // Stop moving the garbage collector
                agent.transform.position += new Vector3(0.0f, 0.0f, 0.0f);

                // Move the spawned garbage towards the garbage bin to throw out
                spawnedGarbage.transform.position += (garbageBin.value.transform.position - 
                    spawnedGarbage.transform.position) * Time.deltaTime;

                // Decrease the spawned garbage scale to make sure it's not longer than the garbage bin itself
                spawnedGarbage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                // If the spawned garbage and garbage bin are close to each other
                if (Vector3.Distance(spawnedGarbage.transform.position, garbageBin.value.transform.position) <= 0.2f)
                {
                    // Destroy the spawned garbage and end this action
                    Object.Destroy(spawnedGarbage);
                    EndAction(true);
                }
            }

            else
            {
                // Move towards the garbage bin after picking up the garbage from the last action
                agent.transform.position += (garbageBin.value.transform.position - agent.transform.position)
                    * moveToGarbageBinSpeed.value * Time.deltaTime;
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