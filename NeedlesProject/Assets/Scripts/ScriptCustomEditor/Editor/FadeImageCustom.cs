using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FadeImage))]
public class FadeImageCustom : Editor
{
    public override void OnInspectorGUI()
    {
        FadeImage image = target as FadeImage;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Source Image");
            image.sprite = EditorGUILayout.ObjectField(image.sprite, typeof(Sprite), true) as Sprite;
        }
        EditorGUILayout.EndHorizontal();

        image.color         = EditorGUILayout.ColorField("Color", image.color);

        image.material      = EditorGUILayout.ObjectField("Material", image.material, typeof(Material), true) as Material;

        image.raycastTarget = EditorGUILayout.Toggle("Raycast Target", image.raycastTarget);

        image.fadeSpeed     = EditorGUILayout.FloatField("Fade Speed", image.fadeSpeed);
    }
}
