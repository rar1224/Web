using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Web web;

    private List<Line> lines = new List<Line>();
    private bool isEssential = false;

    public Web Web { get => web; set => web = value; }
    public bool IsEssential { get => isEssential; set => isEssential = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lines.Count == 0 && !isEssential)
        {
            web.RemoveNode(this);
            Destroy(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        web.StartActiveLine(this);
    }

    public void AddLine(Line line)
    {
        lines.Add(line);
    }

    public void RemoveLine(Line line) { 
        lines.Remove(line);
    }
}
