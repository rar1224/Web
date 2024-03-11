using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public Node nodePrefab;
    public Web web;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseEnter()
    {
        if (web.GetActiveLine() != null)
        {
            Node node = Instantiate(nodePrefab);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 originPos = (Vector2)web.GetActiveLine().Origin.transform.position;
            Vector2 direction = mousePos - originPos;

            node.transform.position = mousePos + direction * 5;
            web.AddNode(node);
            web.EndActiveLine(node);
        }
    }
}
