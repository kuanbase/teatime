using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTime.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }
        public int Count { get; set; }
        public string? Ice { get; set; }
        public string? Sweetness { get; set; }
        public string? ApplicationUserId { get; set; }
        //[ForeignKey("ApplicationUserId")]
        //[ValidateNever]
        //public ApplicationUser? ApplicationUser { get; set; }

        public ShoppingCart()
        {

        }

        public static bool operator ==(ShoppingCart a, ShoppingCart b)
        {
            if (a.Ice == b.Ice && a.ProductId == b.ProductId && a.Sweetness == b.Sweetness && a.ApplicationUserId == b.ApplicationUserId)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(ShoppingCart a, ShoppingCart b)
        {
            return !(a == b);
        }

        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(this, obj))
        //    {
        //        return true;
        //    }

        //    if (ReferenceEquals(obj, null))
        //    {
        //        return false;
        //    }

        //    throw new NotImplementedException();
        //}

        //public bool Equals(ShoppingCart obj)
        //{
        //    if (obj is null)
        //    {
        //        throw new ArgumentNullException(nameof(obj));
        //    }

        //    return this == obj;
        //}
    }
}
