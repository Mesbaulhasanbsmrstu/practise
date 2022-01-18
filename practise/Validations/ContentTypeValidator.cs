using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace practise.Validations
{
    public class ContentTypeValidator:ValidationAttribute
    {
        private readonly string[] _validContentType;
        private readonly string[] _imageContentType=new string[] {"image/jpeg","image/png", "image/gif"};
        public ContentTypeValidator(string[] validContentType)
        {
            _validContentType = validContentType;
        }

        public ContentTypeValidator(ContentTypeGroup contentTypeGroup)
        {
            switch(contentTypeGroup)
            {
                case ContentTypeGroup.Image:
                    _validContentType = _imageContentType;
                    break;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            IFormFile formFile = value as IFormFile;
            if (formFile == null)
            {
                return ValidationResult.Success;
            }
            if(!_validContentType.Contains(formFile.ContentType))
            {
                return new ValidationResult($"Content-Type Should be One of the following {string.Join(",", _validContentType)}");
            }


            return ValidationResult.Success;
        }
    }
    public enum ContentTypeGroup
    {
        Image
    }
}
