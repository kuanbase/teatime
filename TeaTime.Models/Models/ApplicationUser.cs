using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTime.Models.Models;

internal class ApplicationUser: IdentityUser
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required DateTime Date { get; set; }
    [Required]
    public required string Address { get; set; }
}
