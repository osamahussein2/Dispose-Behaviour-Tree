using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class MeleeAttackAT : ActionTask {

        public BBParameter<CastleGuardData> castleGuardData;
        public BBParameter<GarbageCollectorData> garbageCollectorData;

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
            // Make sure not show the hit alert and excalamation mark upon execution
            castleGuardData.value.hitAlert.SetActive(false);
            castleGuardData.value.exclamationMark.SetActive(false);

            castleGuardData.value.castleGuardStateText.text = "Castle Guard State: Attack player by melee combat";
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() 
		{
            // If the castle guard is close enough to the garbage collector for melee combat
            if (Vector3.Distance(agent.transform.position, castleGuardData.value.garbageCollector.transform.position) <=
                castleGuardData.value.meleeAttackRange)
            {
                timer += Time.deltaTime; // Increment the timer
                
                // Don't move the castle guard
                agent.transform.position += Vector3.zero;

                // Rotate the castle guard to look at the player
                Quaternion lookRotation = 
                    Quaternion.LookRotation(-(castleGuardData.value.garbageCollector.transform.position - 
                    agent.transform.position).normalized);

                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation,
                    Time.deltaTime);

                // Don't make the castle guard rotate their x axis to prevent them from looking up or down
                if (agent.transform.rotation.x <= 0 || agent.transform.rotation.x > 0)
                {
                    agent.transform.rotation = new Quaternion(0.0f, agent.transform.rotation.y,
                        agent.transform.rotation.z, agent.transform.rotation.w);
                }

                // If the timer goes above 2, start attacking the player
                if (timer >= 2.0f)
                {
                    // Make the sword appear in their hand
                    sword = Object.Instantiate(castleGuardData.value.swordPrefab, agent.transform.position, Quaternion.identity);

                    garbageCollectorData.value.garbageCollectorHealth.value -= 5.0f;

                    castleGuardData.value.hitAlert.SetActive(true); // Show the hit alert above the player

                    timer = 0.0f;
                }
            }

            /* Otherwise, end this action after the player and castle guard's distance exceeds the melee range of 
            the castle huard */
            else if (Vector3.Distance(agent.transform.position,
                castleGuardData.value.garbageCollector.transform.position) > castleGuardData.value.meleeAttackRange)
            {
                EndAction(true);
            }

            /* If sword is not invalid, move the sword with the guard's hand and rotate it as if they're attacking
            the player */
			if (sword != null)
			{
                sword.transform.position = castleGuardData.value.castleGuardHand.transform.position;
                sword.transform.eulerAngles += new Vector3(10.0f, 0.0f, 0.0f);

                Object.Destroy(sword, 2.0f); // Destroy the sword in 2 seconds after it spawns

                if (sword.transform.eulerAngles.x >= 90.0f)
                {
                    sword.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                }
            }

            // If the timer is greater than 1 and the hit alert is still visible, hide the hit alert
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