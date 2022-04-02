using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public float speed = 6.0f;
    public float maxSpeed = 6.0f;
    public float rotationSpeed = 360f;

    private Vector3 movement = Vector3.zero;

    public Transform itemPlaceholder;
    Animator animator;

    private Item item = null;

    [SerializeField] SetPosition setPosOfPlaceHolder;

    void Start()
    {
        setPosOfPlaceHolder.enabled = true;
        setPosOfPlaceHolder.target = transform;//transform.Find("Model").GetChild(index).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
        {
            //transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(movement), transform.rotation, 0.45f);

            
            Quaternion q = transform.rotation;
            q = Quaternion.Lerp(Quaternion.LookRotation(Camera.main.transform.TransformDirection(movement)), transform.rotation, 0.45f);                    
            transform.rotation = Quaternion.Euler(0, q.eulerAngles.y, 0);
            
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            movement = Input.GetAxis("Vertical") * camForward + Input.GetAxis("Horizontal") * Camera.main.transform.right;

            movement *= speed;

            if (movement.x != 0f && movement.z != 0f) movement = movement / 1.425f;

            characterController.Move(movement * Time.deltaTime);
        }

        animator.SetBool("HasObject", item != null);
        animator.SetFloat("Speed", movement.magnitude);

        characterController.Move(Vector3.down * 20f * Time.deltaTime);
    }
}
