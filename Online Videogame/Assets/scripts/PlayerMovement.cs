using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public CharacterController controller;
    public float speed = 12;

    public float gravity = -9.81f;
    public Vector3 velocity;

    public float groundDistance = 0.4f;
    public LayerMask floorMask;
    bool isGrounded;

    public Animator animator;
    Vector2 currentAnimBlendVec;
    Vector2 animVelocity;

    [SerializeField]
    private float animSmoothTime = 0.1f;
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(transform.position, groundDistance, floorMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(!GameManager.Instance.GameEnded)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(3 * -2 * gravity);
                animator.SetTrigger("Jump");
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector2 input = new Vector2(x, z);

            currentAnimBlendVec = Vector2.SmoothDamp(currentAnimBlendVec, input/2, ref animVelocity, animSmoothTime);

            Vector3 move = transform.right * x * speed + transform.forward * z *speed;
            controller.Move(move * Time.deltaTime);


            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            animator.SetFloat("MoveX", currentAnimBlendVec.x);
            animator.SetFloat("MoveZ", currentAnimBlendVec.y);
        }
       

    }
}
