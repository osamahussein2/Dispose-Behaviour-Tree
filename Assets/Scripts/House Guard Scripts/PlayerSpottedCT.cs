using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions {

	public class PlayerSpottedCT : ConditionTask {

        private Blackboard houseGuardBlackboard;
        private bool playerFound;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() 
		{
			// Initialize the blackboard component to get the player found variable from the blackboard
            houseGuardBlackboard = agent.GetComponent<Blackboard>();

            playerFound = houseGuardBlackboard.GetVariableValue<bool>("PlayerFound");
            playerFound = true; // Set the blackboard variable to true so that it can transition to the next task

            houseGuardBlackboard.SetVariableValue("PlayerFound", playerFound);
        }

		//Called whenever the condition gets disabled.
		protected override void OnDisable()
		{
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			return playerFound;
		}
	}
}