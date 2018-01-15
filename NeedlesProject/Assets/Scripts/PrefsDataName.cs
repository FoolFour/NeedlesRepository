/// <summary>PlayerPrefsで受け渡すデータの名前</summary>
public sealed class PrefsDataName
{
    // ----- ----- ----- -----
    // ステージの情報
    // ----- ----- ----- -----
    public static readonly string StageName     = "StageName";
    public static readonly string Scene         = "Scene";
    public static readonly string NextSene      = "NextScene";
    public static readonly string Border1       = "Border1";
    public static readonly string Border2       = "Border2";
    public static readonly string Time          = "Time";
    public static readonly string SelectedWorld = "SelectedWorld";

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

    public static readonly string isInit    = "isInit";

    // ----- ----- ----- -----
    // フェード関係
    // ----- ----- ----- -----
    public static readonly string FadeStart = "FadeStart";
}
