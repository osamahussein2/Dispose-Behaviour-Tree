using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions {

	public class InteractionLevelLowCT : ConditionTask 
	{
		public BBParameter<HouseGuardData> houseGuardData;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
            houseGuardData.value.houseGuardInteractionSlider.value =
                houseGuardData.value.houseGuardInteractionSlider.maxValue;

            houseGuardData.value.castleGuardInteractionSlider.value =
                houseGuardData.value.castleGuardInteractionSlider.maxValue;

            return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() 
		{
			
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable() {
			
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck() 
		{


			return houseGuardData.value.houseGuardInteractionSlider.value <= 
				houseGuardData.value.lowInteractionLevelValue || 
				houseGuardData.value.castleGuardInteractionSlider.value <= 
				houseGuardData.value.lowInteractionLevelValue;
		}
	}
}