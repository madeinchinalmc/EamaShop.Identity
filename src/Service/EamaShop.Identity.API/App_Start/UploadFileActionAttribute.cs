using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API
{
    /// <summary>
    /// 标记控制器的Action可以接受图片
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UploadFileActionAttribute : Attribute
    {
    }
}
