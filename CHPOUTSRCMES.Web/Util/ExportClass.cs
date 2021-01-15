using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace CHPOUTSRCMES.Web.Util
{
    public static class ExportClass
    {
        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        public static void ShowMethods()
        {
            using (StreamWriter sw = new StreamWriter("D:/methods.txt"))
            {
                Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "CHPOUTSRCMES.Web.DataModel.UnitOfWorks");

                foreach (var type in typelist)
                {
                    sw.WriteLine(type.Name);
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        var parameters = method.GetParameters();
                        var parameterDescriptions = string.Join
                            (", ", method.GetParameters()
                                         .Select(x => x.ParameterType + " " + x.Name)
                                         .ToArray());
                        sw.WriteLine("{0} {1} ({2})",
                                         method.ReturnType,
                                         method.Name,
                                         parameterDescriptions);
                    }
                }
            }
        }
    }
}


