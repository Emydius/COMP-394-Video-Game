using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject rockPrefab;
    public PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateObstacle", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateObstacle()
    {
        GameObject rock = Instantiate(rockPrefab, new Vector3(58, 26, 0), Quaternion.identity);
        FallenAcorn fallenAcornScript = rock.GetComponent<FallenAcorn>();
        fallenAcornScript.playerHit.AddListener(movement.ResetPlayer);
    }
}
