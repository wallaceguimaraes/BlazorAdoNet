using System.ComponentModel.DataAnnotations;

namespace WebApi.Validations
{
    public class Required : RequiredAttribute
    {
        public Required()
        {
            ErrorMessage = "{0}: Required.";
        }
    }
}