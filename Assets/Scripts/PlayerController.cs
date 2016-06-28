using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public GameObject theWorld;
    public float rotationAmount;
    public float rotationSpeed;
    public Vector3 destEuler = new Vector3(0, 0, 0);
    public Text growCounterText;
    public CameraFollower cameraReference;
    public GameController gameControllerReference;
    public int thisGrowCounter;

    private Vector3 currEuler = new Vector3(0, 0, 0);
    private Rigidbody2D myRigidBody;
    private bool isJumping;
    private float scaleModifier = 0.2f;
    private Vector3 baseScale;
    private float jumpModifier;
    private float gravityY;
    private float gravityModifier;


    // Use this for initialization
    void Start()
    {
        rotationAmount = 5.0f;
        rotationSpeed = 2.0f;
        theWorld.transform.eulerAngles = destEuler;
        myRigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        isJumping = true;
        baseScale = new Vector3(0.4f, 0.4f, 0.4f);
        thisGrowCounter = 1;
        jumpModifier = 7000.0f;
        gravityY = -9.81f;
        gravityModifier = 15;
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            Debug.Log("Jump");
            
            myRigidBody.AddForce(new Vector2(0.0f, jumpModifier));
            isJumping = true;

        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            GameObject newPlayer;

            for (int i = 1; i < thisGrowCounter; i++)
            {
                //Debug.Log("Robię kopię.");
                this.gameObject.transform.localScale = baseScale;
                newPlayer = Instantiate<GameObject>(this.gameObject);
                newPlayer.tag = "PlayerClone";
                newPlayer.GetComponent<PlayerController>().thisGrowCounter = 1;
            }
            thisGrowCounter = 1;
        }

        if (Input.GetKey(KeyCode.W) && this.tag == "Player")
        {
            GameObject[] clones = GameObject.FindGameObjectsWithTag("PlayerClone");
            for(int i = 0; i< clones.Length; i++)
            {
                GameObject playerClone;
                playerClone = GameObject.FindGameObjectWithTag("PlayerClone");
                float distance = Vector3.Distance(this.transform.position, playerClone.transform.position);
                this.thisGrowCounter = gameControllerReference.growCounter;
                GameObject.Destroy(playerClone);
            }
            float currScale = this.gameObject.transform.localScale.x;
            this.gameObject.transform.localScale = new Vector3(baseScale.x + (scaleModifier*thisGrowCounter), baseScale.y + (scaleModifier*thisGrowCounter), 1);
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (destEuler.z < 45f)
            {
                //Debug.Log("Right");
                destEuler.z += rotationAmount;
                Physics2D.gravity = new Vector2(destEuler.z, gravityY);

            }
            else
            {
                destEuler.z = 45f;
                Physics2D.gravity = new Vector2((destEuler.z/gravityModifier), gravityY);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (destEuler.z > -45f)
            {
                //Debug.Log("Left");
                destEuler.z -= rotationAmount;
                Physics2D.gravity = new Vector2((destEuler.z / gravityModifier), gravityY);
            }
            else
            {
                destEuler.z = -45;
                Physics2D.gravity = new Vector2((destEuler.z / gravityModifier), gravityY);
            }
        }
        else
        {
            if(destEuler.z != 0)
            {
                if (destEuler.z < 0)
                {
                    destEuler.z += rotationAmount;
                    Physics2D.gravity = new Vector2((destEuler.z / gravityModifier), gravityY);
                }
                else
                {
                    destEuler.z -= rotationAmount;
                    Physics2D.gravity = new Vector2((destEuler.z / gravityModifier), gravityY);
                }
            }
        }

        currEuler = Vector3.Lerp(currEuler, destEuler, Time.deltaTime * rotationSpeed);
        theWorld.transform.eulerAngles = currEuler;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Level")
        {
            isJumping = false;
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            thisGrowCounter -= 1;
            gameControllerReference.UpdateGrowCounter(-1);
            float currScale = this.gameObject.transform.localScale.x;
            this.gameObject.transform.localScale = new Vector3(currScale - scaleModifier, currScale - scaleModifier, 1);  
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Grow")
        {
            float currScale = this.gameObject.transform.localScale.x;
            this.gameObject.transform.localScale = new Vector3(currScale + scaleModifier, currScale + scaleModifier, 1);
            this.thisGrowCounter += 1;
            gameControllerReference.UpdateGrowCounter(1);
            GameObject.Destroy(other.gameObject);
        }
        else if(other.tag == "Death")
        {
            cameraReference.target = GameObject.Find("Player(Clone)");
            cameraReference.target.name = "Player";
            gameControllerReference.UpdateGrowCounter(-thisGrowCounter);
            GameObject.Destroy(this.gameObject);
        }
        else if (other.tag == "Finish" && this.gameObject.tag == "Player")
        {
            gameControllerReference.Finish();
        }
    }
}
