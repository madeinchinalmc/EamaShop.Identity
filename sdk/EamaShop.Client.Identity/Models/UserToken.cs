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

    public partial class UserToken
    {
        /// <summary>
        /// Initializes a new instance of the UserToken class.
        /// </summary>
        public UserToken() { }

        /// <summary>
        /// Initializes a new instance of the UserToken class.
        /// </summary>
        public UserToken(string token = default(string), DateTime? expiredIn = default(DateTime?))
        {
            Token = token;
            ExpiredIn = expiredIn;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Token")]
        public string Token { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ExpiredIn")]
        public DateTime? ExpiredIn { get; private set; }

    }
}
