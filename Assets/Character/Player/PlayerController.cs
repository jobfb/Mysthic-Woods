using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if movemnt inpput is not 0, try to move
        if (movementInput != Vector2.zero) //movementInput == direction
        {
            bool success = TryMove(movementInput); //normal movement

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x,0)); // try to move only on X axys
                if (!success)
                {
                    success = TryMove(new Vector2(0,movementInput.y)); // try to move only on T axys
                }
            }
        }
    }

    private bool TryMove(Vector2 direction)
    // check for potencial colissions
    {
        int count = rb.Cast(
                 direction, // X and Y values between 1 and -1 that represent the direction form body to look  for collisions
                 movementFilter,//The settings that determine where a collision can occur on such as layer to collide with
                 castCollisions,//List of collisions to store the found collisions into after the Cast is finished
                 moveSpeed * Time.fixedDeltaTime + collisionOffset);// The amount to cast equal to the movement plus an offset ( moveSpeed * Time.fixedDeltaTime this will tell the magnitude of the movement)
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}