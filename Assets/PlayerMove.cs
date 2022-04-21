using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static event System.Action OnReachedEndOfLevel;
    [SerializeField] private float moveSpeed = 5;

    [SerializeField] private float smoothMoveTime = .5f;
    [SerializeField] private float turnSpeed = 8;
    private bool disabled;
    private float angle;
    private float smoothInputMagnitude;
    private float smoothMoveVelocity;
    private Vector3 velocity;

    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottenPlayer += Disable;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("gameIsOgfdsgdgdfgve432424242r");

            Disable();
            if (OnReachedEndOfLevel != null)
            {
                OnReachedEndOfLevel();
                Debug.Log("gameIsOgfdsgdgdgsdgsdgdsggsdgdsgsgsgsgsgsgfgver");

            }
        }
    }
    

    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        if (!disabled)
        {
         inputDirection=   new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))
                .normalized;
        }

        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude,
            ref smoothMoveVelocity, smoothMoveTime);
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
        

    }

    void Disable()
    {
        disabled = true;
    }

    private void FixedUpdate()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(Vector3.up *angle));
        _rigidbody.MovePosition(_rigidbody.position + velocity * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottenPlayer -= Disable;
    }
}
