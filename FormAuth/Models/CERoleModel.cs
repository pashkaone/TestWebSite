using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FormAuth.Models
{
    public class EditRoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class CreateRoleModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }

        public string[] IdsToAdd { get; set; }

        public string[] IdsToDelete { get; set; }
    }
}