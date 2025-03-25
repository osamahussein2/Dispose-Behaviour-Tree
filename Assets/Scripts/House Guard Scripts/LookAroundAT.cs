using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class LookAroundAT : ActionTask {

		public BBParameter<float> rotationSpeed;

		public BBParameter<float> lookAroundTime;
		private float timer;
		public BBParameter<float> rotationThreshold;
		private bool playerFound;
		private Blackboard houseGuardBlackboard;
		public BBParameter<float> houseGuardRadius;
		public BBParameter<LayerMask> collectorMask;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			timer = 0.0f;

			houseGuardBlackboard = agent.GetComponent<Blackboard>();

			playerFound = houseGuardBlackboard.GetVariableValue<bool>("PlayerFound");
			playerFound = false;
			
			houseGuardBlackboard.SetVariableValue("PlayerFound", playerFound);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			LookAroundLogic();
			ChangeRotationDirection();
			SeenPlayer();
		}

		private void LookAroundLogic()
		{
			// Increment the timer to rotate the house guard to look around or stop looking (focus on one direction)
			timer += Time.deltaTime;

			// Rotate the house guard to look around
			if (timer >= 0.0f && timer <= lookAroundTime.value)
			{
				agent.transform.eulerAngles += new Vector3(0.0f, rotationSpeed.value * Time.deltaTime, 0.0f);
			}

			// Stop rotating the house guard to make sure they look in one direction
			else if (timer > lookAroundTime.value && timer <= lookAroundTime.value * 2.0f)
			{
				agent.transform.eulerAngles += new Vector3(0.0f, 0.0f, 0.0f);
			}

			// Set the timer back to 0 after it exceeds whatever the look around time is times 2
			if (timer > lookAroundTime.value * 2.0f)
			{
				timer = 0.0f;
			}
		}

		private void ChangeRotationDirection()
		{
			// Make the house guard look the other way if it exceeds an angle
			if (agent.transform.eulerAngles.y >= rotationThreshold.value)
			{
				rotationSpeed.value = -rotationSpeed.value;
			}

			// Make the house guard look the other way if it's below an angle
			else if (agent.transform.eulerAngles.y <= -rotationThreshold.value)
			{
				rotationSpeed.value = rotationSpeed.value;
			}
		}

		private void SeenPlayer()
		{
			Collider[] houseGuardColliders = Physics.OverlapSphere(agent.transform.position, houseGuardRadius.value,
				collectorMask.value);

			foreach (Collider collider in houseGuardColliders)
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