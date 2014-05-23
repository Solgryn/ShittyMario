using UnityEngine;
using System.Collections;

public class PlatformEngine : MonoBehaviour {
	public float gravityForce = 0.5f;
	public float movementSpeed = 5;
	public float jumpHeight = 5;

	private float verticalControl;
	private float horizontalControl;
	private float airVelocity;
	private CollisionFlags collisionFlags;
	private bool canJump = false;

	// Use this for initialization
	void Awake () {
		Application.targetFrameRate = 60; //Set FPS to 60
	}
	
	// Update is called once per frame
	void Update () {
		var controller = GetComponent<CharacterController>(); //Get character controller component (For moving character)

		ApplyGravity();
		ApplyControls();

		//Calculate motion
		var move = new Vector3(horizontalControl, 0, verticalControl);
		move *= movementSpeed; //Apply movement speed modifier
		move += new Vector3(0, airVelocity, 0); //Apply air velocity to movement
		Debug.Log(move);
		move *= Time.deltaTime; //Account for lag


		//Move the character
		collisionFlags = controller.Move(move); //Get collisions from movement
	}

	void ApplyGravity() {
		if(!IsGrounded()){ //If we're not grounded
			airVelocity -= gravityForce; //Add gravity to airVelocity
			canJump = false;
		} else {
			airVelocity = -0.01f; //"reset" the air velocity when on the ground
		}
	}

	void ApplyControls(){
		verticalControl = Input.GetAxisRaw("Vertical"); //Get vertical movement
		horizontalControl = Input.GetAxisRaw("Horizontal"); //Get horizontal movement

		if(Input.GetButtonDown("Jump") && canJump){
			airVelocity = jumpHeight;
			canJump = false;
		} else if(IsGrounded()){
			canJump = true;
		}
	}

	bool IsGrounded(){
		//CollisionFlags is a class used by the Character Controller component, it tells us where the player collided with something

		//Here we "and" the collisionFlags returned from the Move() method with the constant value CollidedBelow
		//CollidedBelow is like an "example value" that shows how the bits look like if collided with the ground.
		//If all the bits are the same (&: and) (i. e. we touched the ground) the result is 1
		//If any of the bits are different from each other, the result is 0
		//We return if the result is not equal to 0
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
}
