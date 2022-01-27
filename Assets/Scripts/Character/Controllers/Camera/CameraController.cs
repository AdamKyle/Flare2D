using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Used to get access to the player.
    // This is set in unity.
    public GameObject player;

    // Lets the player see further.
    public float offset;

    public float offsetY;

    // Smooths the motion of the camera following aspect.
    public float offsetSmoothing;

    // Store the players position.
    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This will only follow the player on the X and Y axis for up and down based movment.
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        if (player.transform.localScale.x > 0f) {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        } else {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        }

        if (player.transform.localScale.y > 0f) {
            playerPosition = new Vector3(playerPosition.x, playerPosition.y - offsetY, playerPosition.z);
        } else {
            playerPosition = new Vector3(playerPosition.x, playerPosition.y + offsetY, playerPosition.z);
        }

        // Make the camera smoothly catch up to the player.
        transform.position = Vector3.Lerp(transform.position, playerPosition, Time.deltaTime);
    }
}
