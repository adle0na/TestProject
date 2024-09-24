using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed = 10f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
        
        if (moveDirection != Vector3.zero)
        {
            _rigidbody.velocity = new Vector3(moveDirection.x * _moveSpeed,
                _rigidbody.velocity.y,
                moveDirection.z * _moveSpeed);
            
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, toRotation, _rotationSpeed * Time.deltaTime);

            _animator.SetInteger("animation", 20);
        }
        else
        {
            _animator.SetInteger("animation", 13);
        }
    }
}
