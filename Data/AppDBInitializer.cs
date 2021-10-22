using ECommerce1.Data.Enums;
using ECommerce1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

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

                #region Employees
                if (!context.Employees.Any())
                {
                    context.Employees.AddRange(new List<Employees>()
                    {
                        new Employees()
                        {
                            EmployeeFirstName = "Jhon",
                            EmployeeLastName = "Smith",
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
                            Title = "Men",
                            AlternateTitle = "Male"
                        },
                        new Gender()
                        {
                            Title = "Women",
                            AlternateTitle = "Female"
                        },
                    });

                    context.SaveChanges();
                }
                #endregion Gender

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

                #region Product
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Adidas",
                            Description = "Adidas",
                            ProductCategoryId = (int)ProductCategoryEnum.Clothing,
                            StatusId = (int)StatusEnum.Active,
                            DateCreated = DateTime.Now,
                            GenderId = (int)GenderEnum.Men,
                            Image = "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"
                }
                    }); ;

                    context.SaveChanges();
                }
                #endregion Product

                #region Promos
                if (!context.Promos.Any())
                {
                    context.Promos.AddRange(new List<Promos>()
                    {
                        new Promos()
                        {
                            Name = "25% off",
                            Description = "25% off on all brands",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            SalePercentage = 25,
                            Image = "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"
                }
                    }); ;

                    context.SaveChanges();
                }
                #endregion Promos

                #region Sizes
                if (!context.Sizes.Any())
                {
                    context.Sizes.AddRange(new List<Size>()
                    {
                        new Size()
                        {
                            Code = "XS",
                            Title = "X-Small"
                        },
                        new Size()
                        {
                            Code = "S",
                            Title = "Small"
                        },
                        new Size()
                        {
                            Code = "M",
                            Title = "Medium"
                        },
                        new Size()
                        {
                            Code = "L",
                            Title = "Large"
                        },
                        new Size()
                        {
                            Code = "XL",
                            Title = "X-Large"
                        },
                    });

                    context.SaveChanges();
                }
                #endregion Status

                #region Status
                if (!context.Statuses.Any())
                {
                    context.Statuses.AddRange(new List<Status>()
                    {
                        new Status()
                        {
                            Title = "Active"
                        },
                        new Status()
                        {
                            Title = "Inactive"
                        },
                        new Status()
                        {
                            Title = "Phase-out"
                        },
                    });

                    context.SaveChanges();
                }
                #endregion Status
            }
        }
    }
}
