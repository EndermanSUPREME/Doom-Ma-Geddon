namespace SysView
{
    using Microsoft.Win32;

    // This class is used as a central HUB for collecting system information
    public class SystemData
    {
        public SystemData(){}

        // Reviews the windows registry to see the theme the system is using (light/dark)
        public bool UsingLightTheme()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("AppsUseLightTheme");
                        if (value is int themeValue)
                        {
                            return themeValue == 1;
                        }
                    }
                }
            } catch (System.Exception)
                {
                    // Default Fallback
                    return true;
                }

            return true;
        }
    }
}