using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private Transform target;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        if (target.position.x >= -655.5453f) {
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
        // } else transform.position = new Vector3(-655.5453f, transform.position.y, transform.position.z);
        } else transform.position = Vector3.Slerp(transform.position, new Vector3(-655.5453f, newPos.y, newPos.z), followSpeed*Time.deltaTime);
    }
}
