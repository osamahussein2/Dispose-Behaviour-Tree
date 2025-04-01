using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class MoveToPlayerAT : ActionTask {
		
		private float timer;

        public BBParameter<CastleGuardData> castleGuardData;

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
            castleGuardData.value.exclamationMark.SetActive(true);

			// Just in case if question mark is still active, hide it
			if (castleGuardData.value.questionMark.activeInHierarchy == true)
			{
				castleGuardData.value.questionMark.SetActive(false);
			}

            castleGuardData.value.castleGuardStateText.text = "Castle Guard State: Moving towards player";
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			// Increment the timer
			timer += Time.deltaTime;

			if (timer >= 1.0f)
			{
                // Turn off excalamation mark after 1 second has passed
                castleGuardData.value.exclamationMark.SetActive(false);
			}

			// Make sure to end this action after the castle guard is near the player for melee combat
			if (Vector3.Distance(agent.transform.position, castleGuardData.value.garbageCollector.transform.position) <=
				castleGuardData.value.meleeAttackRange)
			{
                EndAction(true);
			}

			// Otherwise, keep moving towards the player
			else
			{
                agent.transform.position += (castleGuardData.value.garbageCollector.transform.position - 
					agent.transform.position) *  castleGuardData.value.castleGuardChaseSpeed * Time.deltaTime;
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