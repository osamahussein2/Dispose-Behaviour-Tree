using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class MoveToCastleGuardAT : ActionTask {

		public BBParameter<HouseGuardData> houseGuardData;

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
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			// If the distance between the house guard and castle guard are close enough
			if (Vector3.Distance(agent.transform.position, houseGuardData.value.castleGuard.transform.position) <=
				houseGuardData.value.distanceToCastleGuard)
			{
				// Don't move the house guard
				agent.transform.position += Vector3.zero;

				// Increment both the house guard and castle guard interaction level sliders
				houseGuardData.value.houseGuardInteractionSlider.value +=
				houseGuardData.value.houseGuardInteractionIncreaseRate * Time.deltaTime;

				houseGuardData.value.castleGuardInteractionSlider.value +=
					houseGuardData.value.castleGuardInteractionIncreaseRate * Time.deltaTime;
			}

			// Otherwise, move towards the castle guard
			else
			{
				agent.transform.position += (houseGuardData.value.castleGuard.transform.position -
					agent.transform.position).normalized * houseGuardData.value.moveToCastleGuardSpeed * Time.deltaTime;
			}

			// If the house and castle guards interaction slider values have maxed out, end this action
			if (houseGuardData.value.houseGuardInteractionSlider.value >=
				houseGuardData.value.houseGuardInteractionSlider.maxValue &&
				houseGuardData.value.castleGuardInteractionSlider.value >=
				houseGuardData.value.castleGuardInteractionSlider.maxValue)
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