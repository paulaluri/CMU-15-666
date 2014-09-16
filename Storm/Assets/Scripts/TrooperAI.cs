using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrooperAI : MonoBehaviour {
	public Transform target;
	public AIBehaviourType BehaviourSetting;
	public bool leader = false;
	public Transform[] targets;

	private List<AIBehaviour> behaviours;
	
	private AISeparate separation;
	private AICollisionAvoid avoid;
	private AIObjectAvoid rayAvoid;
	private AIState state;
	private Animator animator;

	public void SetSeparationTargets(Transform[] targets){
		separation.Targets = targets;
		avoid.Targets = targets;
	}

	public Vector3 GetVelocity(){
		return state.LinearVelocity;
	}

	// Use this for initialization
	void Awake () {
		//Initialize state
		state = new AIState();
     	state.MaxLinearAcceleration = 5f;
		state.AngularVelocity = 0f;
		state.MaxAngularAcceleration = 10f;
		state.MaxLinearVelocity = new Vector3(5f, 0, 5f);
		state.MaxAngularVelocity = 10f;
		state.LinearVelocity = state.MaxLinearVelocity.magnitude * this.transform.forward;
		if(target != null){
		    Vector3 temp = target.position;
		    temp.y = 0;
		    state.Target = temp;
		}
		else
			state.Target = Vector3.zero;

		//Reference animator
		animator = this.GetComponent<Animator>();
		//Mandatory Behaviours
		behaviours = new List<AIBehaviour>();

		AISeek seek = new AISeek(state, this.transform, 1f);
		behaviours.Add (seek);
		rayAvoid = new AIObjectAvoid(state, this.transform, 10f, seek);
		behaviours.Add (rayAvoid);
		behaviours.Add (new AIAlign(state, this.transform, 1f));
		separation = new AISeparate(state, this.transform, .3f);
		behaviours.Add (separation);
		avoid = new AICollisionAvoid(state, this.transform, 3f);
		behaviours.Add (avoid);

        if(BehaviourSetting == AIBehaviourType.ReachGoal)
			behaviours.Add (new AIArrive(state, this.transform, .4f));
		else if(BehaviourSetting == AIBehaviourType.Wander)
			behaviours.Add (new AIWander(state, this.transform, .1f));
	}
	
	// Update is called once per frame
	void Update () {
		//RUN BEHAVIOURS
		AIDynamic movement = new AIDynamic();
		movement = RunBehaviours();

		//UPDATE POSITION
		PositionUpdate (movement);

		//ANIMATION
		animator.SetFloat ("velocity", state.LinearVelocity.magnitude);
		if(state.LinearVelocity.magnitude > 0.1f)
		    animator.speed = state.LinearVelocity.magnitude / state.MaxLinearVelocity.magnitude;
		else
			animator.speed = 1;

		//TOROIDAL
		PositionWrap();
	}

	//Wrap position of object to plane
	private void PositionWrap(){
		float x = this.transform.position.x;
		float z = this.transform.position.z;
		
		if(x > 75)
			x -= 150;
		else if(x < -75)
			x += 150;
		
		if(z > 75)
			z -= 150;
		else if(z < -75)
			z += 150;
		
		Vector3 newPos = new Vector3(x, 0f, z);
		this.transform.position = newPos;
	}
	
	private void PositionUpdate(AIDynamic motion){
		//Set linear velocity to max
		if(state.LinearVelocity.magnitude > state.MaxLinearVelocity.magnitude)
			state.LinearVelocity = state.LinearVelocity.normalized * state.MaxLinearVelocity.magnitude;
		
		//Set angular velocity to max
		if(Mathf.Abs (state.AngularVelocity) > Mathf.Abs (state.MaxAngularVelocity))
			state.AngularVelocity = (state.AngularVelocity / Mathf.Abs (state.AngularVelocity)) * state.MaxAngularVelocity;

		//Update position and linear velocity
		this.transform.position += state.LinearVelocity * Time.deltaTime;
		state.LinearVelocity += motion.Linear * Time.deltaTime;
		
		//Update orientation and angular velocity
		Vector3 rotation = this.transform.rotation.eulerAngles;
		rotation.y += state.AngularVelocity * Time.deltaTime;
		this.transform.rotation = Quaternion.Euler (rotation);
		state.AngularVelocity += motion.Angular * Time.deltaTime;
	}
	
	private AIDynamic RunBehaviours(){
		AIDynamic movement = new AIDynamic();
		AIDynamic avoidDyn = new AIDynamic();

		//Iterate through behaviours
		foreach(AIBehaviour b in behaviours){
			AIDynamic dyn = b.GetDynamics();

		    movement.Angular += dyn.Angular * b.Weight;
			movement.Linear += dyn.Linear * b.Weight;

			if(b.BehaviourType == AIBehaviourType.ObjectAvoid || b.BehaviourType == AIBehaviourType.CollisionAvoid)
				avoidDyn.Linear += dyn.Linear * b.Weight * 5f;
		}

		
		if(avoidDyn.Linear.magnitude > 0){
			movement.Linear = avoidDyn.Linear;
		}

		return movement;
	}
}
