using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class ThrowGarbageAT : ActionTask {

        public BBParameter<GarbageCollectorData> garbageCollectorData;

        private GameObject spawnedGarbage;

        private GameObject topOfBin;

        private bool isGarbageFallingIntoBin;

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

            // Get the child of the garbage bin object to set the top bin
            topOfBin = garbageCollectorData.value.garbageBin.transform.GetChild(0).gameObject;

            // Set the top bin to be positioned just above the top of garbage bin
            topOfBin.transform.localPosition = new Vector3(0.0f, 0.65f, 0.0f);

            // Set the bool to false first to ensure the top bin doesn't lower
            isGarbageFallingIntoBin = false;

            // However, end this action if the garbage object isn't found in the scene
            if (!spawnedGarbage)
            {
                EndAction(true);
            }

            garbageCollectorData.value.playerStateText.text = "Player State: Throwing out garbage";
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() 
		{
            // Make the camera follow the garbage collector around by adding camera offset
            Camera.main.transform.position = agent.transform.position + garbageCollectorData.value.cameraOffset;

            // Get the distance between the garbage collector and the garbage bin
            float distanceFromBin = Vector3.Distance(agent.transform.position, 
                garbageCollectorData.value.garbageBin.transform.position);

            /* If the distance from garbage bin is less than the garbage bin distance, make the garbage collector throw
			out the garbage */
            if (distanceFromBin <= garbageCollectorData.value.garbageBinDistance)
            {
                // Stop moving the garbage collector
                agent.transform.position += new Vector3(0.0f, 0.0f, 0.0f);

                // Decrease the spawned garbage scale to make sure it's not longer than the garbage bin itself
                spawnedGarbage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                // Move the spawned garbage towards the top of garbage bin
                spawnedGarbage.transform.position += (topOfBin.transform.position - spawnedGarbage.transform.position) 
                    * Time.deltaTime;

                // If the spawned garbage and the top of garbage bin are close to each other
                if (Vector3.Distance(spawnedGarbage.transform.position, topOfBin.transform.position) <= 0.1f)
                {
                    // Set bool to true to make the garbage fall into the bin
                    isGarbageFallingIntoBin = true;

                    // Once the top bin goes less than 0 in y position, destroy the spawned garbage
                    if (topOfBin.transform.position.y <= 0f)
                    {
                        // End this action once the garbage has been destroyed
                        Object.Destroy(spawnedGarbage);
                        EndAction(true);
                    }
                }
            }

            else
            {
                // Move towards the garbage bin after picking up the garbage from the last action
                agent.transform.position += (garbageCollectorData.value.garbageBin.transform.position - 
                    agent.transform.position) * garbageCollectorData.value.garbageCollectorSpeed * Time.deltaTime;
            }

            // Make sure the top bin doesn't move when either the bool is false or is less than 0 in y position
            if (!isGarbageFallingIntoBin || topOfBin.transform.position.y <= 0f)
            {
                topOfBin.transform.position += Vector3.zero;
            }

            // Otherwise, lower top bin to act as if the garbage is thrown into the bin
            else
            {
                topOfBin.transform.position += (garbageCollectorData.value.garbageBin.transform.position - 
                    topOfBin.transform.position) * garbageCollectorData.value.garbageFallSpeed * Time.deltaTime;
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