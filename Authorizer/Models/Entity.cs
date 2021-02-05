using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authorizer.Models
{
    public class Entity
    {
        [JsonProperty("violations")]
        private IList<string> Errors { get; set; } = new List<string>();

        public void FailIf(Func<bool> condition, string error)
        {
            if (condition())
                Errors.Add(error);
        }
        public void CleanErrors()
        {
            Errors.Clear();
        }

        public IList<string> GetErrors()
        {
            return Errors;
        }

        [JsonIgnore]
        public bool Valid => !Errors.Any();

    }
}
