using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right
    }

    [SerializeField]
    Direction direction;

    [SerializeField]
    StageBasicInfoManager info;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if(direction == Direction.Left)
        {
            image.enabled = !info.IsFirstWorld;
        }

        if(direction == Direction.Right)
        {
            image.enabled = !info.IsLastWorld;
        }
    }
}
