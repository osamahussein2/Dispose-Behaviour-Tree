using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class MeleeAttackAT : ActionTask {

        public BBParameter<CastleGuardData> castleGuardData;

        private float timer;

		private GameObject sword;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() 
		{
            castleGuardData.value.hitAlert.SetActive(false); // Make sure not show the hit alert upon execution
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() 
		{
            // If the garbage collector is close enough, stop the castle guard from moving and start attacking the player in 2 seconds
            if (Vector3.Distance(agent.transform.position, castleGuardData.value.garbageCollector.transform.position) <=
                castleGuardData.value.meleeAttackRange)
            {
				timer += Time.deltaTime;

				agent.transform.position += Vector3.zero;

                Quaternion lookRotation = Quaternion.LookRotation(-(castleGuardData.value.garbageCollector.transform.position -
                agent.transform.position).normalized);

                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation,
                    Time.deltaTime);

                if (timer >= 2.0f)
				{
                    // Make the sword appear in their hand
					sword = Object.Instantiate(castleGuardData.value.swordPrefab, agent.transform.position, Quaternion.identity);

                    castleGuardData.value.hitAlert.SetActive(true); // Show the hit alert above the player

                    timer = 0.0f;
				}
            }

            // If sword is not invalid, move the sword with the guard's hand and rotate it as if they're attacking the player
			if (sword != null)
			{
                sword.transform.position = castleGuardData.value.castleGuardHand.transform.position;
                sword.transform.eulerAngles += new Vector3(10.0f, 0.0f, 0.0f);

                Object.Destroy(sword, 2.0f);

                if (sword.transform.eulerAngles.x >= 90.0f)
                {
                    sword.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                }
            }

			if (timer >= 1.0f && castleGuardData.value.hitAlert.activeInHierarchy)
			{
                castleGuardData.value.hitAlert.SetActive(false);
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