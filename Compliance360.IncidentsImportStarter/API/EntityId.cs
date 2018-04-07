using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Compliance360.IncidentsImportStarter.API
{
    public class EntityId
    {
        /// <summary>
        /// This is a conversion operator that lets you case a string
        /// to an EntityId which is used in JSON deserialization.
        /// </summary>
        /// <param name="val">The token value.</param>
        public static explicit operator EntityId (string val) {
            EntityId ent = new EntityId(val);
            return ent;
        }

        /// <summary>
        /// The string id token value.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Token { get; set; }

        /// <summary>
        /// Returns the integer id of the token.
        /// </summary>
        [JsonIgnore]
        public int Id
        {
            get
            {
                var idx = Token.IndexOf(":");
                var idString = Token.Substring(idx + 1);
                int idValue;
                if (int.TryParse(idString, out idValue))
                {
                    return idValue;
                }

                return 0;
            }
        }

        /// <summary>
        /// Initializes a new EntityId
        /// </summary>
        /// <param name="token">Entity Id token.</param>
        public EntityId(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            Token = token;
        }
    }
}
