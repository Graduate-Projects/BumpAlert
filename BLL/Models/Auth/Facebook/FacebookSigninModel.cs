using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.Models
{
    public class FacebookSigninModel
    {
        public string Username { get; set; }
        public Properties Properties { get; set; }
    }
    public class Properties
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
