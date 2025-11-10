using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
//Stoped at doublejump titlecard 
public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isFacingright;
    [Header("movement")]
    public float MoveSpeed=5f;
   float horizontalMovemont;

    [Header("Jump")]
    public float JumpPower = 10f;
    //dounle
    public int maxJumps = 2;
    int Jumpsremaning;
    [Header("Groundcheck")]
    public Transform Groundcheckpos;
    public Vector2 Groundchecksize = new Vector2(0.5f, 0.05f);
    public LayerMask Groundlayer;
    bool isgrounded;
    [Header("Wallcheck")]
    public Transform Wallcheckpos;
    public Vector2 Wallchecksize = new Vector2(0.5f, 0.05f);
    public LayerMask Walllayer;
    
    [Header("Wallmove")]
    public float wallslidespeed = 2f;
   public bool iswallsilide;


    //fall
    public DisplayMessage displayMessage;
    private bool isFalling = false;
    private float fallStartTime = 0.5f;
    private float fallThreshold = 2.0f;
    public bool deathState = false;
    public string showMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovemont * MoveSpeed, rb.velocity.y);
        groundcheck();
        flip();
        fall();
        Wallslide();


    }
    public void move(InputAction.CallbackContext context)
    {
        horizontalMovemont = context.ReadValue<Vector2>().x;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (Jumpsremaning>0)
        {


            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpPower);
                Jumpsremaning--;
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                Jumpsremaning--;
            }
        }
    }
    
        private void Wallslide() 
        {   if(!isgrounded & Wallcheck() & horizontalMovemont!=0)
        {
            iswallsilide = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallslidespeed));
        }
        else { iswallsilide=false; }    

        }



    private void fall()
    {

        // Add this code to your update() script
        // Check if the player is falling
        if (IsFalling())
        {
            // The player is falling, start timing the fall.
            if (!isFalling)
            {
                isFalling = true;
                fallStartTime = Time.time;
            }
            // Check if the fall duration has exceeded the threshold
            if (Time.time - fallStartTime >= fallThreshold)
            {
                deathState = true;
            }
        }
        else
        {
            // The player is not falling, reset the fall timer.
            isFalling = false;
        }

        if (deathState == true)
        {
            // display win message
            displayMessage.ShowMessage(showMessage);
            // Delay for visual effect (optional)
            Invoke("LoadLevel", 2f);
        }
    }



    private void groundcheck()
    {
        if (Physics2D.OverlapBox(Groundcheckpos.position, Groundchecksize, 0, Groundlayer))
        {
            Jumpsremaning = maxJumps;
            isgrounded = true;
        }
        else
        {
            isgrounded = false;
        }



    }
    private bool Wallcheck()
    {
      return Physics2D.OverlapBox(Wallcheckpos.position, Wallchecksize, 0, Walllayer);
    }
    private void flip()
    {
     if(isFacingright&& horizontalMovemont<0||!isFacingright && horizontalMovemont >0)
        {
            isFacingright = !isFacingright;
            Vector3 ls =transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Groundcheckpos.position, Groundchecksize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Wallcheckpos.position, Wallchecksize);
    }
    bool IsFalling()
    {
        // You can implement your own logic here to determine if the player is falling.
        // For example, you can check if the player's vertical velocity is negative.
        return GetComponent<Rigidbody2D>().velocity.y < 0;
    }
}
