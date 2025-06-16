using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Text;

namespace MainService.Application.Helpers;

public static partial class StringHelper
{
    private const string VietnamseCountryCode = "84";

    public static string ConvertCamelCaseToWords(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var builder = new StringBuilder();
        char lastChar = '-';

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 1 && char.IsLower(input[i - 1]) && char.IsLower(lastChar))
                {
                    builder.Append('-');
                }

                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                builder.Append(c);
            }

            lastChar = c;
        }

        return builder.ToString();
    }

    public static bool IsEmail(this string input)
    {
        return !string.IsNullOrWhiteSpace(input) && new EmailAddressAttribute().IsValid(input);
    }

    public static bool IsNullOrEmpty(this string input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static string RemoveSpecialCharacters(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var builder = new StringBuilder();
        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c))
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    public static string StandardizeVietnamesePhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return string.Empty;
        }

        if (phoneNumber.StartsWith(VietnamseCountryCode))
        {
            return phoneNumber;
        }
        else if (phoneNumber.StartsWith('0'))
        {
            return VietnamseCountryCode + phoneNumber[1..];
        }
        else
        {
            return VietnamseCountryCode + phoneNumber;
        }
    }

    public static string? ToNullIfEmpty(this string input)
    {
        return string.IsNullOrEmpty(input) ? null : input;
    }

    public static string ToVietnamesePhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return string.Empty;
        }

        if (phoneNumber.StartsWith(VietnamseCountryCode))
        {
            return "0" + phoneNumber[2..];
        }
        else
        {
            return "0" + phoneNumber[1..];
        }
    }
}