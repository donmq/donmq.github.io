using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class RoleUser
    {
        [Key][Column(Order = 0)]
        public string user_account { get; set; }
        [Key][Column(Order = 0)]
        public string  role_unique { get; set; }

        public string create_by { get; set; }

        public DateTime create_time { get; set; }
    }
}