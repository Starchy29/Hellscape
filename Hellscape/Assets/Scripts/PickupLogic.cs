using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLogic : MonoBehaviour
{
    [SerializeField] private bool addDoubleJump;
    [SerializeField] private bool addDash;
    [SerializeField] private bool addPogo;
    public bool AddDoubleJump {  get { return addDoubleJump; } }
    public bool AddDash {  get { return addDash; } }
    public bool AddPogo {  get { return addPogo; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player") {
            PlayerMovementTest script =  GameObject.Find("Player").GetComponent<PlayerMovementTest>();
            if(addDoubleJump) {
                script.hasDoubleJump = true;
            }
            if(addDash) {
                script.hasDash = true;
            }
            if(addPogo) {
                script.hasPogoBounce = true;
            }
            this.gameObject.SetActive(false);
        }
    }
}
