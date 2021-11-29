using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVectorContainerValueByMousePos : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Vector2IntContainer vectorContainer;

    // Update is called once per frame
    void Update()
    {
        Vector3 unitPos = unit.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < unitPos.x && mousePos.y < unitPos.y)
        {
            vectorContainer.Content = Vector2Int.left;
        }

        if (mousePos.x > unitPos.x && mousePos.y < unitPos.y)
        {
            vectorContainer.Content = Vector2Int.down;
        }

        if (mousePos.x < unitPos.x && mousePos.y > unitPos.y)
        {
            vectorContainer.Content = Vector2Int.up;
        }

        if (mousePos.x > unitPos.x && mousePos.y > unitPos.y)
        {
            vectorContainer.Content = Vector2Int.right;
        }

    }
}
