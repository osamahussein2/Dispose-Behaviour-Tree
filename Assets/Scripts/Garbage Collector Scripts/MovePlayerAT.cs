using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Unity.VisualScripting;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class MovePlayerAT : ActionTask {

		public BBParameter<GarbageCollectorData> garbageCollectorData;
		public static GameObject meleeWeapon;

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
			// If a garbage is found in the scene, destroy it
			if (GameObject.FindWithTag("Garbage"))
			{
				Object.Destroy(GameObject.FindWithTag("Garbage"));
			}

            garbageCollectorData.value.playerStateText.text = "Player State: Moving/attacking by player input";
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() 
		{
			// Get the horizontal and vertical values
			float horizontal = Input.GetAxisRaw("Horizontal");
			float vertical = Input.GetAxisRaw("Vertical");

            // Make the camera follow the player around by adding camera offset
            Camera.main.transform.position = agent.transform.position + garbageCollectorData.value.cameraOffset;

			// Set the movement using the horizontal and vertical (in z axis for moving forward/backward) values
            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);

			// Move the player by their movement vector and their player move speed (using delta time)
			agent.transform.position += movement * garbageCollectorData.value.playerMoveSpeed * Time.deltaTime;

			// If the player presses space and the melee weapon is invalid
			if (Input.GetKeyDown(KeyCode.Space) && meleeWeapon == null)
			{
				// Set the melee weapon to instantiate its prefab for melee attack
				meleeWeapon = Object.Instantiate(garbageCollectorData.value.playerMeleeWeaponPrefab,
					agent.transform.position + new Vector3(0.8f, 0.0f, 0.0f), Quaternion.identity);
            }

			if (meleeWeapon != null)
			{
				// Set the melee weapon to move forward as if the player is melee attacking something
                meleeWeapon.transform.localPosition += Vector3.forward * 0.5f;

				// If the melee weapon hits the castle guard, decrease their health and destroy the weapon
				if (!garbageCollectorData.value.castleGuard.IsDestroyed() && 
					Vector3.Distance(garbageCollectorData.value.castleGuard.transform.position, 
					meleeWeapon.transform.position) <= 1.0f)
				{
					CastleGuardData.castleGuardHealth.value -= 5.0f;
                    Object.Destroy(meleeWeapon);
                }

                // If the melee weapon hits the house guard, decrease their health and destroy the weapon
                if (!garbageCollectorData.value.houseGuard.IsDestroyed() &&
                    Vector3.Distance(garbageCollectorData.value.houseGuard.transform.position,
                    meleeWeapon.transform.position) <= 1.0f)
                {
                    HouseGuardData.houseGuardHealth.value -= 5.0f;
                    Object.Destroy(meleeWeapon);
                }

                Object.Destroy(meleeWeapon, 0.1f);
            }

			// If the player isn't holding any of the movement keys, end this action
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) &&
                !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.LeftArrow) && 
				!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && 
				!Input.GetKey(KeyCode.DownArrow) || meleeWeapon.IsDestroyed())
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