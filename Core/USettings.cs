using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Edit.Core
{
    /// <summary>
    /// 应用程序设置.
    /// </summary>
    public class USettings
    {
        
        StorageFolder folder;
        string displayName;

        
        public StorageFolder Folder => folder;
        public string DisplayName => displayName;

        public async void LoadAsync()
        {

        }
    }
    /// <summary>
    /// 应用设置属性.
    /// </summary>
    public sealed class USettingsProperty
    {
        USettings uSettings;
        string key;

        public USettings USettings => uSettings;
        public string Key => key;
        public object Value { get; set; }
    }
    public sealed class USettingsProperty<TValue>
    {
        USettings uSettings;
        string key;

        public USettings USettings => uSettings;
        public string Key => key;
        public TValue Value { get; set; }
    }
}
