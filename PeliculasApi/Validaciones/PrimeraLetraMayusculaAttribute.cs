﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()![0].ToString();
            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe de ser en mayuscula");
            }
            return ValidationResult.Success;
        }
    }
}