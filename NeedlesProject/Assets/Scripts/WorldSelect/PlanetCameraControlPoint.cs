using UnityEngine;

public class PlanetCameraControlPoint : MonoBehaviour
{
    [SerializeField]
    private Transform planet;

    [SerializeField]
    private Vector3   offset;

    private void Awake()
    {
        transform.position = planet.position + offset;
    }
}
