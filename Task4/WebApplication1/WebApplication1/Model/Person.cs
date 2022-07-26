﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public partial class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
