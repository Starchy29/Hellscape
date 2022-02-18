using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private int left;
    [SerializeField] private int right;
    [SerializeField] private int down;
    [SerializeField] private int up;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, transform.localPosition.z);

        if(transform.localPosition.x < left)
        {
            transform.localPosition = new Vector3(left, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.x > right)
        {
            transform.localPosition = new Vector3(right, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.y < down)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, down, transform.localPosition.z);
        }
        if (transform.localPosition.y > up)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, up, transform.localPosition.z);
        }
    }
}
