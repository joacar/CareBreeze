using System.Collections.Generic;

namespace CareBreeze.WebApp.Features
{
    public class Error
    {
        public static readonly Error Generic = new Error
        {
            Key = "GenericError",
            Message = "I'm so sorry, but I couldn't fullfill your request at the moment."
        };

        public string Key { get; set; }

        public string Parameter { get; set; }

        public string Message { get; set; }
    }

    public class EnumerationError : Error
    {
        public IList<string> Values { get; set; }
    }

    public class BaseResult
    {
        public IList<Error> Errors { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        public bool HasError => Errors?.Count > 0;

        public BaseResult()
        {

        }

        public void AddError(params Error[] errors)
        {
            if (Errors == null)
            {
                Errors = new List<Error>(errors);
            }
            else
            {
                foreach (var error in errors)
                {
                    Errors.Add(error);
                }
            }
        }

        public BaseResult(Error error)
        {
            AddError(error);
        }
    }
}
