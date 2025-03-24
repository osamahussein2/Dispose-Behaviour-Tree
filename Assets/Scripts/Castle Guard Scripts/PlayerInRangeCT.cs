using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.ObjectModel;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class PlayerInRangeCT : ConditionTask
	{
		private Blackboard castleGuardBlackboard;
		public BBParameter<float> castleGuardRadius;
		public BBParameter<LayerMask> collector;
		private bool playerFound;
		public BBParameter<GameObject> exclamationMark;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable()
		{
			// Get the blackboard component
			castleGuardBlackboard = agent.GetComponent<Blackboard>();

			// Make sure player found bool is set to true to check it
			playerFound = castleGuardBlackboard.GetVariableValue<bool>("PlayerFound");
			playerFound = true;

			castleGuardBlackboard.SetVariableValue("PlayerFound", playerFound);

			exclamationMark.value.SetActive(true);
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable()
		{
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			// Check if player found is already set to true upon initialization
			return playerFound;
		}
	}
}