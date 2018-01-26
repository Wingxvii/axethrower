
using UnityEngine;

public class movement : MonoBehaviour {

    //speeds
    public float movespeed;
    public Vector2 dir;
    public float lookAngle;
    public Rigidbody2D player;
    public Vector2 lookDir;

    //movement variables
    public float moveHAxis;
    public float moveVAxis;
    public float lookHAxis;
    public float lookVAxis;

    // updates once per frame
    void FixedUpdate () {

        //updates movement
        moveHAxis = Input.GetAxis("LeftJoystickH");
        moveVAxis = Input.GetAxis("LeftJoystickV");
        //updates faceing
        lookHAxis = Input.GetAxis("RightJoystickH");
        lookVAxis = Input.GetAxis("RightJoystickV");

        //converts left joystick input into movement 
        dir = new Vector2(moveHAxis, -moveVAxis).normalized;
        player.velocity += dir*movespeed;

        //handles looking

        if (lookHAxis != 0 && lookVAxis != 0)
        {
            lookAngle = Mathf.Atan2(lookHAxis, lookVAxis) * Mathf.Rad2Deg;
        }
        player.rotation = lookAngle;

    }
}
