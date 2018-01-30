public class PauseNowTimeDrawer : PauseDataDrawer
{
    private void Start()
    {
        parameterType = ParameterType.Time;
    }

    protected override object GetData()
    {
        return data.time;
    }
}
