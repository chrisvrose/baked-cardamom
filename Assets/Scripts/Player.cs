using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController playerCharacterController;
    public float yVelocity;

    [Min(0.1f)]
    public float mouseSensitivity;

    // shows up as a space in the editor, for aesthetics
    [Space(2)]

    [Range(0.1f,10)]
    public float playerGravity;
    [Range(0.2f,10)]
    public float jumpSensitivity;
    [Range(.5f, 10)]
    public float movementSensitivity;
    [HideInInspector]
    public Transform playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        // just get the camera and store it - we will need it
        playerCamera = transform.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // we will need the time elapsed a few times so better cache this
        float dt = Time.deltaTime;

        // set the camera stuff
        doCameraRotate(dt);
        // get the movement vector that was entered
        doPlayerMove(dt);
    }


    private void doCameraRotate(float dt)
    {
        var rotateX = Input.GetAxis("Mouse X") * mouseSensitivity;
        // the mouse Y controls are inverted for our purposes, moving down now should decrease the vals
        var rotateY = -Input.GetAxis("Mouse Y")*mouseSensitivity;

        // now, lets apply this mouse x rotation on the body - this will actually be the body rotating around the y axis
        transform.Rotate(Vector3.up * rotateX);


        // for the y rotation tho, lets locally rotate the camera - around the x axis, itll pan up and down
        playerCamera.localRotation *= Quaternion.Euler(rotateY, 0, 0);

    }


    /// <summary>
    /// Move a player
    /// </summary>
    /// <param name="dt">Delta time since last frame update</param>
    private void doPlayerMove(float dt)
    {
        var attemptMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // scale by the sensitivity
        attemptMove *= movementSensitivity;

        //multiply the vector by the players rotation matrix to get somewhere sane
        attemptMove = transform.rotation * attemptMove;

        // note that the above may push our nice flat vector off the xz plane - force it back in
        // we also want it to be the same magnitude as before the projection
        attemptMove = Vector3.ProjectOnPlane(attemptMove, Vector3.up).normalized * attemptMove.magnitude;

        if (playerCharacterController.isGrounded)
        {
            // if we are on the ground, check the jump axis and decide.
            // if it is zero, we are simply setting it to 0.
            yVelocity = Input.GetAxis("Jump") * jumpSensitivity;
        }
        else
        {
            // v = u+at

            yVelocity -= playerGravity * dt;
        }
        attemptMove.y = yVelocity;

        // actually move the character now
        // multiply by deltatime since we have to normalize by frametimings
        // motion => dS = v * dt
        playerCharacterController.Move(attemptMove * dt);
    }

}
