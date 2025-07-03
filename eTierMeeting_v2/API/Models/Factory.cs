using System.ComponentModel.DataAnnotations;

namespace eTierV2_API.Models
{
    public class Factory
    {
        [Key]
        public string factory_id {get;set;}
        public string factory_name {get;set;}
        public string customer_name {get;set;}
        public string location {get;set;}
    }
}