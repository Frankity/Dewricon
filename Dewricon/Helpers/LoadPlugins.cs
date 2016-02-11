using System;
using System.IO;
using DewPlugins;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

namespace Dewricon.Helpers
{
    public partial class LoadPlugins
    {
        public static ICollection<DewPlugins.DewPlugins> LoadPlugin(string path) 
        {
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllfile in dllFileNames)
                {
                    AssemblyName aN = AssemblyName.GetAssemblyName(dllfile);
                    Assembly assembly = Assembly.Load(aN);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(DewPlugins.DewPlugins);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                                continue;
                            else
                                if (type.GetInterface(pluginType.FullName) != null)
                                    pluginTypes.Add(type);
                        }
                    }
                }
                ICollection<DewPlugins.DewPlugins> plugins = new List<DewPlugins.DewPlugins>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    DewPlugins.DewPlugins plugin = (DewPlugins.DewPlugins)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }
                return plugins;
            }
            return null;
        }
    }
}
