using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class MoveToPlayerAT : ActionTask {
		
		public BBParameter<GameObject> garbageCollector;
		public BBParameter<float> castleGuardChaseSpeed;
		public BBParameter<float> castleGuardRadius;
		private float timer;
		public BBParameter<GameObject> exclamationMark;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			// Set the timer to 0 every time this action executes
			timer = 0.0f;

			// Make sure the exclamation mark is turned on
			exclamationMark.value.SetActive(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			// Increment the timer
			timer += Time.deltaTime;

			if (timer >= 1.0f)
			{
				// Turn off excalamation mark after 1 second has passed
				exclamationMark.value.SetActive(false);
			}

			// Move towards the garbage collector
			agent.transform.position += (garbageCollector.value.transform.position - agent.transform.position) *
				castleGuardChaseSpeed.value * Time.deltaTime;

			// If the garbage collector is far enough from the castle guard, end this action
			if (Vector3.Distance(agent.transform.position, garbageCollector.value.transform.position) >
				castleGuardRadius.value)
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