using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTime.Utility
{
    public class Global
    {
        // 定義一個Sizes常量用於指定數組多少 
        public IEnumerable<SelectListItem> Sizes = new List<string>()
        {
            "大杯",
            "中杯",
            "小杯"
        }.Select(x => new SelectListItem
        {
            Text = x,
            Value = x,
        });
    }
}
