
using UnityEngine;

public class weapon : MonoBehaviour {
    //public variables
    public Transform player1;
    public Transform player2;
    public Transform self;
    public Rigidbody2D selfRigid;
    public BoxCollider2D selfCollider;
    public float power;
    public float maxPower;
    public float minPower;
    public float powerIncrease;
    public float range;
    public float minRange;
    public float maxRange;
    public float rangeIncrease;
    public float rotationalSpeed;
    public float rotationalSpeedMin;
    public float rotationalSpeedMax;
    public float rotationalSpeedIncrease;
    public int player1Wins = 0;
    public int player2Wins = 0;

    public bool alternate;


    //private variables
    bool thrown = false;
    bool held = false;
    int tether = 0;
    int thrownBy = 0;
    float trig1;
    float trig2;
    bool preped1;
    bool preped2;
    float lookHAxis;
    float lookVAxis;
    float lookHAxis2;
    float lookVAxis2;
    Vector2 lastDir1;
    Vector2 lastDir2;

    public bool test;




    void Update () {

        //decreases range
        if (range > 0 && thrown) {
            range = range - 1;

            //rotates the axe
            selfRigid.rotation = range * rotationalSpeed;
        }
        //sets axe into resting
        if (range <= 0 && thrown) {

            //stops axe movement and resets status
            thrown = false;
            selfRigid.velocity = Vector2.zero;
            selfRigid.rotation = 0;
            thrownBy = 0;

            //reset rotation
            rotationalSpeed = rotationalSpeedMin;


        }

        //updates location and state if being held
        if (tether == 1)
        {
            self.transform.position = player1.position;
        }
        else if (tether == 2)
        {
            self.transform.position = player2.position;
        }
        else {
            //resets properties if not being held
            held = false;
        }

        //checks if player throws weapon
        if (held) {
            //only seeks trigger input if held
            trig1 = Input.GetAxis("Triggers");
            trig2 = Input.GetAxis("Triggers2");
            //calls throwWep function for each player
            if (tether == 1 && trig1 != 0)
            {
                throwWep(1);
            }
            else if (tether == 2 && trig2 != 0)
            {
                throwWep(2);
            }
        }

        //updates look for axe direction
        lookHAxis = Input.GetAxis("RightJoystickH");
        lookVAxis = Input.GetAxis("RightJoystickV");
        lookHAxis2 = Input.GetAxis("RightJoystickH2");
        lookVAxis2 = Input.GetAxis("RightJoystickV2");

        //converts controller input into vectors
        if (lookHAxis != 0 || lookVAxis != 0) {
            lastDir1 = new Vector2(lookHAxis, -lookVAxis);
        }
        if (lookHAxis2 != 0 || lookVAxis2 != 0)
        {
            lastDir2 = new Vector2(lookHAxis2, -lookVAxis2);
        }
        
        //throw the axe after trigger is released
        if (preped1 && trig1 == 0) {
            if (alternate)
            {
                selfRigid.AddForce(lastDir1.normalized * 1000);
            }
            else
            {
                selfRigid.AddForce(lastDir1.normalized * power);
            }
            //reset power
            power = minPower;
            //ensure axe cant hit self
            thrownBy = tether;
            //reset tether
            tether = 0;

            //update status
            thrown = true;
            held = false;
            preped1 = false;

        }
        //throw the axe after trigger is released
        if (preped2 && trig2 == 0)
        {
            selfRigid.AddForce(lastDir2.normalized * power);
            //reset power
            power = minPower;
            //ensure axe cant hit self
            thrownBy = tether;
            //reset tether
            tether = 0;

            //update status
            thrown = true;
            held = false;
            preped2 = false;
        }

        //switch between types
        if (Input.GetKeyDown("space")) {
            if (alternate)
            {
                alternate = false;
            }
            else {
                alternate = true;
            }
        }


    }


    //detect if player has touched axe
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if player picks up axe
        if (!held && !thrown)
        {
            if (collision.name == "Player1")
            {
                tether = 1;
                held = true;
            }
            else if (collision.name == "Player2")
            {
                tether = 2;
                held = true;
            }
        }
        //checks if axe hits object
        else if (thrown)
        {
            if (thrownBy == 1 && collision.name != "Player1")
            {
                range = 0;
            }
            else if (thrownBy == 2 && collision.name != "Player2")
            {
                range = 0;
            }
            //checks death
            if ((thrownBy == 1 && collision.name == "Player2") || (thrownBy == 2 && collision.name == "Player1")) {
                if (collision.name == "Player2")
                {
                    player1Wins++;
                }
                else if (collision.name == "Player1") {
                    player2Wins++;
                }
                Debug.LogFormat("{0},{1}\n", player1Wins, player2Wins);
                self.position = new Vector2(0, 0);
                player1.position = new Vector2(-6.5f, 0);
                player2.position = new Vector2(6.5f, 0);
            }
        }

    }

    public void throwWep(int player) {

        //increase power per frame
        if (power < maxPower)
        {
            power += powerIncrease;
        }

        //ensures range is more than minimum
        if (range < minRange) {
            range = minRange;
        }

        //increase range per frame
        if (range < maxRange)
        {
            range += rangeIncrease;
        }

        //ensures rotational speed is more than the min
        if (rotationalSpeed < rotationalSpeedMin) {
            rotationalSpeed = rotationalSpeedMin;
        }

        //increases rotatinoal speed over time
        if (rotationalSpeed < rotationalSpeedMax) {
            rotationalSpeed += rotationalSpeedIncrease;
        }

        //alternate control scheme sets range and rotatinalspeed as constant values last
        if(alternate) {
            range = 9999999;
            rotationalSpeed = 20;
        }

        //throws for player 1
        if (tether == 1)
        {
            //setup shot
            preped1 = true;

        }
        //throws for player 2
        else if (tether == 2)
        {
            //setup shot
            preped2 = true;
        }
    }
}
