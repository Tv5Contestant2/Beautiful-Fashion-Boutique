using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            List<string> _results = new List<string>();

            switch (module.ToLower())
            {
                case "dashboard":
                    break;

                case "order":
                case "returns":
                case "aboutus":
                case "contactus":
                    break;

                default:
                    _results.AddRange(new List<string>(){
                        $"Permissions.{module}.Create",
                        $"Permissions.{module}.View",
                        $"Permissions.{module}.Edit",
                        $"Permissions.{module}.Delete",
                    });
                    break;
            }

            return _results;
        }
    }
}