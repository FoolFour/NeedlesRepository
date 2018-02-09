using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoalSend : BlockBase
{
    private GameObject m_VideoObject;

    [SerializeField]
    public Player player;

    public SceneChanger sceneChanger;
    private RectTransform m_ClearImage;

    public string m_Scene = "none";

    public void Start()
    {
        m_VideoObject = GameObject.Find("VideoImage");
        m_ClearImage = GameObject.Find("m_ClearImage").GetComponent<RectTransform>();
        player = FindObjectOfType<Player>();
    }

    private void Reset()
    {
        player = FindObjectOfType<Player>();
    }

    public override void StickStay(GameObject arm, GameObject stickpoint)
    {
        Destroy(m_VideoObject);

        stickpoint.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        player = GameManagers.Instance.PlayerManager.GetPlayer().GetComponent<Player>();
        player.Goal();

        StartCoroutine(DelaySceneChange(1.0f));

        GetComponent<BoxCollider>().isTrigger = true;
        GameManagers.Instance.GameStateManager.StateChange(GameState.End);
    }

    IEnumerator DelaySceneChange(float second)
    {
        if (m_ClearImage)
        {
            {
                float t = 0;
                var scalefrom = m_ClearImage.localScale;
                var scaleto = new Vector3(1f, 1f, 1.0f);
                while (t <= 1)
                {
                    t += 0.05f;
                    m_ClearImage.localScale = Vector3.Lerp(scalefrom, scaleto, Mathf.SmoothStep(0, 1, t));
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        yield return new WaitForSeconds(second);
        sceneChanger.SceneChange(m_Scene);
    }

    //public void SceneChange()
    //{
    //    sceneChanger.SceneChangeAsync("Result", LoadSceneMode.Additive);
    //}
}
