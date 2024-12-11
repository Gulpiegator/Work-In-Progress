using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class OneWay : MonoBehaviour
{
    public Vector3 passDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;
    private new BoxCollider collider = null;
    private BoxCollider collisionCheck = null;
    [SerializeField] private Vector3 collisionCheckScale;
    // Start is called before the first frame update
    void Awake(){
        collider = GetComponent<BoxCollider>(); //Grab collider
        collider.isTrigger = false; //Make sure it is not a trigger
        //Initialize a trigger that will be slightly larger than the actual collider
        collisionCheck = gameObject.AddComponent<BoxCollider>();
        collisionCheck.size = collider.size + collisionCheckScale;
        collisionCheck.center = collider.center;
        collisionCheck.isTrigger = true;
    }

    private void OnTriggerStay(Collider other){
        if(Physics.ComputePenetration(collisionCheck, transform.position, 
        transform.rotation, other, other.transform.position, other.transform.rotation, 
        out Vector3 direction, out float penetrationDepth)){
            float dot = Vector3.Dot(passDirection, direction);
            if(Vector3.Dot(passDirection, direction)<0)
                Physics.IgnoreCollision(collider, other, false);
            else
                Physics.IgnoreCollision(collider, other);
        }
    }

    private void OnDrawGizmosSelected(){
        //Decide direction based on local v global
        Vector3 direction;
        if(localDirection)
            direction = transform.TransformDirection(passDirection.normalized);
        else
            direction = passDirection;
        //Cast red ray in unpassable direction
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction);
        //Cast green ray in passable direction
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -direction);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
