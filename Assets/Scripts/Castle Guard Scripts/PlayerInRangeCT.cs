using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.ObjectModel;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class PlayerInRangeCT : ConditionTask
	{
		private Blackboard castleGuardBlackboard;
		private bool playerFound;

        public BBParameter<CastleGuardData> castleGuardData;

        private float timer;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
		{
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable()
		{
            timer = 0.0f;

            // Get the blackboard component
            castleGuardBlackboard = agent.GetComponent<Blackboard>();

            timer += Time.deltaTime;

            // Hide the exclamation and question marks after 1 second
            if (timer >= 1.0f)
            {
                castleGuardData.value.exclamationMark.SetActive(false);
                castleGuardData.value.questionMark.SetActive(false);
            }
        }

		//Called whenever the condition gets disabled.
		protected override void OnDisable()
		{
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
            // Check if the player and castle guard's distance is within the castle guard's radius
            /* Detect the sphere overlap between the castle guard and the garbage collector's layer mask to determine
			if it's within the castle guard's radius */
            Collider[] castleGuardColliders = Physics.OverlapSphere(agent.transform.position, castleGuardData.value.castleGuardRadius,
                 castleGuardData.value.collector);

            // If there are no target colldiers in the array
            if (castleGuardColliders.Length == 0)
            {
                // Make sure player found bool is set to false
                playerFound = castleGuardBlackboard.GetVariableValue<bool>("PlayerFound");
                playerFound = false;

                castleGuardBlackboard.SetVariableValue("PlayerFound", playerFound);
            }

            // Once it gets the target colliders
            foreach (Collider collider in castleGuardColliders)
            {
                // Make sure player found bool is set to true
                playerFound = castleGuardBlackboard.GetVariableValue<bool>("PlayerFound");
                playerFound = true;

                castleGuardBlackboard.SetVariableValue("PlayerFound", playerFound);

                // And hide the question mark afterwards
                castleGuardData.value.questionMark.SetActive(false);
            }

            // Check if player found is already set to true upon initialization
            return playerFound;
		}
	}
}