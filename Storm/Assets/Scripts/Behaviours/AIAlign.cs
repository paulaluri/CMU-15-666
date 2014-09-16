using UnityEngine;
using System.Collections;

public class AIAlign : AIBehaviour {
	private Transform trans;

	//private float TARGET_RADIUS = 1f;
	//private float SLOW_RADIUS = 2f;
	//private float TIME_TO_TARGET = 0.1f;

	public AIAlign(AIState state, Transform obj, float weight){
		trans = obj;
		this.State = state;
		this.Weight = weight;
	}

	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.Align; }
	}
	
	public override AIDynamic GetDynamics(){
		AIDynamic ai = new AIDynamic();
		/* For now, simply lerp this out */
		Vector3 rotation = trans.rotation.eulerAngles;
		if(State.LinearVelocity.magnitude > 1f){
		    rotation.y = GetDirectionFromVector2D(State.LinearVelocity);
		    trans.rotation = Quaternion.Lerp(trans.rotation, 
		                                          Quaternion.Euler(rotation), 
		                                          Time.deltaTime*5);
		}
		/*float rotation = 0;
		float targetRotation;

		rotation = GetDirectionFromVector2D (State.LinearVelocity) - trans.rotation.eulerAngles.y;

		while(rotation > 180)
			rotation -= 360;
		while(rotation < -180)
			rotation += 360;

		float angleMag = Mathf.Abs (rotation);
		
		AIDynamic ai = new AIDynamic();
		ai.Angular = 0;
		ai.Linear = Vector3.zero;

		if(angleMag < TARGET_RADIUS){
			State.AngularVelocity = 0;
			return ai;
		}

		if(angleMag > SLOW_RADIUS)
			targetRotation = 6f;
		else
			targetRotation = 6f * angleMag / SLOW_RADIUS;

		targetRotation *= (rotation / angleMag);

		ai.Angular = trans.rotation.eulerAngles.y - targetRotation;
		ai.Angular /= TIME_TO_TARGET;

		float a = Mathf.Abs (ai.Angular);

		if(a > 2f){
			ai.Angular  /= a;
			ai.Angular  *= 2f;
		}*/

		return ai;
	}
}
