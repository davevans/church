namespace Church.Common.Settings
{
    public interface ISettingsProvider
    {
        T GetSetting<T>() where T : ISetting;
    }
}
