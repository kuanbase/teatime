using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace TeaTime.Models;

public class Category
{
    [Key] // 主鍵
    public int Id { get; set; }
    [Required(ErrorMessage ="不能為空")]
    [MaxLength(30)] // 要求名字最長為30個字。
    [DisplayName("類別名稱")]
    public string? Name { get; set; }
    [DisplayName("顯示順序")]
    [Required(ErrorMessage = "不能為空")]
    [Range(1,100, ErrorMessage = "輸入範圍應該在1-100之間")] // 要求值的範圍在[1, 100]。
    public int? DisplayOrder { get; set; }
}
