using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickMovement : MonoBehaviour
{
    public Vector2 targetPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.y = transform.position.y;

        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
    }
}
