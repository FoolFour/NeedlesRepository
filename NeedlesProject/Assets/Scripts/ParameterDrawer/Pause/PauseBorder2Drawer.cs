public class PauseBorder2Drawer : PauseDataDrawer
{
    private void Start()
    {
        parameterType = ParameterType.Time;
    }

    protected override object GetData()
	{
		return data.border2;
	}
}
