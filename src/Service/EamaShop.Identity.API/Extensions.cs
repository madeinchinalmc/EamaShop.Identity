using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// ex
    /// </summary>
    public static class Extensions
    {
        public static long GetId(this ClaimsPrincipal principal)
        {
            return long.Parse(principal.FindFirstValue("Id"));
        }
        /// <summary>
        /// 上传的图片是否是图片格式
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static bool IsPicture(this IFormFile formFile)
        {
            if (formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }
            return Regex.IsMatch(formFile.FileName, ".+(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.BMP|.bmp|.PNG|.png)$", RegexOptions.Compiled);
        }
    }
}
