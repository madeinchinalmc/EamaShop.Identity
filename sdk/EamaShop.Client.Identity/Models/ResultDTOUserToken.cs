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

    public partial class ResultDTOUserToken
    {
        /// <summary>
        /// Initializes a new instance of the ResultDTOUserToken class.
        /// </summary>
        public ResultDTOUserToken() { }

        /// <summary>
        /// Initializes a new instance of the ResultDTOUserToken class.
        /// </summary>
        public ResultDTOUserToken(UserToken data = default(UserToken), string message = default(string))
        {
            Data = data;
            Message = message;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Data")]
        public UserToken Data { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; private set; }

    }
}
