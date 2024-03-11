using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Node origin;
    private Node end;

    public Web web;

    public Web Web { get => web; set => web = value; }
    public Node Origin { get => origin; set => origin = value; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DrawLine(Vector3 endPosition)
    {
        this.transform.position = (endPosition + origin.transform.position) / 2;
        Vector2 distance = (endPosition - origin.transform.position);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, distance);
        this.transform.rotation = rotation;
        float scale = Mathf.Abs(distance.magnitude);
        this.transform.localScale = new Vector3(0.2f, scale, 1);
    }

    public void StartLine(Node origin)
    {
        this.origin = origin;
        origin.AddLine(this);
    }

    public void EndLine(Node end)
    {
        this.end = end;
        end.AddLine(this);
        DrawLine(end.transform.position);
    }

    public void OnMouseEnter()
    {
        if (web.GetActiveLine() == this) return;

        else if (web.GetActiveLine() == null && web.IsValid)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (web.IsValid)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Node breakPoint = Instantiate(origin);
            breakPoint.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            web.AddNode(breakPoint);

            // active line
            web.EndActiveLine(breakPoint);

            // separate broken line into two lines
            // breakpoint - end
            Line line = Instantiate(this);
            line.StartLine(breakPoint);
            line.EndLine(end);
            line.GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);    

            // origin - breakpoint
            end.RemoveLine(this);
            EndLine(breakPoint);

            // start new active line in breakpoint
            web.StartActiveLine(breakPoint);
        }
    }

    public void OnMouseExit()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void OnMouseDown()
    {
        if (web.GetActiveLine() == null && web.IsValid)
        {
            origin.RemoveLine(this);
            end.RemoveLine(this);
            Destroy(this.gameObject);
        }
    }
}
