using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleServiceTracker.Application.Features.Auth.Commands;

namespace VehicleServiceTracker.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            //   Username Kuralları
            RuleFor(x => x.Username)
                .NotEmpty()
                    .WithMessage("Kullanıcı adı zorunludur.")
                .Length(3, 100)
                    .WithMessage("Kullanıcı adı 3-100 karakter arasında olmalıdır.")
                .Must(NotContainDangerousCharacters)
                    .WithMessage("Kullanıcı adı geçersiz karakterler içeriyor.")
                .Matches("^[a-zA-Z0-9_.-]+$")
                    .WithMessage("Kullanıcı adı sadece harf, rakam, tire, alt çizgi ve nokta içerebilir.");

            //   Password Kuralları
            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Şifre zorunludur.")
                .MinimumLength(6)
                    .WithMessage("Şifre en az 6 karakter olmalıdır.")
                .MaximumLength(100)
                    .WithMessage("Şifre maksimum 100 karakter olabilir.");
        }

        private bool NotContainDangerousCharacters(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // SQL Injection, XSS karakterleri
            var dangerousChars = new[] { "'", "\"", ";", "--", "/*", "*/", "<", ">", "&" };
            return !dangerousChars.Any(username.Contains);
        }
    }
}
