using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EamaShop.Identity.API
{
    /// <summary>
    /// Provides extension methods for <see cref="ModelStateDictionary"/>
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        static ObjectPool<StringBuilder> StringBuilders = new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());
        /// <summary>
        /// Parse <see cref="ModelStateDictionary"/> to response string.
        /// </summary>
        /// <param name="modelStateDictionary"></param>
        /// <returns></returns>
        public static string ToResponseString(this ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }
            var sb = StringBuilders.Get();
            try
            {
                foreach (var md in modelStateDictionary)
                {
                    sb.Append(md.Key);
                    foreach (var er in md.Value.Errors)
                    {
                        sb.Append(er.ErrorMessage);
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                    sb.Append("#");
                }
                return sb.ToString();
            }
            finally
            {
                StringBuilders.Return(sb);
            }

        }
    }
}
