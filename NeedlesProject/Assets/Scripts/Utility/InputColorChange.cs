using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class InputColorChange : MonoBehaviour
{
    enum State
    {
        Up,
        Down
    }

    [SerializeField]
    string InputButtonName;

    [SerializeField]
    Color  buttonUpColor;

    [SerializeField]
    Color  buttonDownColor;

    [SerializeField]
    float  speed;

    Graphic graphic;

    float amount = 0.0f;

    State state;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    private void Start()
    {
        state = State.Up;
    }

    private void Update()
    {
        if (Input.GetButton(InputButtonName))
        {
            if(state == State.Up)
            {
                state = State.Down;
                StopAllCoroutines();
                StartCoroutine(UpToDown());
            }
        }
        else
        {
            if(state == State.Down)
            {
                state = State.Up;
                StopAllCoroutines();
                StartCoroutine(DownToUp());
            }
        }

        graphic.color = Color.Lerp(buttonUpColor, buttonDownColor, amount);
    }

    private IEnumerator UpToDown()
    {
        amount = Mathf.Clamp01(amount);
        while(amount < 1.0f)
        {
            amount += Time.deltaTime * speed;
            yield return null;
        }

        amount = 1.0f;
    }

    private IEnumerator DownToUp()
    {
        amount = Mathf.Clamp01(amount);
        while(amount > 0.0f)
        {
            amount -= Time.deltaTime * speed;
            yield return null;
        }

        amount = 0.0f;
    }
}
