using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Church.Common.Extensions;

namespace Church.Common.Settings
{
    public class AppSettingsProvider : ISettingsProvider
    {
        private static readonly Dictionary<Type, ISetting> Cache = new Dictionary<Type, ISetting>();

        public T GetSetting<T>() where T : ISetting
        {
            var cached = Cache.Get(typeof (T));

            if(cached != null)
            {
                return (T)cached;
            }

            var setting = Activator.CreateInstance<T>();
            var settingType = setting.GetType();
            var shortName = settingType.Name;

            foreach (var key in ConfigurationManager.AppSettings)
            {
                var sKey = (string)key;
                var split = sKey.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if(split[0] != shortName)
                {
                    continue;
                }

                //if split is 3, means it's a child object, not a value type. e.g. NotificationSetting.CustomerServiceEmailFrom.Address
                if(split.Length == 3)
                {
                    var property = settingType.GetProperty(split[1]);
                    
                    //create child object
                    if(property.GetValue(setting, null) == null)
                    {
                        property.SetValue(setting, Activator.CreateInstance(property.PropertyType), null);
                    }

                    //set child property of the child object.
                    var childProperty = property.PropertyType.GetProperty(split[2]);
                    childProperty.SetValue
                          (
                          property.GetValue(setting, null), 
                          TypeDescriptor.GetConverter(childProperty.PropertyType).ConvertFromInvariantString(ConfigurationManager.AppSettings[sKey]),
                          null
                          );

                }

                if (split.Length == 2)
                {
                    var property = settingType.GetProperty(split[1]);

                    if (property != null)
                    {
                        property.SetValue
                            (
                            setting,
                            TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(ConfigurationManager.AppSettings[sKey]),
                            null
                            );
                    }
                }
            }

            Cache[typeof (T)] = setting;
            return setting;
        }
    }
}
