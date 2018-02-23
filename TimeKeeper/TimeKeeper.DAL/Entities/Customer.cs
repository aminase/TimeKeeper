﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public enum CustomerStatus
    {
        Client = 1,
        Prospect
    }

    public class Customer : BaseClass<int>
    {
        public Customer()
        {
            Address = new Address();
            Projects = new List<Project>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Image { get; set; }
        [MaxLength(3)]
        public string Monogram { get; set; }
        [Required]
        [MaxLength(50)]
        public string Contact { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }
        public Address Address { get; set; }
        [Required]
        public CustomerStatus Status { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
