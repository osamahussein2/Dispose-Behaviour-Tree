using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Threading;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

    public class ShootPlayerAT : ActionTask {

		public BBParameter<HouseGuardData> houseGuardData;
		public BBParameter<GarbageCollectorData> garbageCollectorData;

		private GameObject bullet;
		private float timer;
		private float hitAlertTimer;

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
			hitAlertTimer = 0.0f;

            houseGuardData.value.houseGuardStateText.text = "House Guard State: Shooting player";

            houseGuardData.value.shootingAlert.SetActive(true);
            houseGuardData.value.lookingAlert.SetActive(false);
            houseGuardData.value.movingToCastleGuardAlert.SetActive(false);
            houseGuardData.value.interactingWithCastleGuardAlert.SetActive(false);
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
			hitAlertTimer += Time.deltaTime;

			// Spawn the bullet after the timer exceeds the firing cooldown time
			if (timer >= houseGuardData.value.bulletFireCooldownTimer)
			{
                houseGuardData.value.shootingAlert.SetActive(false); // Hide the shooting alert too

                bullet = Object.Instantiate(houseGuardData.value.bulletPrefab, agent.transform.position, 
					agent.transform.rotation);

                timer = 0.0f; // Reset timer to 0
			}

			// If a bullet has spawned, move it towards the player
			if (bullet != null)
			{
				bullet.transform.position += (houseGuardData.value.player.transform.position - 
					bullet.transform.position) * houseGuardData.value.bulletSpeed * Time.deltaTime;

				// If the bullet is close enough to the player, destroy it and make the player lose health
				if (Vector3.Distance(houseGuardData.value.player.transform.position, bullet.transform.position) <= 0.2f)
				{
                    hitAlertTimer = 0.0f; // Reset the hit alert timer

                    houseGuardData.value.hitAlert.SetActive(true); // Show the hit alert above the player

                    garbageCollectorData.value.garbageCollectorHealth.value -= 2.0f;

                    Object.Destroy(bullet);
				}

				// Destroy the bullet in 3 seconds just in case if it doesn't destroy at all
                Object.Destroy(bullet, 3.0f);
            }

            // If the hit alert timer is greater than 1 and the hit alert is still visible, hide the hit alert
            if (hitAlertTimer >= 1.0f && houseGuardData.value.hitAlert.activeInHierarchy)
            {
                houseGuardData.value.hitAlert.SetActive(false);
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
                houseGuardData.value.shootingAlert.SetActive(false); // Hide the shooting alert
                houseGuardData.value.hitAlert.SetActive(false); // Hide the hit alert

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