using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Threading;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

    public class ShootPlayerAT : ActionTask {

		public BBParameter<HouseGuardData> houseGuardData;
		private GameObject bullet;
		private float timer;

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
            // Make sure timer is set to 0 after executing this task
            timer = 0.0f;
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			DecreaseInteractionLevel();
            InstantiateBullets();
            RotateToLookAtPlayer();
            FarAwayFromPlayer();
        }

		private void InstantiateBullets()
		{
            timer += Time.deltaTime; // Increment the timer

			// Spawn the bullet after the timer exceeds the firing cooldown time
			if (timer >= houseGuardData.value.bulletFireCooldownTimer)
			{
				bullet = Object.Instantiate(houseGuardData.value.bulletPrefab, agent.transform.position, 
					agent.transform.rotation);

                timer = 0.0f; // Reset timer to 0
			}

			// If a bullet has spawned, move it towards the player
			if (bullet != null)
			{
				bullet.transform.position += (houseGuardData.value.player.transform.position - 
					bullet.transform.position) * houseGuardData.value.bulletSpeed * Time.deltaTime;

				// If the bullet is close enough to the player, destroy it
				if (Vector3.Distance(houseGuardData.value.player.transform.position, bullet.transform.position) <= 0.2f)
				{
					Object.Destroy(bullet);
				}

				// Destroy the bullet in 3 seconds just in case if it doesn't destroy at all
                Object.Destroy(bullet, 3.0f);
            }
        }

		private void RotateToLookAtPlayer()
		{
			// Rotate the house guard to look at the player when shooting them
            Quaternion lookRotation = Quaternion.LookRotation(-(houseGuardData.value.player.transform.position - 
				agent.transform.position).normalized);

            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, 
				Time.deltaTime * houseGuardData.value.rotationSpeed);
        }

		private void FarAwayFromPlayer()
		{
			// If the house guard and player distance is far from the house guard's radius, destroy bullets
			if (Vector3.Distance(agent.transform.position, houseGuardData.value.player.transform.position) >= 
				houseGuardData.value.houseGuardRadius)
			{
                Object.Destroy(bullet);
                EndAction(true); // And end this action too
			}
		}

		private void DecreaseInteractionLevel()
		{
			// Decrease the interaction slider values for both house and castle guards
            houseGuardData.value.houseGuardInteractionSlider.value -=
                houseGuardData.value.houseGuardInteractionDecreaseRate * Time.deltaTime;

            houseGuardData.value.castleGuardInteractionSlider.value -=
                houseGuardData.value.castleGuardInteractionDecreaseRate * Time.deltaTime;
        }

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}