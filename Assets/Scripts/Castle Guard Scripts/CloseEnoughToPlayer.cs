using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class CloseEnoughToPlayer : ConditionTask {

		private bool isCloseToPlayer;

        public BBParameter<CastleGuardData> castleGuardData;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
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
            // If the garbage collector is close enough, stop the castle guard from moving and start attacking the player in 2 seconds
            if (Vector3.Distance(agent.transform.position, castleGuardData.value.garbageCollector.transform.position) <=
                castleGuardData.value.meleeAttackRange)
            {
				isCloseToPlayer = true;
            }

			else
			{
				isCloseToPlayer = false;

                castleGuardData.value.hitAlert.SetActive(false);
            }

            return isCloseToPlayer;
		}
	}
}