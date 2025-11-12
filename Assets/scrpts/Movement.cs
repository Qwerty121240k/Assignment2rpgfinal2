using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
//Stoped at doublejump titlecard 
public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isFacingright=true;
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
    public bool isgrounded;


    [Header("Wallcheck")]
    public Transform Wallcheckpos;
    public Vector2 Wallchecksize = new Vector2(0.05f, 0.5f);
    public LayerMask Walllayer;
    
    [Header("Wallmove")]
    public float wallslidespeed = 2f;
   public bool iswallsilide;
    [Header("Walljump")]
    //wall jump
    bool iswalljump;
    float wallJumpDirection;
    float wallJumpTime=0.5f;
     float wallJumptimer;
    public Vector2 wallJumpPOWER = new Vector2(5f,10f);

    [Header("dash")]
    public float dashspeed=20f;
    public float dashDuraton = 0.9f;
    public float dashcool = 0.2f;
    bool isdashing;
    bool candash=true;
    [Header("fall")]
    //fall
    public DisplayMessage displayMessage;
    private bool isFalling = false;
    public float fallStartTime = 0.5f;
    public float fallThreshold = 2.0f;
         public string loadSceneName;
    public bool deathState = false;
    public string showMessage;
 

    // Update is called once per frame
    void Update()
    {
        if (isdashing) { return; }

        groundcheck();
      
        fall();
        Wallslide();
        processwalljump();
       
        if (!iswalljump)
        {
            rb.linearVelocity = new Vector2(horizontalMovemont * MoveSpeed, rb.linearVelocity.y);
            flip();
        }
    }

    public void move(InputAction.CallbackContext context)
    {
        horizontalMovemont = context.ReadValue<Vector2>().x;

    }
    public void dash(InputAction.CallbackContext context)
    {
       if (context.performed && candash)
        {
            StartCoroutine(DashCoroutine());

        }

    }
    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        candash = false;
        isdashing = true;
        float dashDirection = isFacingright ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection* dashspeed,rb.linearVelocity.y);//dash move
        yield return new WaitForSeconds(dashDuraton);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        isdashing=false;
        Physics2D.IgnoreLayerCollision(7, 8, false);
        yield return new WaitForSeconds(dashcool);

        candash = true;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (Jumpsremaning>0)
        {


            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpPower);
                Jumpsremaning--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                Jumpsremaning--;
            }
        }
        //wall
        if (context.performed && wallJumptimer > 0f)
        {
            iswalljump=true;
            rb.linearVelocity= new Vector2(wallJumpDirection*wallJumpPOWER.x,wallJumpPOWER.y);//jumpaway
            wallJumptimer=0f;
            //flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingright = !isFacingright;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
             


            Invoke(nameof(Cancelwalljump), wallJumpTime + 0.1f);//walljump = 0.5 jump
        }
    }
    
        private void Wallslide() 
        {   if(!isgrounded & Wallcheck() & horizontalMovemont!=0)
        {
            iswallsilide = true ;
            Jumpsremaning = maxJumps; //addjump
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallslidespeed));
        }
        else { iswallsilide=false; }    

        }
    private void processwalljump()
    { if (iswallsilide)
        {
            iswalljump = false;
            
            wallJumpDirection = -transform.localScale.x;
            wallJumptimer = wallJumpTime;
            Jumpsremaning = maxJumps; //addjump
            CancelInvoke(nameof(Cancelwalljump));
        }
       else if (wallJumptimer > 0f) { wallJumptimer-=Time.deltaTime; }
    }
    private void Cancelwalljump()
    { iswalljump = false;
        
    }



    private void fall()
    {
        //addwallslide
      
        if (IsFalling ())
        {
            // The player is falling, start timing the fall.
            if (!isFalling)
            {
                isFalling = true;
                fallStartTime = Time.time;
            }
            // Check if the fall duration has exceeded the threshold
            if (Time.time - fallStartTime >= fallThreshold)
            { //if (iswallsilide = false)
                
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
            //displayMessage.ShowMessage(showMessage);
            // Delay for visual effect (optional)
            //Invoke("LoadLevel", 2f);
            SceneManager.LoadScene("Death");
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
        return GetComponent<Rigidbody2D>().linearVelocity.y < 0;
    }
}
