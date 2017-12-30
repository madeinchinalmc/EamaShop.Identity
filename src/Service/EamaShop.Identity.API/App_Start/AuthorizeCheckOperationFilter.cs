using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace EamaShop.Identity.API
{
    internal class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAuthorize = (context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>().Any() ||
                context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>().Any()) && !context.ApiDescription.ActionAttributes().OfType<AllowAnonymousAttribute>().Any();


            if (hasAuthorize)
            {
                operation.Responses.Add("401", new Response { Description = "用户未登陆，或未提供用户token" });

                operation.Responses.Add("403", new Response { Description = "用户权限不足，无法访问该api" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    { "JsonWebTokenBearer", new [] { context.GetType().Assembly.FullName } }
                });
            }
            if (context.ApiDescription.ActionAttributes().OfType<UploadFileActionAttribute>().Any())
            {
                operation.Consumes.Add("multipart/form-data");
                operation.Parameters = operation.Parameters ?? new List<IParameter>();
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "file",
                    In = "formData",
                    Description = "选择需要上传的文件",
                    Required = true,
                    Type = "file"
                });
            }
        }
    }
}