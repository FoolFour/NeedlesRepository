public class PauseBorder1Drawer : PauseDataDrawer
{
    private void Start()
    {
        parameterType = ParameterType.Time;
    }

    protected override object GetData()
	{
		return data.border1;
	}
}
