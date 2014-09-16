using UnityEngine;
using System.Collections;

public class AIObjectAvoid : AIBehaviour {
	private Transform trans;
	private AISeek seeker;
	private const float RAYLENGTH = 12f;
	private const float WHISKER = 8f;
	private const float MARGIN = 4f;

	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.ObjectAvoid; }
	}

	public AIObjectAvoid(AIState state, Transform obj, float weight, AISeek seek){
		trans = obj;
		seeker = seek;
		this.State = state;
		this.Weight = weight;
	}

	public override AIDynamic GetDynamics(){
		AIDynamic ai = new AIDynamic();
		ai.Linear = Vector3.zero;
		ai.Angular = 0;

		if(State.LinearVelocity.magnitude < 0.3f)
			return ai;

		bool hit1 = false;
		bool hit2 = false;
		bool hit3 = false;
		Vector3 ray = State.LinearVelocity.normalized;
		Vector3 ray_left = Quaternion.AngleAxis(-20, Vector3.up) * ray;
		Vector3 ray_right =  Quaternion.AngleAxis(20, Vector3.up) * ray;
		RaycastHit ray_info1; //left
		RaycastHit ray_info2 ; //middle
		RaycastHit ray_info3; //right

		RaycastHit min_hit = new RaycastHit();
		min_hit.distance = RAYLENGTH + 1f;

		if(Physics.Raycast(new Ray(trans.position, ray_left), out ray_info1, WHISKER)){
		    min_hit = ray_info1;
			hit1 = true;
		}

		if(Physics.Raycast(new Ray(trans.position, ray), out ray_info2, RAYLENGTH) && 
		   ray_info2.distance < min_hit.distance){
			min_hit = ray_info2;
			hit2 = true;
		}

		if(Physics.Raycast(new Ray(trans.position, ray_right), out ray_info3, WHISKER) && 
		   ray_info3.distance < min_hit.distance){
			min_hit = ray_info3;
			hit3 = true;
		}

		Debug.DrawLine (trans.position, trans.position + ray_left * WHISKER, Color.blue);
		Debug.DrawLine (trans.position, trans.position + ray * RAYLENGTH, Color.cyan);
		Debug.DrawLine (trans.position, trans.position + ray_right * WHISKER, Color.blue);

		if(hit2 && !(hit1 || hit3)){
			Vector3 perm_target = State.Target;
			State.Target = trans.position + ray_right * 4f;
			ai = seeker.GetDynamics();
			State.Target = perm_target;
			return ai;
		}
	    else if(hit1 || hit2 || hit3){
			Vector3 perm_target = State.Target;
			Vector3 normal = min_hit.normal * MARGIN;

			State.Target = min_hit.point + normal;
			ai = seeker.GetDynamics();
			State.Target = perm_target;
			return ai;
		}
		else
			return ai;
	}
}
