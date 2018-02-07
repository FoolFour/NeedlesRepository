using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewRecordEffect : BaseMeshEffect
{
    [SerializeField]
    float speed;

    [SerializeField]
    float height;

    [SerializeField]
    float shift;

    static readonly int oneCharMesh = 6;

    List<UIVertex> stream = new List<UIVertex>();

    Text text;

    float startRadius;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponent<Text>();
        startRadius = 0;
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if(!IsActive()){ return; }
        
        vh.GetUIVertexStream(stream);

        Modify();

        vh.Clear();
        vh.AddUIVertexTriangleStream(stream);
    }

    private void Modify()
    {
        for(int i = 0; i < stream.Count; i += oneCharMesh)
        {
            float amount  = i / stream.Count;

            for(int j = 0; j < oneCharMesh; j++)
            {
                var element = stream[i+j];
                Vector3 position = element.position;
                position.y += Mathf.Sin((i * shift) + startRadius) * height;
                element.position = position;
                stream[i+j] = element;
            }
        }
    }

    private void Update()
    {
        startRadius += Time.deltaTime * speed;
        Mathf.Repeat(startRadius, 360.0f);
        text.SetAllDirty();
    }
}
