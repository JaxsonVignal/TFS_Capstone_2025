using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class PlayerAttack : MonoBehaviour
{

    //commented headers due to error: 
    //Attribute 'Header' is not valid on this declaration type. It is only valid on 'field' declarations.
    //[Header ("Keybinds")]
    [SerializeField] private KeyCode attackKey = KeyCode.LeftAlt;
    
    //[Header ("Animation")]
    [SerializeField] private Animator anim;
        
    //[Header("Attack Settings")]
    //attack speed
    //attack interval/reset/cooldown

    private bool isAttacking;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
    }

    private void PlayerInput() //handles input and state, may wish to separate state in the future
    {
        if (Input.GetKeyDown(attackKey) || Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
            Debug.Log("attack");
        }
        else //TODO: update this so it transitions at the end rather than every frame
        {
            //attackState = AttackStates.idle;
        }
    }
}
