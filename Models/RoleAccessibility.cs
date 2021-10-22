using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class RoleAccessibility
    {
        [Key]
        public int RoleID { get; set; }
        public int RoleDisplayItem { get; set; }
        public int RoleCreateItem { get; set; }
        public int RoleUpdateItem { get; set; }
        public int RoleDeleteItem { get; set; }
        public int RoleDisplayReports { get; set; }
        public int RoleCreateReports { get; set; }
        public int RoleDeleteReports { get; set; }
        public int RoleUpdateReports { get; set; }
        public int RoleDisplayAccounts { get; set; }
        public int RoleCreateAccounts { get; set; }
        public int RoleDeleteAccounts { get; set; }
        public int RoleBanAccounts { get; set; }
        public int RoleUnbanAccounts { get; set; }
        public int RoleDisplayPromotion { get; set; }
        public int RoleUpdatePromotion { get; set; }
        public int RoleCreatePromotion { get; set; }
        public int RoleDeletePromotion { get; set; }
        public int RoleDisplayOrders { get; set; }
        public int RoleUpdateOrderStatus { get; set; }
        public int RoleDisplayMessages { get; set; }
        public int RoleUpdateMessages { get; set; }
        public int RoleReplyMessages { get; set; }
        public int RoleDeleteMessages { get; set; }
        public int RoleDisplayAnnouncement { get; set; }
        public int RoleCreateAnnouncement { get; set; }
        public int RoleUpdateAnnouncement { get; set; }
        public int RoleDeleteAnnouncement { get; set; }
    }
}
