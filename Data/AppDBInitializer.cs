using ECommerce1.Data.Enums;
using ECommerce1.Data.Seeds;
using ECommerce1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data
{
    public class AppDBInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDBContext>();
                context.Database.EnsureCreated();

                var services = serviceScope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                if (roleManager.Roles.ToList().Count <= 0)
                {
                    var task = RolesSeeds.SeedRolesAsync(userManager, roleManager);
                    task.GetAwaiter().GetResult();
                }

                #region Colors

                if (!context.Colors.Any())
                {
                    context.Colors.AddRange(new List<Color>()
                    {
                        new Color()
                        {
                            Code = "#000000",
                            Title = "Black"
                        },
                        new Color()
                        {
                            Code = "#ff0000",
                            Title = "Red"
                        },
                        new Color()
                        {
                            Code = "#ffffff",
                            Title = "White"
                        },
                    }); ;

                    context.SaveChanges();
                }

                #endregion Colors

                #region Customers

                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(new List<Customers>()
                    {
                        new Customers()
                        {
                            CustomerFirstName = "Will",
                            CustomerLastName = "Smith",
                            Image = "/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QMraHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjI0ODBGNkIxRkVFQTExRTQ5RUU3RjVDODRBRTdDNkQ5IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjI0ODBGNkIyRkVFQTExRTQ5RUU3RjVDODRBRTdDNkQ5Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MjQ4MEY2QUZGRUVBMTFFNDlFRTdGNUM4NEFFN0M2RDkiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MjQ4MEY2QjBGRUVBMTFFNDlFRTdGNUM4NEFFN0M2RDkiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7gAOQWRvYmUAZMAAAAAB/9sAhAAGBAQEBQQGBQUGCQYFBgkLCAYGCAsMCgoLCgoMEAwMDAwMDBAMDg8QDw4MExMUFBMTHBsbGxwfHx8fHx8fHx8fAQcHBw0MDRgQEBgaFREVGh8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx//wAARCACWAJYDAREAAhEBAxEB/8QAbAABAAMBAQEAAAAAAAAAAAAAAAIEBQMBCAEBAAAAAAAAAAAAAAAAAAAAABABAAIBAgMECQQDAAAAAAAAAAECAxEEIUFxMVESBWGRobHBIjJyE1IzFDSBQiQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/APpkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFPd72cc/jx/VHbbuBT/lbjxa/ktr14eoHfH5lkjhkrFo744SCzTf7a3Oa9YBYiYmNYnWJ7JgAAAAAAAAAAAAAEM2T8eK1/wBMcOoMWZmZmZ4zPaAAAC1sdzOPJFLT8luHSQaYAAAAAAAAAAAAK3mNtNvp32iPiDLAAAABtYb+PFS3OYiZBMAAAAAAAAAAAFTzP9iv3R7pBmgAAAA19n/Wx9PiDsAAAAAAAAAAACp5l+xX7o90gzQAAAAa+z/rY+nxB2AAAAAAAAAAABV8yiZwRPdaNfVIMwAAAAGxtI022PoDqAAAAAAAAAAACGfH+TFanfHDryBizExMxPCY7QAAAe1rNrRWOMzOkA26VitK1jsrER6gegAAAAAAAAAAAAz/ADDb+G35ax8tvq6gpAAA0dls5pMZcn1f6x3AuAAAAAAAAAAAAAA4b6P+W/8Aj3wDJAABuUnWsT6IB6AAAAAAAAAAAAADhvrRG2t6dIj1gyQAAbG1yRfBSecRpPWOAOoAAAAAAAAAAAAOd9zgp9V46Rxn2Azt3upzWiI4Ur2R8QVwAAd9rupwWmJ40ntj4g0abrBeOF46Twn2g69oAAAAAAAAIZc2PFGt7ad0cwUsvmV54Yo8Md88ZBVvlyX+u026ggAAAAAACVMuSk60tNegLeLzK8cMseKO+OEgu4suPLXxUnWOYJgAAAAqbrfRjmaY+N+c8oBnWva9ptadZnnIPAAAAAAAAAAASpkvS3ipOkwDT2u8rm+W3DJHLlPQFgAAEct/Bjtf9MTIMWZmZmZ4zPaDwAAAAAAAAAAAAE8F/BmpbumNenMG0AACvv7aba3pmI9oMoAAAAAAAAAAAAAAG3it4sVLd8RPsBIAFXzL+vH3R7pBmAAAAAAAAAAAAAAA2Npr/Gx69wOoP//Z"
                }
                    }); ;

                    context.SaveChanges();
                }

                #endregion Customers

                #region Delivery Method

                if (!context.DeliveryMethods.Any())
                {
                    context.DeliveryMethods.AddRange(new List<DeliveryMethod>()
                    {
                        new DeliveryMethod()
                        {
                            Title = "Delivery"
                        },
                        new DeliveryMethod()
                        {
                            Title = "Store Pick-up"
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Delivery Method

                #region Delivery Status

                if (!context.DeliveryStatuses.Any())
                {
                    context.DeliveryStatuses.AddRange(new List<DeliveryStatus>()
                    {
                        new DeliveryStatus()
                        {
                            Title = "Pending",
                            Class = "bg-warning"
                        },
                        new DeliveryStatus()
                        {
                            Title = "Shipped",
                            Class = "bg-info"
                        },
                        new DeliveryStatus()
                        {
                            Title = "Received",
                            Class = "bg-success"
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Delivery Status

                #region Employees

                if (!context.Employees.Any())
                {
                    context.Employees.AddRange(new List<Employees>()
                    {
                        new Employees()
                        {
                            EmployeeFirstName = "Beautiful",
                            EmployeeLastName = "Administrator",
                            Image = "/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QMraHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjI0ODBGNkIxRkVFQTExRTQ5RUU3RjVDODRBRTdDNkQ5IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjI0ODBGNkIyRkVFQTExRTQ5RUU3RjVDODRBRTdDNkQ5Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MjQ4MEY2QUZGRUVBMTFFNDlFRTdGNUM4NEFFN0M2RDkiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MjQ4MEY2QjBGRUVBMTFFNDlFRTdGNUM4NEFFN0M2RDkiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7gAOQWRvYmUAZMAAAAAB/9sAhAAGBAQEBQQGBQUGCQYFBgkLCAYGCAsMCgoLCgoMEAwMDAwMDBAMDg8QDw4MExMUFBMTHBsbGxwfHx8fHx8fHx8fAQcHBw0MDRgQEBgaFREVGh8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx//wAARCACWAJYDAREAAhEBAxEB/8QAbAABAAMBAQEAAAAAAAAAAAAAAAIEBQMBCAEBAAAAAAAAAAAAAAAAAAAAABABAAIBAgMECQQDAAAAAAAAAAECAxEEIUFxMVESBWGRobHBIjJyE1IzFDSBQiQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/APpkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFPd72cc/jx/VHbbuBT/lbjxa/ktr14eoHfH5lkjhkrFo744SCzTf7a3Oa9YBYiYmNYnWJ7JgAAAAAAAAAAAAAEM2T8eK1/wBMcOoMWZmZmZ4zPaAAAC1sdzOPJFLT8luHSQaYAAAAAAAAAAAAK3mNtNvp32iPiDLAAAABtYb+PFS3OYiZBMAAAAAAAAAAAFTzP9iv3R7pBmgAAAA19n/Wx9PiDsAAAAAAAAAAACp5l+xX7o90gzQAAAAa+z/rY+nxB2AAAAAAAAAAABV8yiZwRPdaNfVIMwAAAAGxtI022PoDqAAAAAAAAAAACGfH+TFanfHDryBizExMxPCY7QAAAe1rNrRWOMzOkA26VitK1jsrER6gegAAAAAAAAAAAAz/ADDb+G35ax8tvq6gpAAA0dls5pMZcn1f6x3AuAAAAAAAAAAAAAA4b6P+W/8Aj3wDJAABuUnWsT6IB6AAAAAAAAAAAAADhvrRG2t6dIj1gyQAAbG1yRfBSecRpPWOAOoAAAAAAAAAAAAOd9zgp9V46Rxn2Azt3upzWiI4Ur2R8QVwAAd9rupwWmJ40ntj4g0abrBeOF46Twn2g69oAAAAAAAAIZc2PFGt7ad0cwUsvmV54Yo8Md88ZBVvlyX+u026ggAAAAAACVMuSk60tNegLeLzK8cMseKO+OEgu4suPLXxUnWOYJgAAAAqbrfRjmaY+N+c8oBnWva9ptadZnnIPAAAAAAAAAAASpkvS3ipOkwDT2u8rm+W3DJHLlPQFgAAEct/Bjtf9MTIMWZmZmZ4zPaDwAAAAAAAAAAAAE8F/BmpbumNenMG0AACvv7aba3pmI9oMoAAAAAAAAAAAAAAG3it4sVLd8RPsBIAFXzL+vH3R7pBmAAAAAAAAAAAAAAA2Npr/Gx69wOoP//Z"
                }
                    }); ;

                    context.SaveChanges();
                }

                #endregion Employees

                #region Gender

                if (!context.Genders.Any())
                {
                    context.Genders.AddRange(new List<Gender>()
                    {
                        new Gender()
                        {
                            Title = "Women",
                            AlternateTitle = "Female"
                        },
                        new Gender()
                        {
                            Title = "Men",
                            AlternateTitle = "Male"
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Gender

                #region Modules

                if (!context.Modules.Any())
                {
                    context.Modules.AddRange(new List<Modules>()
                    {
                        new Modules()
                        {
                            Title = "Customer Management",
                        },
                        new Modules()
                        {
                            Title = "Employee Management",
                        },
                        new Modules()
                        {
                            Title = "Roles Management",
                        },
                        new Modules()
                        {
                            Title = "Order Management",
                        },
                        new Modules()
                        {
                            Title = "Returns Management",
                        },
                        new Modules()
                        {
                            Title = "Product Management",
                        },
                        new Modules()
                        {
                            Title = "Promos and Discounts",
                        },
                        new Modules()
                        {
                            Title = "Site Configurations",
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Modules

                #region Payment Method

                if (!context.PaymentMethods.Any())
                {
                    context.PaymentMethods.AddRange(new List<PaymentMethod>()
                    {
                        new PaymentMethod()
                        {
                            Title = "Cash On Delivery"
                        },
                        new PaymentMethod()
                        {
                            Title = "Pay Maya"
                        },
                        new PaymentMethod()
                        {
                            Title = "GCash"
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Payment Method

                #region Product Category

                if (!context.ProductCategories.Any())
                {
                    context.ProductCategories.AddRange(new List<ProductCategory>()
                    {
                        new ProductCategory()
                        {
                            Title = "Shoes"
                        },
                        new ProductCategory()
                        {
                            Title = "Clothing"
                        },
                        new ProductCategory()
                        {
                            Title = "Accessories"
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Product Category

                #region Sizes

                if (!context.Sizes.Any())
                {
                    context.Sizes.AddRange(new List<Size>()
                    {
                        // Clothing
                        new Size()
                        {
                            Code = "XS",
                            Title = "X-Small",
                            CategoryId = (int)ProductCategoryEnum.Clothing
                        },
                        new Size()
                        {
                            Code = "S",
                            Title = "Small",
                            CategoryId = (int)ProductCategoryEnum.Clothing
                        },
                        new Size()
                        {
                            Code = "M",
                            Title = "Medium",
                            CategoryId = (int)ProductCategoryEnum.Clothing
                        },
                        new Size()
                        {
                            Code = "L",
                            Title = "Large",
                            CategoryId = (int)ProductCategoryEnum.Clothing
                        },
                        new Size()
                        {
                            Code = "XL",
                            Title = "X-Large",
                            CategoryId = (int)ProductCategoryEnum.Clothing
                        },

                        // Shoes
                        new Size()
                        {
                            Code = "6",
                            Title = "6",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                        new Size()
                        {
                            Code = "6.5",
                            Title = "6.5",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                        new Size()
                        {
                            Code = "7",
                            Title = "7",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                        new Size()
                        {
                            Code = "7.5",
                            Title = "7.5",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                        new Size()
                        {
                            Code = "8",
                            Title = "8",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                        new Size()
                        {
                            Code = "8.5",
                            Title = "8.5",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                        new Size()
                        {
                            Code = "9",
                            Title = "9",
                            CategoryId = (int)ProductCategoryEnum.Shoes
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Sizes

                #region Stock Status

                if (!context.StockStatuses.Any())
                {
                    context.StockStatuses.AddRange(new List<StockStatus>()
                    {
                       new StockStatus()
                        {
                            Title = "Critical",
                            Class = "bg-warning"
                        },
                        new StockStatus()
                        {
                            Title = "Out-of-Stock",
                            Class = "bg-danger"
                        },
                        new StockStatus()
                        {
                            Title = "In-Stock",
                            Class = "bg-success"
                        },
                    });

                    context.SaveChanges();
                }

                #endregion Stock Status
            }
        }
    }
}