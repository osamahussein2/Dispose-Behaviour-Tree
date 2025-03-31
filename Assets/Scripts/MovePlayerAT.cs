using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class MovePlayerAT : ActionTask {

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
			// If a garbage is found in the scene, destroy it
			if (GameObject.FindWithTag("Garbage"))
			{
				Object.Destroy(GameObject.FindWithTag("Garbage"));
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() 
		{
			// Get the horizontal and vertical values
			float horizontal = Input.GetAxisRaw("Horizontal");
			float vertical = Input.GetAxisRaw("Vertical");

            // Make the camera follow the player around by adding camera offset
            Camera.main.transform.position = agent.transform.position + garbageCollectorData.value.cameraOffset;

			// Set the movement using the horizontal and vertical (in z axis for moving forward/backward) values
            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);

			// Move the player by their movement vector and their player move speed (using delta time)
			agent.transform.position += movement * garbageCollectorData.value.playerMoveSpeed * Time.deltaTime;

			// If the player isn't holding any of the movement keys, end this action
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) &&
                !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.LeftArrow) && 
				!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && 
				!Input.GetKey(KeyCode.DownArrow))
			{
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