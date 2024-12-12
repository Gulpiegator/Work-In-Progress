using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //player components 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerBody;
    //player movement
    public float speed = 5;
    Vector2 move;
    //ground detection
    public float castDistance;
    public LayerMask groundLayer;
    //shooting fields
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate;
    private float shotCounter = 0;
    //current game stage
    public int stage = 0;

    public int playerHealth = 3;

    //invincibility
    public float invincibilityDuration = 2f;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    [SerializeField] private GameObject currentCheckpoint;

    public GameObject losePrefab;
    public GameObject winPrefab;
    void Start(){
        playerBody.freezeRotation = true;
    }

    void Update(){
        //directional input
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //avoid higher diagonal move speed
        if (inputDirection.magnitude > 1)
            inputDirection.Normalize();

        //check how player is allowed to move in this stage
        if(stage > 0)
            playerBody.velocity = new Vector3(inputDirection.x * speed, playerBody.velocity.y, playerBody.velocity.z);
        if(stage > 4)
            playerBody.velocity = new Vector3(playerBody.velocity.x, playerBody.velocity.y, inputDirection.z * speed);

        //jumping
        if(Input.GetKey(KeyCode.Space) && isGrounded() && stage > 1)
            playerBody.velocity = new Vector3(playerBody.velocity.x, speed*1.5f, playerBody.velocity.z);

        //move player between foreground and background
        if (Input.GetKeyDown(KeyCode.F) && stage == 4){
            if (playerTransform.position.z < 5)
                playerTransform.position += new Vector3(0, 0, 10);
            else if (playerTransform.position.z > 5)
                playerTransform.position -= new Vector3(0, 0, 10);
        } 
        if(stage > 2 && stage != 4)
        shoot();

        if (Input.GetKeyDown(KeyCode.R) && currentCheckpoint != null){
            RespawnAtCheckpoint();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }

    //check if player is on ground for jumping
    public bool isGrounded(){
        return Physics.Raycast(transform.position, -transform.up, castDistance, groundLayer);
    }

    private void shoot(){
        if (Input.GetMouseButton(0)){
            //check if we can shoot
            if (shotCounter + fireRate <= Time.time){
                //grab camera
                Camera mainCamera = Camera.main;

                //using the camera type and mouse position determine world position
                Vector3 mouseWorldPosition;
                if (mainCamera.orthographic){
                    mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        -mainCamera.transform.position.z
                    ));
                }
                else{
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit)){
                        mouseWorldPosition = hit.point;
                    }
                    else{
                        mouseWorldPosition = ray.GetPoint(10f);
                    }
                }

                //get direction for the bullet
                Vector3 fireDirection = (mouseWorldPosition - playerTransform.position).normalized;

                //lock axis based on stage
                if (stage >= 3 && stage <= 4){
                    fireDirection.z = 0;
                }
                else if (stage > 4){
                    fireDirection.y = 0;
                }

                //create bullet
                GameObject newProjectile = Instantiate(projectile, playerTransform.position, Quaternion.identity);

                //give bullet information
                BulletController bulletController = newProjectile.GetComponent<BulletController>();
                bulletController.SetDirection(fireDirection);

                shotCounter = Time.time;
            }
        }
    }

    private void OnCollisionEnter(Collision collision){
        // Check if collision is enemy
        if (!isInvincible && collision.gameObject.CompareTag("Enemy")){
            playerHealth -= 1;

            // Trigger invincibility and reset timer
            isInvincible = true;
            invincible();

            // Check if dead
            if (playerHealth <= 0){
                RespawnAtCheckpoint();
                playerHealth = 3;
                StartCoroutine(DisplayUICoroutine(losePrefab, 3));
            }
        }
    }

    private void invincible(){
        //grab renderer and turn opacity down
        Renderer playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null){
            Color color = playerRenderer.material.color;
            color.a = 0.75f;
            playerRenderer.material.color = color;
        }

        //turn of collision with enemies
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = gameObject.layer;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        //start timer for invincibility 
        StartCoroutine(ResetInvincibility(playerRenderer, enemyLayer, playerLayer));
    }

    private IEnumerator ResetInvincibility(Renderer playerRenderer, int enemyLayer, int playerLayer){
        yield return new WaitForSeconds(invincibilityDuration);

        //reset opacity
        if (playerRenderer != null){
            Color color = playerRenderer.material.color;
            color.a = 1f;
            playerRenderer.material.color = color;
        }

        //turn collision back on
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        isInvincible = false;
    }

    void OnTriggerEnter(Collider other){
        // Check if collision is with a checkpoint
        if (other.gameObject.CompareTag("Checkpoint")){
            currentCheckpoint = other.gameObject;
            Debug.Log("Checkpoint saved at: " + currentCheckpoint.transform.position);
            playerHealth = 3;
        }
        if (other.gameObject.CompareTag("Star")){
            StartCoroutine(DisplayUICoroutine(winPrefab, 10));
        }
    }

    private void RespawnAtCheckpoint(){
        transform.position = currentCheckpoint.transform.position;
    }

    private IEnumerator DisplayUICoroutine(GameObject canvasPrefab, float duration){
        //create canvas
        GameObject canvasInstance = Instantiate(canvasPrefab);
        //wait
        yield return new WaitForSeconds(duration);
        if(canvasPrefab == winPrefab)
            SceneManager.LoadScene("Menu");
        //remove
        Destroy(canvasInstance);
    }
}
