using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMovementController : MonoBehaviour
{

    // Speed the player is moving.
    public float speed = 5f;

    public float ladderSpeed = 5f;

    // Speed the player is jumping at.
    public float jumpSpeed = 8f;

    // Essentially draws a circle around the players feet to see if they
    // are touching ground.
    public float groundCheckRadious;

    // Used on an empty game object that is attached to the player feet to
    // determine if the player is touching the ground or not.
    public Transform groundCheck;

    // This is the layer we state is the ground.
    // This is linked in unity.
    public LayerMask groundLayer;

    // The fall detector game object.
    public GameObject fallDetector;

    // Holds a refernce to the UI's text for the coin count.
    public Text coinText;

    // Direction player is moving
    private float direction = 0f;

    // stores verticle input.
    private float verticle;

    // Are we in front of a ladder?
    private bool isLadder;

    // Are we climbing?
    private bool isClimbing;

    private bool canInteract;

    // Player object with Rigidbody2D
    private Rigidbody2D player;

    private SpriteRenderer playerSprite;

    // Are we touching the ground?
    private bool isTouchingGround;

    private bool isInteracting;

    private bool dialogueIsOpen;

    // Hooks into the Animator for the player movement animations (walk, idle and jump)
    private Animator playerMovementAnimation;

    // Where the player should respawn when colliding with the fall detector.
    private Vector3 respawnPoint;

    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the Rigid Body 2d Component from the player object.
        player = GetComponent<Rigidbody2D>();

        // Get the player sprite. This is used to adjust sorting layer for ladders.
        playerSprite = GetComponent<SpriteRenderer>(); 

        // Gets the animator component for the player movement animation.
        // This is already attached to the player.
        playerMovementAnimation = GetComponent<Animator>();

        // Set the respawn point to where the player is when the game launches:
        respawnPoint = transform.position;

        // Set the initial value:
        coinText.text = "Coins: " + CoinCollection.totalCoins;
    }

    // Update is called once per frame
    void Update()
    {

        // Find the ground check object which is at the players feet.
        // Uses the radius, speficied in unity, to draw a circle around the players feet.
        // Then we check if that circle is overlappin any object on the ground the layer, including the ground it's self.
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadious, groundLayer);

        // Determines which direction they are moving horizontally.
        // This could be negative: a or left arrow.
        // This could be positive: d or right arrow.
        // This number could be zero if the player is not moving.
        direction = Input.GetAxis("Horizontal");

        // Get verticle movement information of up and down, w or s.
        verticle = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Submit") && canInteract)
        {
            dialogueIsOpen = true;
            isInteracting = true;
        }

        if (Mathf.Abs(verticle) > 0f && isLadder) {
            isClimbing = true;
        }

        // If the player is moving right:
        if (direction > 0f && !dialogueIsOpen) {
            // Increase the velocity with a Vector2 that holds the X and Y coordinates.
            // The Y will be the players default Y, foir later when there is jumping.
            player.velocity = new Vector2(direction * speed, player.velocity.y);

            // Make the player face right by using the transform method to set the scale.
            // The value used is the scale X and Y value of the character sprite.
            transform.localScale = new Vector2(1f, 1f);
        } else if (direction < 0f && !dialogueIsOpen) {
            player.velocity = new Vector2(direction * speed, player.velocity.y);

            // Make the player face left by usin gthe transform method to set the scale of the X and Y
            // to -1, -1 (these values come fro the character sprites scale.
            transform.localScale = new Vector2(-1f, 1f);
        } else  {
            // Reduce X velocity to 0.
            player.velocity = new Vector2(0, player.velocity.y);
        }

        // Lets see if the player is pressing space (default) to jump:
        if (Input.GetButtonDown("Jump") && isTouchingGround && !dialogueIsOpen) {
            // Keep the players X velcity and have them move up at a speed of eight.
            // This allows players to jump while moving in either left or right.
            // The jump speed can be adjusted in unity.
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }

        // Update the animators speed variable for the character movement animation:
        // Using the Mathf.Abs we can make sure this float is always a positive number.
        playerMovementAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));

        // Next lets update the OnGround Variable and set it to the isTouchingGround, so we can transition
        // between idle <-> jump and moving -> jump.
        playerMovementAnimation.SetBool("OnGround", isTouchingGround);

        // Now we make the fall detector follow the player, but only on the x axis.
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void FixedUpdate() {
        if (isClimbing) {
            // Remove collision from the player and the ground, so they can pass through.
            Physics2D.IgnoreLayerCollision(0, 6);

            playerMovementAnimation.SetLayerWeight(1, 1);

            // Make the player a higher sorting level, so they are infront of the ladder.
            playerSprite.sortingOrder = 2;

            // Allow the player to climb up at the specified ladder speed.
            player.gravityScale = 0f;
            player.velocity = new Vector2(player.velocity.x, verticle * ladderSpeed);

            playerMovementAnimation.speed = Mathf.Abs(verticle);
        } else {

            // Stop ignoring the collision dectection between the ground layer and the player.
            Physics2D.IgnoreLayerCollision(0, 6, false);

            // Put the player back to their sorting layer.
            playerSprite.sortingOrder = 0;

            // Give the player gravity.
            player.gravityScale = 1f;

            // When we are falling, increase the fall speed, this effects jumping.
            if (player.velocity.y < 0f)
            {
                player.velocity += Vector2.up * Physics2D.gravity * (2.5f - 1) * Time.deltaTime;
            }
        }

        if (isInteracting && canInteract)
        {
            // Interact with the interactable object (npc/item what ever).
            dialogueIsOpen = interactable.interact(dialogueIsOpen);

            isInteracting = false;
        }
    }

    /**
     *  Used to detect the colider that the player is interacting with or entering  into.
     *  This is run every time a player enters a collider, so we can use tags to see which
     *  collider and then do things appropriatly.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        // If the player has collided with the FallDetector tag (colider object) then 
        // we respawn them at the starting point.
        if (collision.tag == "FallDetector") {
            transform.position = respawnPoint;
        } else if (collision.tag == "Checkpoint") {
            // If the player has collided with a checkpoint collision object, then we update their respawn point.
            respawnPoint = transform.position;
        } else if (collision.tag == "Ladder") {
            // When we enter the ladder collider.
            isLadder = true;
        } else if (collision.CompareTag("Bottom of Ladder")) {
            // Make collision happen between the player and the ground.
            Physics2D.IgnoreLayerCollision(0, 6, false);
        } else if (collision.CompareTag("Portal")) {
            // Move the player to the next scene when they enter the portal.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // Set the players respawn point.
            respawnPoint = transform.position;
        } else if (collision.CompareTag("Portal Back")) {
            // Move the player to the previous scene when they enter the portal.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

            // Set the players respawn point.
            respawnPoint = transform.position;
        } else if (collision.CompareTag("Coin")) {

            CoinCollection.totalCoins += 1;

            CoinSceneDeletion.coinsToDelete.Add(collision.gameObject.name, true);

            // Set the value when we collect a coin.
            coinText.text = "Coins: " + CoinCollection.totalCoins;

            // Remove the coin from the game:
            Destroy(collision.gameObject);
        } else if (collision.CompareTag("Interactable")) {
            canInteract = true;

            interactable = collision.GetComponent<Interactable>();

        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        // When we leave the ladder ...
        if (collision.CompareTag("Ladder")) {
            isLadder   = false;
            isClimbing = false;

            playerMovementAnimation.SetLayerWeight(1, 0);
        }

        if (collision.CompareTag("Interactable"))
        {
            canInteract = false;

            interactable = null;
        }
    }
}
