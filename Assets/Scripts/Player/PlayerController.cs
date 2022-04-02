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
         

            Vector3 p1 = transform.position + characterController.center + Vector3.up * -characterController.height * 0.5F;
            Vector3 p2 = p1 + Vector3.up * characterController.height;
            Collider[] colliders = Physics.OverlapCapsule(p1 + transform.forward / 2f, p2 + transform.forward / 2f, 0.5f, 
                LayerMask.GetMask("Items", "Fireplace"), queryTriggerInteraction: QueryTriggerInteraction.Collide).ToArray();
            if (colliders.Any(c => c.gameObject.tag == "Fire") && item !=null)
                StartCoroutine(DropItemInFire(item, colliders.Where(c => c.gameObject.tag == "Fire").First().transform));
            else
            {
                if (item != null)
                    StartCoroutine(DropItem(item));


                if (colliders.Where(c => c.gameObject.tag == "Item").ToArray().Length > 0)
                    PickItem(colliders.Where(c => c.gameObject.tag == "Item").First().gameObject);
            }
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


    IEnumerator DropItem(Item obj)
    {
        if (item!= null)
        {
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
    IEnumerator DropItemInFire(Item obj, Transform fireplace)
    {
        if (item != null)
        {
            item = null;

            Debug.Log("DROP IN FIRE");
            Debug.Log(fireplace.name);
            Debug.Log(fireplace.GetComponentInChildren<Fireplace>());
            fireplace.GetComponentInChildren<Fireplace>().AddPower(obj.illumination);
            actionKeyUp = false;
            obj.gameObject.tag = "Untagged";
            obj.transform.parent = null;
            obj.transform.position = fireplace.position + Vector3.up * 2f;
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().AddExplosionForce(5f, obj.transform.position - Vector3.up, 5f);
            obj.GetComponent<Rigidbody>().AddTorque(new Vector3(2f, 2f, 2f) * 3f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.1f);
            obj.GetComponent<Collider>().enabled = true;

        }



    }
}
