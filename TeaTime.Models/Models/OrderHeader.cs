using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTime.Models;

public class OrderHeader
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public DateTime OrderDate { get; set; }
    public double OrderTotal { get; set; }
    public string? OrderState { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? PaymentDueTime { get; set; }
    public string? SessionId { get; set; }
    [Required]
    public required string PhoneNumber { get; set; }
    [Required]
    public required string Address { get; set; }
    [Required]
    public required string Name { get; set; }
}
