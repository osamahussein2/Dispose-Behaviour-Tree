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
            timer = 0.0f;
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			InstantiateBullets();
            RotateToLookAtPlayer();
            FarAwayFromPlayer();
        }

		private void InstantiateBullets()
		{
            timer += Time.deltaTime;

			if (timer >= houseGuardData.value.bulletFireCooldownTimer)
			{
				bullet = Object.Instantiate(houseGuardData.value.bulletPrefab, agent.transform.position, 
					agent.transform.rotation);

                timer = 0.0f;
			}

			if (bullet != null)
			{
				bullet.transform.position += (houseGuardData.value.player.transform.position - 
					bullet.transform.position) * houseGuardData.value.bulletSpeed * Time.deltaTime;

				if (Vector3.Distance(houseGuardData.value.player.transform.position, bullet.transform.position) <= 0.2f)
				{
					Object.Destroy(bullet);
				}

                Object.Destroy(bullet, 3.0f);
            }
        }

		private void RotateToLookAtPlayer()
		{
            Quaternion lookRotation = Quaternion.LookRotation(-(houseGuardData.value.player.transform.position - 
				agent.transform.position).normalized);

            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, 
				Time.deltaTime * houseGuardData.value.rotationSpeed);
        }

		private void FarAwayFromPlayer()
		{
			if (Vector3.Distance(agent.transform.position, houseGuardData.value.player.transform.position) >= 
				houseGuardData.value.houseGuardRadius)
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