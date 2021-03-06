﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace EamaShop.Client.Identity.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class ResultDTOUserInfoDTO
    {
        /// <summary>
        /// Initializes a new instance of the ResultDTOUserInfoDTO class.
        /// </summary>
        public ResultDTOUserInfoDTO() { }

        /// <summary>
        /// Initializes a new instance of the ResultDTOUserInfoDTO class.
        /// </summary>
        public ResultDTOUserInfoDTO(UserInfoDTO data = default(UserInfoDTO), string message = default(string))
        {
            Data = data;
            Message = message;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Data")]
        public UserInfoDTO Data { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; private set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Data != null)
            {
                this.Data.Validate();
            }
        }
    }
}
