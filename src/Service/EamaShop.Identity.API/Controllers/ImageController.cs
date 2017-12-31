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
    [Route("api/Image")]
    public class ImageController : Controller
    {
        /// <summary>
        /// 图片的目录
        /// </summary>
        public string DirectoryName => "images";
        /// <summary>
        /// 当前的应用程序环境
        /// </summary>
        public IHostingEnvironment Environment { get; }
        /// <summary>
        /// init
        /// </summary>
        /// <param name="environment"></param>
        public ImageController(IHostingEnvironment environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
        /// <summary>
        /// 获取图片 非API型接口
        /// </summary>
        /// <param name="name">图片的名称 Eg.  1241asdjaoidn12od.jpg</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [Produces("image/jpg")]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "*" })]
        public IActionResult GetPicture(string name)
        {
            if (!name.IsPicture())
            {
                ModelState.TryAddModelError("Message", "只能获取图片");
                return BadRequest(ResultDTOWrapper.Error(ModelState));
            }
            var extension = Path.GetExtension(name);
            try
            {
                var file = new FileStream(Path.Combine(Environment.WebRootPath, DirectoryName, name), FileMode.Open);

                return File(file, $"image/{extension.Replace(".", "")}");
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns>图片相关信息</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResultDTOWrapper<ImageInfoDTO>))]
        [ProducesResponseType(400, Type = typeof(ResultDTOWrapper<ImageInfoDTO>))]
        [UploadFileAction]
        [Produces("application/json")]
        public async Task<IActionResult> Upload([FromServices]ImagePostDTO parameters)
        {
            var fileName = Guid.NewGuid().ToString().Replace("-", "").ToUpper() + Path.GetExtension(parameters.File.FileName);

            var path = Path.Combine(Environment.WebRootPath, DirectoryName, fileName);
            using (var file = new FileStream(path, FileMode.Create))
            {
                await parameters.File.CopyToAsync(file);
                await file.FlushAsync();
            }
            return Ok(ResultDTOWrapper.Ok(new ImageInfoDTO(Request.Host + "/" + "api/Image/" + fileName)));
        }
        
    }
}