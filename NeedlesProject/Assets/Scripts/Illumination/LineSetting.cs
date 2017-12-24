using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSetting : MonoBehaviour
{

    LineRenderer m_lineRenderer;
    int index = 0;

    public void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetVertex(int index)
    {
        m_lineRenderer.SetVertexCount(index);
    }

    public void AddPoint(Vector3 point)
    {
        m_lineRenderer.SetPosition(index, point);
        index++;
    }

    public void Loop(bool loopfrag)
    {
        m_lineRenderer.loop = loopfrag;
    }

}
