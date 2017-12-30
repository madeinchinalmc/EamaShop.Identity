using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API.Dto
{
    /// <summary>
    /// 上传图片接口参数
    /// </summary>
    public class ImagePostDTO
    {
        /// <summary>
        /// init by service container.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ImagePostDTO(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }
            var httpContext = httpContextAccessor.HttpContext;

            var form = httpContext?.Request?.HasFormContentType == true
                ? httpContext.Request.Form
                : null;

            if (form == null)
            {
                var message = "错误的请求，上传图片应该在Request Body里添加你要上传的图片";

                throw new DomainException(message);
            }
            if (form.Files == null || !form.Files.Any())
            {
                throw new DomainException("无法获取到上传的图片");
            }
            if (form.Files.Count > 1)
            {
                throw new DomainException("一次最多只能上传一张图片");
            }
            var file = form.Files[0];
            if (!file.IsPicture())
            {
                throw new DomainException("图片格式错误 可使用此正则匹配上传的文件格式 '.+(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.BMP|.bmp|.PNG|.png)$'");
            }
            if (file.Length > 1024 * 1024 * 5)
            {
                throw new DomainException("图片的大小应该小于5M");
            }

            File = form.Files[0];
        }
        /// <summary>
        /// 获取上传的文件 只支持单次上传一张图片
        /// </summary>
        public IFormFile File { get; }
    }
}
