using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VehicleServiceTracker.Application.Features.ServiceEntries.Commands;

namespace VehicleServiceTracker.Application.Validators
{
    public class CreateServiceEntryCommandValidator : AbstractValidator<CreateServiceEntryCommand>
    {
        public CreateServiceEntryCommandValidator()
        {
            // Command'in Data property'sini validate et
            RuleFor(x => x.Data)
                .NotNull()
                .WithMessage("Servis girişi verisi zorunludur.")
                .SetValidator(new CreateServiceEntryDtoValidator());
        }
    }

    public class CreateServiceEntryDtoValidator : AbstractValidator<CreateServiceEntryDto>
    {
        public CreateServiceEntryDtoValidator()
        {
            // ✅ Araç Plakası Kuralları
            RuleFor(x => x.LicensePlate)
            .NotEmpty()
                .WithMessage("Araç plakası zorunludur.")
            .Length(7, 10) // Boşluksuz: 34ABC123 (8 karakter), boşluklu: 34 ABC 123 (10)
                .WithMessage("Araç plakası 7-10 karakter arasında olmalıdır.")
            .Must(BeValidTurkishLicensePlate)
                .WithMessage("Geçerli bir Türkiye plaka formatı giriniz. Örnek: 34ABC123, 06XY1234, 34A12345");


            // ✅ Marka Kuralları
            RuleFor(x => x.BrandName)
                .NotEmpty()
                    .WithMessage("Marka adı zorunludur.")
                .MaximumLength(100)
                    .WithMessage("Marka adı maksimum 100 karakter olabilir.")
                .Must(NotContainSpecialCharacters)
                    .WithMessage("Marka adı özel karakter içeremez.");

            // ✅ Model Kuralları
            RuleFor(x => x.ModelName)
                .NotEmpty()
                    .WithMessage("Model adı zorunludur.")
                .MaximumLength(100)
                    .WithMessage("Model adı maksimum 100 karakter olabilir.")
                .Must(NotContainSpecialCharacters)
                    .WithMessage("Model adı özel karakter içeremez.");

            // ✅ Kilometre Kuralları
            RuleFor(x => x.Kilometers)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Kilometre bilgisi negatif olamaz.")
                .LessThan(10000000)
                    .WithMessage("Kilometre bilgisi çok yüksek. Lütfen kontrol ediniz.");

            // ✅ Model Yılı Kuralları (Opsiyonel)
            When(x => x.ModelYear.HasValue, () =>
            {
                RuleFor(x => x.ModelYear!.Value)
                    .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
                        .WithMessage($"Model yılı 1900 ile {DateTime.UtcNow.Year + 1} arasında olmalıdır.");
            });

            // ✅ Servis Tarihi Kuralları
            RuleFor(x => x.ServiceDate)
                .NotEmpty()
                    .WithMessage("Servise geliş tarihi zorunludur.")
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                    .WithMessage("Servise geliş tarihi gelecek bir tarih olamaz.")
                .GreaterThan(new DateTime(2000, 1, 1))
                    .WithMessage("Servise geliş tarihi çok eski.");

            // ✅ Şehir Kuralları (Opsiyonel)
            When(x => !string.IsNullOrWhiteSpace(x.ServiceCity), () =>
            {
                RuleFor(x => x.ServiceCity)
                    .MaximumLength(100)
                        .WithMessage("Şehir adı maksimum 100 karakter olabilir.")
                    .Must(BeValidCityName)
                        .WithMessage("Geçerli bir Türkiye şehri giriniz.");
            });

            // ✅ Servis Notu Kuralları (Opsiyonel)
            When(x => !string.IsNullOrWhiteSpace(x.ServiceNote), () =>
            {
                RuleFor(x => x.ServiceNote)
                    .MaximumLength(1000)
                        .WithMessage("Servis notu maksimum 1000 karakter olabilir.");
            });

            // ✅ Kombine Kurallar - Business Logic
            RuleFor(x => x)
                .Must(HaveReasonableKilometersForAge)
                    .WithMessage("Kilometre bilgisi araç yaşına göre yüksek görünüyor. Lütfen kontrol ediniz.")
                    .When(x => x.ModelYear.HasValue);
        }

        // ✅ Custom Validators

        private bool NotContainSpecialCharacters(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            // Sadece harf, rakam ve boşluk, tire
            return value.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-');
        }

        private bool BeValidCityName(string? cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return true;

            var turkishCities = new[]
            {
            "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya",
            "Ardahan", "Artvin", "Aydın", "Balıkesir", "Bartın", "Batman", "Bayburt", "Bilecik",
            "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum",
            "Denizli", "Diyarbakır", "Düzce", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir",
            "Gaziantep", "Giresun", "Gümüşhane", "Hakkâri", "Hatay", "Iğdır", "Isparta", "İstanbul",
            "İzmir", "Kahramanmaraş", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kilis",
            "Kırıkkale", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa",
            "Mardin", "Mersin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize",
            "Sakarya", "Samsun", "Şanlıurfa", "Siirt", "Sinop", "Sivas", "Şırnak", "Tekirdağ",
            "Tokat", "Trabzon", "Tunceli", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak"
        };

            return turkishCities.Contains(cityName.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        private bool HaveReasonableKilometersForAge(CreateServiceEntryDto dto)
        {
            if (!dto.ModelYear.HasValue)
                return true;

            var vehicleAge = DateTime.UtcNow.Year - dto.ModelYear.Value;

            if (vehicleAge < 0)
                return false; // Gelecek yıl olamaz

            var expectedMaxKm = vehicleAge * 60000; // Yılda maksimum 60k km

            return dto.Kilometers <= expectedMaxKm;
        }

        private bool BeValidTurkishLicensePlate(string? licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return false;

            // Boşlukları kaldır
            var cleanPlate = licensePlate.Replace(" ", "").ToUpperInvariant();

            // Minimum ve maksimum uzunluk kontrolü
            if (cleanPlate.Length < 7 || cleanPlate.Length > 9)
                return false;

            // Türkiye plaka regex pattern'leri
            var patterns = new[]
            {
            // Standart yeni format: 34ABC123, 34AB1234, 34A12345
            @"^[0-9]{2}[A-Z]{1,3}[0-9]{1,4}$",
            
            // Eski format da kabul edilebilir: 34A1234
            @"^[0-9]{2}[A-Z]{1}[0-9]{4}$"
        };

            // En az bir pattern'e uymalı
            var isValidFormat = patterns.Any(pattern => Regex.IsMatch(cleanPlate, pattern));

            if (!isValidFormat)
                return false;

            // İl plaka kodu kontrolü (01-81 arası)
            var provinceCode = cleanPlate.Substring(0, 2);
            var provinceNumber = int.Parse(provinceCode);

            if (provinceNumber < 1 || provinceNumber > 81)
                return false;

            // Yasaklı harf kombinasyonları (Türkiye'de kullanılmayan)
            var letterPart = new string(cleanPlate.Skip(2).TakeWhile(char.IsLetter).ToArray());
            var forbiddenLetters = new[] { "Q", "W", "X" }; // Türkçe'de olmayan harfler

            if (forbiddenLetters.Any(forbidden => letterPart.Contains(forbidden)))
                return false;

            return true;
        }
    }
}
