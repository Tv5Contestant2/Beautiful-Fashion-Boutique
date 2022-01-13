using ECommerce1.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ECommerce1.ViewModel
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Role { get; set; }

        public IEnumerable<Modules> Modules { get; set; }
        public IEnumerable<IdentityRole> RoleList { get; set; }

        #region Pagination

        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(RoleList.Count() / (double)ItemPerPage));
        }

        public IEnumerable<IdentityRole> PaginatedList()
        {
            int start = (CurrentPage - 1) * ItemPerPage;
            return RoleList.OrderBy(b => b.Id).Skip(start).Take(ItemPerPage);
        }

        #endregion Pagination
    }
}