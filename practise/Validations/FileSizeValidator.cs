

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace practise.Validations
{
    public class FileSizeValidator:ValidationAttribute
    {
        private readonly int _maxFileSizeInMbps;
        public FileSizeValidator(int maxFileSizeInMbps)
        {
            _maxFileSizeInMbps= maxFileSizeInMbps;
        }

        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            if(value==null)
            {
                return ValidationResult.Success;
            }
            IFormFile formFile = value as IFormFile;
            if(formFile==null)
            {
                return ValidationResult.Success;
            }
            if(formFile.Length>_maxFileSizeInMbps*1024*1024)
            {
                return new ValidationResult($"File size can not be bigger than {_maxFileSizeInMbps} megabytes");
            }

            return ValidationResult.Success;
        }
    }
}
