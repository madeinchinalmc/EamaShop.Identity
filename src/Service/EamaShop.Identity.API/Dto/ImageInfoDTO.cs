using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API.Dto
{
    /// <summary>
    /// 图片信息
    /// </summary>
    public class ImageInfoDTO
    {
        /// <summary>
        /// init
        /// </summary>
        /// <param name="uri"></param>
        public ImageInfoDTO(string uri) : this(new Uri(uri))
        {
        }
        /// <summary>
        /// init
        /// </summary>
        /// <param name="uri"></param>
        public ImageInfoDTO(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            Uri = uri.AbsolutePath;
        }
        /// <summary>
        /// 图片的路径
        /// </summary>
        public string Uri { get; }


    }
}
