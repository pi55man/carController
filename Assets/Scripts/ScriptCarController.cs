using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{   public ScriptWheel[] wheels;

    [Header("car specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;

    [Header("inputs")]
    public float steerInput;
    
    public float ackermannLeft;
    public float ackermannRight; 

    // Update is called once per frame
    void Update()
    {   
        steerInput = Input.GetAxis("Horizontal");
        if (steerInput > 0){
            ackermannLeft =  Mathf.Rad2Deg * Mathf.Atan(wheelBase/(turnRadius + (rearTrack/2))) * steerInput;
            ackermannRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase/(turnRadius-(rearTrack/2))) * steerInput;
        } else if (steerInput < 0) {
            ackermannLeft =  Mathf.Rad2Deg * Mathf.Atan(wheelBase/(turnRadius - (rearTrack/2))) * steerInput;
            ackermannRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase/(turnRadius+ (rearTrack/2))) * steerInput;
        } else {
            ackermannLeft = 0;
            ackermannRight = 0;
            
        }   
        foreach(ScriptWheel w in wheels){
                if(w.wheelFrontLeft){
                    w.steerAngle = ackermannLeft;
                } 
                if(w.wheelFrontRight){
                    w.steerAngle = ackermannRight;
                }  
        }  
    }
}
