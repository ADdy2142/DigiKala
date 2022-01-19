using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectDigiKala.Models.Profile;

namespace ProjectDigiKala.Models
{
    public class Operator : IdentityUser
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public IEnumerable<Address> Addresses { get; set; }

        public string FullName => $"{Name} {Family}";
    }
}
