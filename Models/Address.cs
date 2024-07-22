using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace RunWebAppGroup.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        




    }
}
