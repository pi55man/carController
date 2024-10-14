using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptWheel : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;

    private float maxLength;
    private float minLength;
    private float lastLength;
    private float springVelocity;
    private float springLength;
    private float springForce;
    private float damperForce;
    private float wheelAngle;

    private float Fx;
    private float Fy;

    private Vector3 suspensionForce;
    private Vector3 wheelVelocityLS;
    
    [Header("Wheel")]
    public float wheelRadius;
    public float steerAngle;
    public float steerTime;

    [Header("Car")]
    public float carDrag;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        rb.drag = carDrag;

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    
    }

    void Update(){
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + wheelAngle, transform.localRotation.z);

        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.green);
    }   

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, wheelRadius+maxLength)){
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength,minLength,maxLength);

            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;

            springForce = springStiffness * (restLength-springLength);
            damperForce = damperStiffness * springVelocity;
            
            suspensionForce = (springForce+damperForce) * transform.up;            
            wheelVelocityLS  = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            Fx = (Input.GetAxis("Vertical") * springForce)*0.5f ;
            Fy = (wheelVelocityLS.x * springForce) * 0.5f  ;


            rb.AddForceAtPosition(suspensionForce + (Fx*transform.forward) + (Fy*-transform.right),hit.point);
        }
    }
}
