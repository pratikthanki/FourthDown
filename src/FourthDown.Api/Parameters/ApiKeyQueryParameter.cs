using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FourthDown.Api.Parameters
{
    public class ApiKeyQueryParameter
    {
        [BindRequired] public string Name { get; set; }

        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();

            if (Name == null || string.IsNullOrWhiteSpace(Name))
                errors["queryParameter"] = new[] {"The Name field must be provided."};

            return errors;
        }
    }
}