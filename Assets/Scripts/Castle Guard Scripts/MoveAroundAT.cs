using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class MoveAroundAT : ActionTask {

		private Vector3 randomPoint;
		private bool randomPointDetected;
		private Blackboard castleGuardBlackboard;
		private bool playerFound;

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
			// Initialize all blackboard/private variables
			randomPointDetected = false;

			castleGuardBlackboard = agent.GetComponent<Blackboard>();

			playerFound = castleGuardBlackboard.GetVariableValue<bool>("PlayerFound");
			playerFound = false;

			castleGuardBlackboard.SetVariableValue("PlayerFound", playerFound);

            castleGuardData.value.questionMark.SetActive(false);
            castleGuardData.value.exclamationMark.SetActive(false);
            castleGuardData.value.hitAlert.SetActive(false);
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			/* If the castle guard can't find a random point to move towards, set its random point and the 
			bool to true to start moving there */
			if (!randomPointDetected)
			{
				// Get and set the random point using its blackboard component
				randomPoint = castleGuardBlackboard.GetVariableValue<Vector3>("RandomPoint");
				randomPoint = new Vector3(Random.Range(3, 6), 0.0f, Random.Range(3, 6));

				castleGuardBlackboard.SetVariableValue("RandomPoint", randomPoint);

				randomPointDetected = true;
			}

			else
			{
				// Move towards the random point
				agent.transform.position += (randomPoint - agent.transform.position) * castleGuardData.value.castleGuardPatrolSpeed *
					Time.deltaTime;

				/* Once the castle guard is close enough to the random point, set the bool to false to find another
				random point to move to */
				if (Vector3.Distance(agent.transform.position, randomPoint) <= 0.1f)
				{
					randomPointDetected = false;
				}
			}

			// If the garbage collector and the castle guard distance is less than the castle guard's warning value
			if (Vector3.Distance(agent.transform.position, castleGuardData.value.garbageCollector.transform.position) <=
                 castleGuardData.value.castleGuardWarning)
			{
                // Activate the question mark above their head
                castleGuardData.value.questionMark.SetActive(true);
			}

			else
			{
                // Else hide the question mark
                castleGuardData.value.questionMark.SetActive(false);
			}

			/* Detect the sphere overlap between the castle guard and the garbage collector's layer mask to determine
			if it's within the castle guard's radius */
			Collider[] castleGuardColliders = Physics.OverlapSphere(agent.transform.position, castleGuardData.value.castleGuardRadius,
                 castleGuardData.value.collector);

			// Once it gets the collider, hide the question mark and end this action
			foreach (Collider collider in castleGuardColliders)
			{
                castleGuardData.value.questionMark.SetActive(false);
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