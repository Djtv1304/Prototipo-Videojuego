using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private InputManager inputManager;

    private Vector3 targetPosition;

    private bool isMoving;

    private bool isRolling;

    private bool isJumping;

    private bool isBlinking;

    public float laneDistance = 3.65f;

    private Animator animator;

    public GameObject[] renderableMeshes;

    // Start is called before the first frame update
    void Start()
    {

        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        isMoving = false;
        isRolling = false;
        isJumping = false;
        isBlinking = false;

    }

    private void SelectTargetPosition()
    {

        if (IsBusy())
        {

            return;

        }


        float horizontalMovement = inputManager.horizontalMovement.ReadValue<float>();
        float x = transform.position.x;

        if (horizontalMovement == 1 && x <= 0)
        {
 
            targetPosition = transform.position + Vector3.right * laneDistance;

            isMoving = true;

        }
        else if (horizontalMovement == -1 && x >= 0)
        {

            targetPosition = transform.position + Vector3.left * laneDistance;

            isMoving = true;

        }

    }

    private void MoveToTargetPosition()
    {

        if (!isMoving)
        {

            return;

        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);

        float distance = (targetPosition - transform.position).magnitude;

        if (distance < 0.01f)
        {

            transform.position = targetPosition;
            isMoving = false;

        }

    }

    private void CheckForRoll()
    {

        if(IsBusy())
        {

            return;

        }

        if (inputManager.roll.IsPressed())
        {

            animator.SetTrigger("Roll");
            isRolling = true;

        }

    }

    public void EndRoll()
    {

        isRolling = false;

    }

    private void CheckForJump()
    {

        if (IsBusy())
        {

            return;

        }

        if (inputManager.jump.IsPressed())
        {

            animator.SetTrigger("Jump");
            isJumping = true;

        }

    }

    public void EndJump()
    {

        isJumping = false;

    }

    private bool IsBusy()
    {

        return isMoving || isRolling || isJumping;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (isBlinking)
        {

            return;

        }

        if(other.tag == "SmallObstacle" && !isJumping)
        {

            animator.SetTrigger("Stumble");

            // Esta función se ejecuta en paralelo con el resto del código
            StartCoroutine(Blink());

            //Debug.Log("Game Over");

        }

        if (other.tag == "DoorObstacle" && !isRolling && !isBlinking)
        {

            animator.SetTrigger("Crash");

            // Detener el mapa
            Section[] sections = FindObjectsOfType<Section>();

            foreach (Section section in sections)
            {

                section.speed = 0;

            }

        }


    }

    private IEnumerator Blink()
    {

        isBlinking = true;

        for (int i = 0; i < 4; i++)
        {

            foreach (GameObject mesh in renderableMeshes)
            {

                mesh.SetActive(false);

            }

            yield return new WaitForSeconds(0.25f);

            foreach (GameObject mesh in renderableMeshes)
            {

                mesh.SetActive(true);

            }

            yield return new WaitForSeconds(0.25f);

        }

        isBlinking = false;

    }

    // Update is called once per frame
    void Update()
    {

        SelectTargetPosition();
        MoveToTargetPosition();
        CheckForRoll();
        CheckForJump();

    }
}
