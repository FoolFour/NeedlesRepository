using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BlockTiling : MonoBehaviour
{
	[SerializeField]
	private Texture2D[] useTextures;

	private Sprite[]    sprites;

	private Vector2     screenSize;

	private const float blockSize = 144;

	private void Awake()
    {
		var parentRect = transform.parent.GetComponent<RectTransform>();
		screenSize = parentRect.sizeDelta;
    }

    private void Start()
    {
		//for()
		sprites = new Sprite[useTextures.Length];
		sprites[0] = Sprite.Create(useTextures[0], new Rect(0, 0, useTextures[0].width, useTextures[0].height), Vector2.zero);

		Vector3 position   = new Vector3();
		Vector3 scale      = Vector3.one;
		Vector2 left_up    = new Vector2(0, 1);
		Vector2 size_delta = new Vector2(blockSize, blockSize);

		for(int i_x = 0; i_x < screenSize.x / blockSize; i_x++)
		{
			for(int j_y = 0; j_y < screenSize.y / blockSize; j_y++)
			{
				RectTransform child = new GameObject().AddComponent<RectTransform>();
				child.parent        = transform;
				child.anchorMax     = left_up;
				child.anchorMin     = left_up;
				child.pivot         = left_up;

				position.x = i_x *  blockSize;
				position.y = j_y * -blockSize;

				child.localPosition = position;
				child.sizeDelta     = size_delta;
				child.localScale    = scale;

				var image = child.gameObject.AddComponent<Image>();
				image.sprite = sprites[0];
			}
		}
    }

    private void Update()
    {
		
    }
}
