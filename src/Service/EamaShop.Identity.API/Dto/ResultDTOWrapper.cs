using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API.Dto
{
    /// <summary>
    /// 接口返回对象的基础结构
    /// </summary>
    public class ResultDTOWrapper
    {
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="message">Success Message Or ErrorMessage</param>
        protected ResultDTOWrapper(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
        /// <summary>
        /// 返回的结果描述信息，如果状态码为错误，则为错误信息
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// 指定信息提示
        /// </summary>
        /// <returns></returns>
        public static ResultDTOWrapper New(string message="SUCCESS")
        {
            return new ResultDTOWrapper(message);
        }
        /// <summary>
        /// 输入的参数有误
        /// </summary>
        /// <returns></returns>
        public static ResultDTOWrapper Error(ModelStateDictionary modelState)
        {
            return new ResultDTOWrapper(string.Join("#", modelState.Select(kvp => $"{kvp.Key}——{kvp.Value}")));
        }
        /// <summary>
        /// 成功，包含数据
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultDTOWrapper Ok<TDTO>(TDTO data)
        {
            return new ResultDTOWrapper<TDTO>(data);
        }
    }
    /// <summary>
    /// 接口返回对象基础结构
    /// </summary>
    public class ResultDTOWrapper<TDTO> : ResultDTOWrapper
    {
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="data"></param>
        public ResultDTOWrapper(TDTO data) : this(data, "SUCCESS")
        {
        }
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public ResultDTOWrapper(TDTO data, string message) : base(message)
        {
            Data = data;
        }
        /// <summary>
        /// 数据 Can be null
        /// </summary>
        public TDTO Data { get; }
    }
}
