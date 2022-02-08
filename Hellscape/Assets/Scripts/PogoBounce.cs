using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogoBounce : MonoBehaviour
{
    private float timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Updates the timer each frame
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///  Determine how long the trigger will exist
    /// </summary>
    public void SetTimer(float duration)
    {
        timer = duration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            GetComponentInParent<PlayerMovementTest>().PogoBounce();
        }
    }
}
