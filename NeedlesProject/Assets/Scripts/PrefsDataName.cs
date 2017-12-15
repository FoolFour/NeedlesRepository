/// <summary>PlayerPrefsで受け渡すデータの名前</summary>
public class PrefsDataName
{
    // ----- ----- ----- -----
    // ステージの情報
    // ----- ----- ----- -----
    public const string StageName = "StageName";
    public const string Scene     = "Scene";
    public const string NextSene  = "NextScene";
    public const string Border1   = "Border1";
    public const string Border2   = "Border2";
    public const string Time      = "Time";

    public static string StageTime(string stageName)
    {
        return stageName + "_" + Time;
    }

    public static string StageClearFrag(string stageName)
    {
        return stageName + "_" + "ClearFrag";
    }

    public static string Border1ClearFrag(string stageName)
    {
        return stageName + "_" + "B1ClearFrag";
    }

    public static string Border2ClearFrag(string stageName)
    {
        return stageName + "_" + "B2ClearFrag";
    }

    public const string isInit    = "isInit";

    // ----- ----- ----- -----
    // フェード関係
    // ----- ----- ----- -----
    public const string FadeStart = "FadeStart";
    public const string Fade_R    = "Fade_R";
    public const string Fade_G    = "Fade_G";
    public const string Fade_B    = "Fade_B";
}
