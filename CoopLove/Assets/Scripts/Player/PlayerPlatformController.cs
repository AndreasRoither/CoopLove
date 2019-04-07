using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformController : MonoBehaviour
{
    public CharacterController2D controller;
    private Player player;

    float horizontalMove = 0f;

    public float runSpeed = 40f;

    public bool crouch = false;
    public bool jump = false;
    public bool jumpButtonPressed = false;

    public void Start()
    {
        player = this.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.playerId == 0) {
            if (Input.GetButtonDown("Jump_Player1")) jump = true;
            if (Input.GetButtonDown("Crouch_Player1")) crouch = true;
            else if (Input.GetButtonUp("Crouch_Player1")) crouch = false;

            if (Input.GetButton("Jump_Player1")) jumpButtonPressed = true;
            else if (Input.GetButtonUp("Jump_Player1")) jumpButtonPressed = false;

            horizontalMove = Input.GetAxisRaw("Horizontal_Player1") * runSpeed;
        }
        else
        {
            if (Input.GetButtonDown("Jump_Player2")) jump = true;
            if (Input.GetButtonDown("Crouch_Player2")) crouch = true;
            else if (Input.GetButtonUp("Crouch_Player2")) crouch = false;

            if (Input.GetButton("Jump_Player2")) jumpButtonPressed = true;
            else if (Input.GetButtonUp("Jump_Player2")) jumpButtonPressed = false;

            horizontalMove = Input.GetAxisRaw("Horizontal_Player2") * runSpeed;
        }
    }

    private void FixedUpdate()
    {
        // Move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, jumpButtonPressed);
        jump = false;
    }
}
