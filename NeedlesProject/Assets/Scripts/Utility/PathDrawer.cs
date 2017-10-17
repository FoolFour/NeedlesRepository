using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class PathDrawer : MonoBehaviour
{
    //////////////////////////
    // 変数(SerializeField) /
    ////////////////////////
    [SerializeField, Tooltip("値が大きくなるほど滑らかに")]
    [Range(1, 20)]
    private int              smoothness;

    [SerializeField]
    [Tooltip("使用するマテリアル \n" +
             "既にLineRendererがある場合、この設定は適用されない")]
    private Material         lineMaterial;

    [SerializeField]
    [Tooltip("線の太さ \n" +
             "既にLineRendererがある場合、この設定は適用されない")]
    private float            lineStartWidth;

    [SerializeField]
    [Tooltip("線の太さ \n" +
             "既にLineRendererがある場合、この設定は適用されない")]
    private float            lineEndWidth;

    [SerializeField]
     [Tooltip("線の色 \n" +
             "既にLineRendererがある場合、この設定は適用されない")]
    private Gradient         lineGradient;


    /////////////////////////////
    // 変数(NonSerializeField) /
    ///////////////////////////
    private LineRenderer     lineRenderer;
    private CatmullRomSpline spline;

    //////////////////
    // 関数(public) /
    ////////////////

    public void SetPath(params Vector3[] path)
    {
        lineRenderer.positionCount = path.Length;

        if (smoothness == 1)
        {
            lineRenderer.SetPositions(path);
        }
        else
        {
            CalcLinePosition(path);
        }
    }

    ///////////////////
    // 関数(private) /
    /////////////////
    private void Awake()
    {
        spline = new CatmullRomSpline();

        //カスタムしたLineRendererを使う場合
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            InitLineRenderer();
        }
    }

    private void InitLineRenderer()
    {
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.receiveShadows    = false;
        lineRenderer.material          = lineMaterial;
        lineRenderer.startWidth        = lineStartWidth;
        lineRenderer.endWidth          = lineEndWidth;
        lineRenderer.colorGradient     = lineGradient;
    }

    private void CalcLinePosition(params Vector3[] path)
    {
        Debug.Log("パスの計算開始");
        spline.AddPath(path);
        int maxPosition = (path.Length-1) * smoothness;

        //+1しないと終点の点を含んでくれない
        lineRenderer.positionCount = maxPosition+1;

        Debug.Log("座標の数 : " + (maxPosition+1));

        int positionCount = 0;
        for (float i = 0.0f; i <= path.Length+float.Epsilon; i += 1.0f / smoothness)
        {
            if(positionCount >= maxPosition) { break; }

            Vector3 position = spline.FetchPosition(i);
            lineRenderer.SetPosition(positionCount, position);
            positionCount++;
        }

        //終点の点が入っていないのでここでadd
        lineRenderer.SetPosition(maxPosition, spline.EndPoint);

        Debug.Log("パスの計算終了");
    }
}
