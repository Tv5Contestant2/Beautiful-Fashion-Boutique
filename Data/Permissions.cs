using System.Collections.Generic;

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
                case "reports":
                    //_results.AddRange(new List<string>()
                    //{
                    // $"Permissions.{module}.View",
                    //});
                    break;

                case "customers":
                    _results.AddRange(new List<string>(){
                        $"Permissions.{module}.Delete",
                        $"Permissions.{module}.Edit",
                        $"Permissions.{module}.ArchivedCustomer",
                        $"Permissions.{module}.BlockCustomer",
                    });
                    break;

                case "roles":
                    _results.AddRange(new List<string>(){
                        $"Permissions.{module}.Create",
                         $"Permissions.{module}.Delete",
                        //$"Permissions.{module}.View",
                        $"Permissions.{module}.Edit",
                       $"Permissions.{module}.ManagePermission",
                    });
                    break;

                case "orders":
                    _results.AddRange(new List<string>(){
                        //$"Permissions.{module}.Create",
                        // $"Permissions.{module}.Delete",
                        //$"Permissions.{module}.View",
                        $"Permissions.{module}.Edit",
                    });
                    break;

                case "returns":
                case "aboutus":
                case "contactus":
                    _results.AddRange(new List<string>()
                    {
                     //$"Permissions.{module}.View",
                       $"Permissions.{module}.Edit",
                    });
                    break;

                default:
                    _results.AddRange(new List<string>(){
                        $"Permissions.{module}.Create",
                        //$"Permissions.{module}.View",
                        $"Permissions.{module}.Edit",
                        $"Permissions.{module}.Delete",
                    });
                    break;
            }

            return _results;
        }

        public static List<string> GeneratePermissionsViewForModule(string module)
        {
            List<string> _results = new List<string>();
            _results.AddRange(new List<string>() { $"Permissions.{module}.View" });
            return _results;
        }

        public static class AboutUs
        {
            public const string Edit = "Permissions.AboutUs.Edit";
            //public const string View = "Permissions.AboutUs.View";
        }

        public static class ContactUs
        {
            public const string Edit = "Permissions.ContactUs.Edit";
            //public const string View = "Permissions.ContactUs.View";
        }

        public static class Customers
        {
            public const string ArchivedCustomer = "Permissions.Customers.ArchivedCustomer";
            public const string BlockCustomer = "Permissions.Customers.BlockCustomer";
            public const string Edit = "Permissions.Customers.Edit";
            public const string Delete = "Permissions.Customers.Delete";
        }

        public static class Dashboard
        {
            //public const string View = "Permissions.Dashboard.View";
        }

        public static class Employees
        {
            public const string Create = "Permissions.Employees.Create";
            public const string Delete = "Permissions.Employees.Delete";
            public const string Edit = "Permissions.Employees.Edit";
            //public const string View = "Permissions.Employees.View";
        }

        public static class Orders
        {
            public const string Edit = "Permissions.Orders.Edit";
            //public const string View = "Permissions.Orders.View";
        }

        public static class Products
        {
            public const string Create = "Permissions.Products.Create";
            public const string Delete = "Permissions.Products.Delete";
            public const string Edit = "Permissions.Products.Edit";
            //public const string View = "Permissions.Products.View";
        }

        public static class PromosAndDiscounts
        {
            public const string Create = "Permissions.PromosAndDiscounts.Create";
            public const string Delete = "Permissions.PromosAndDiscounts.Delete";
            public const string Edit = "Permissions.PromosAndDiscounts.Edit";
            //public const string View = "Permissions.PromosAndDiscounts.View";
        }

        public static class Reports
        {
            //public const string View = "Permissions.Reports.View";
        }

        public static class Returns
        {
            public const string Edit = "Permissions.Returns.Edit";
            //public const string View = "Permissions.Returns.View";
        }

        public static class Roles
        {
            public const string Create = "Permissions.Roles.Create";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Edit = "Permissions.Roles.Edit";

            //public const string View = "Permissions.Roles.View";
            public const string ManagePermission = "Permissions.Roles.ManagePermission";
        }
    }
}