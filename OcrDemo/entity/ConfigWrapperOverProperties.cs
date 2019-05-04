using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrDemo.entity
{
    /// <summary>
    /// Implements a live wrapper over Properties.Settings.
    /// Why is this benefecial? We can inject a standardized IConfiguration into the plugins and this will give the 
    /// plugins a "live view" of the settings. Any inflight changes will be visible to the plugins without the need for restart
    /// </summary>
    public class ConfigWrapperOverProperties : IConfiguration
    {
        public string this[string key]
        {
            get
            {
                return $"{Properties.Settings.Default[key]}";
            }
            set => throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
