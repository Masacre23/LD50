using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    private bool actionKeyUp = true;
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
        if (Input.GetKey(KeyCode.E) && actionKeyUp)
        {
            if (item != null)
               StartCoroutine(DropItem());

            Vector3 p1 = transform.position + characterController.center + Vector3.up * -characterController.height * 0.5F;
            Vector3 p2 = p1 + Vector3.up * characterController.height;
            Collider[] colliders = Physics.OverlapCapsule(p1 + transform.forward / 2f, p2 + transform.forward / 2f, 0.5f).Where(c => c.gameObject.tag == "Item").ToArray();

            if (colliders.Length > 0)
                PickItem(colliders.First().gameObject);
        }
      
    }
    private void Update()
    {
        if (!Input.GetKey(KeyCode.E))
        {
            actionKeyUp = true;

        }

    } 

    void PickItem(GameObject g)
    {
        Debug.Log("PickItem");

        actionKeyUp = false;
        g.transform.parent = GetComponentInChildren<SetPosition>().transform;
        g.transform.localPosition = Vector3.zero;
        g.transform.localEulerAngles = Vector3.zero;
        g.GetComponent<Rigidbody>().isKinematic = true;
        g.GetComponent<Collider>().enabled = false;
        item = g.GetComponentInChildren<Item>();
    }


    IEnumerator DropItem()
    {
        if (item!= null)
        {
            Item obj = item;
            item = null;

            Debug.Log("DROP");
            actionKeyUp = false;

            obj.transform.localPosition += Vector3.up * 2f;
            obj.transform.parent = null;
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().AddExplosionForce(5f, obj.transform.position - Vector3.up+ Random.insideUnitSphere, 5f);
            obj.GetComponent<Rigidbody>().AddTorque(new Vector3(2f,2f,2f)+Random.insideUnitSphere*3f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.1f);
            obj.GetComponent<Collider>().enabled = true;

        }



    }

}
