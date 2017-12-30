using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EamaShop.Identity.API.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EamaShop.Identity.API.Controllers
{
    /// <summary>
    /// 图片接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageController : Controller
    {
        private readonly ImagePath _basePath;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public ImageController(IConfiguration configuration, IHostingEnvironment environment)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _basePath = new ImagePath(configuration, environment);

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns>图片相关信息</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTOWrapper<ImageInfoDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResultDTOWrapper<ImageInfoDTO>))]
        [UploadFileAction]
        public async Task<IActionResult> Upload([FromServices]ImagePostDTO parameters)
        {
            var file = parameters.File;
            var extension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString().Replace("-", "") + extension;
            var filePath = Path.Combine(_basePath.BaseDirectory.FullName, fileName);

            using (var stream = new FileStream(filePath, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream, HttpContext.RequestAborted);
                await stream.FlushAsync();
            }

            return Ok(ResultDTOWrapper.Ok(new ImageInfoDTO(_basePath.Transform(fileName))));
        }

        private struct ImagePath
        {
            private readonly string SavedDirectory;
            private readonly string Host;
            public ImagePath(IConfiguration configuration, IHostingEnvironment environment)
            {

                SavedDirectory = configuration[IdentityDefaults.ImageSavedRelativePathKey];
                Host = configuration[IdentityDefaults.ImageSavedServerHostKey];
                var path = Path.Combine(environment.WebRootPath, SavedDirectory);

                BaseDirectory = Directory.CreateDirectory(path);
            }
            /// <summary>
            /// 图片所存储的目录路径
            /// </summary>
            public DirectoryInfo BaseDirectory { get; }
            /// <summary>
            /// 图片的文件名称
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public Uri Transform(string fileName)
            {
                return new Uri(Host+SavedDirectory + fileName);
            }
        }
    }
}