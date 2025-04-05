using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEditor.Rendering;
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
			houseGuardData.value.houseGuardStateText.text = "House Guard State: Moving to castle guard";

            houseGuardData.value.shootingAlert.SetActive(false);
            houseGuardData.value.lookingAlert.SetActive(false);
            houseGuardData.value.movingToCastleGuardAlert.SetActive(true);
            houseGuardData.value.interactingWithCastleGuardAlert.SetActive(false);

            agent.transform.rotation = Quaternion.identity;
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			// If the distance between the house guard and castle guard are close enough
			if (Vector3.Distance(agent.transform.position, houseGuardData.value.castleGuard.transform.position) <=
				houseGuardData.value.distanceToCastleGuard)
			{
				houseGuardData.value.houseGuardStateText.text = "House Guard State: Interacting with castle guard";

				// Don't move the house guard
				agent.transform.position += Vector3.zero;

				// Increment both the house guard and castle guard interaction level sliders
				houseGuardData.value.houseGuardInteractionSlider.value +=
				houseGuardData.value.houseGuardInteractionIncreaseRate * Time.deltaTime;

				houseGuardData.value.castleGuardInteractionSlider.value +=
					houseGuardData.value.castleGuardInteractionIncreaseRate * Time.deltaTime;

                // Hide the moving alert to castle guard and show the interacting with castle guard alert
                houseGuardData.value.movingToCastleGuardAlert.SetActive(false);
				houseGuardData.value.interactingWithCastleGuardAlert.SetActive(true);
			}

			// Otherwise, move towards the castle guard
			else
			{
				agent.transform.position += (houseGuardData.value.castleGuard.transform.position -
					agent.transform.position).normalized * houseGuardData.value.moveToCastleGuardSpeed * Time.deltaTime;
			}

			// Hide the castle guard interaction signifiers after both sliders reaching a certain value
			if (houseGuardData.value.houseGuardInteractionSlider.value >= houseGuardData.value.houseGuardSliderValue &&
				houseGuardData.value.castleGuardInteractionSlider.value >= houseGuardData.value.castleGuardSliderValue)
			{
                houseGuardData.value.movingToCastleGuardAlert.SetActive(false);
                houseGuardData.value.interactingWithCastleGuardAlert.SetActive(false);
            }

			// If the house and castle guards interaction slider values have maxed out, end this action
			if (houseGuardData.value.houseGuardInteractionSlider.value >=
				houseGuardData.value.houseGuardInteractionSlider.maxValue &&
				houseGuardData.value.castleGuardInteractionSlider.value >=
				houseGuardData.value.castleGuardInteractionSlider.maxValue)
			{
				// And hide the signifiers related to castle guard alerts
                houseGuardData.value.movingToCastleGuardAlert.SetActive(false);
				houseGuardData.value.interactingWithCastleGuardAlert.SetActive(false);
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