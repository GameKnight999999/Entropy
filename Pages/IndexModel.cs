using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Entropy.Pages;

/// <summary>Описание готового алфавита из списка.</summary>
public sealed record AlphabetInfo(string Key, string Name, int Size);

/// <summary>Модель страницы: интерактивный калькулятор объёма информации.</summary>
public class IndexModel : PageModel
{
    // Границы по ТЗ
    public const int MinN = 5;
    public const int MaxN = 256;
    public const int MinKBits = 1;
    public const int MaxKBits = 8;

    /// <summary>Готовые алфавиты из списка (мощности — по ТЗ).</summary>
    public static readonly IReadOnlyList<AlphabetInfo> Alphabets = new List<AlphabetInfo>
    {
        new("russian", "Русский", 33),
        new("english", "Английский", 26),
        new("ascii", "ASCII без управляющих", 95)
    };

    /// <summary>Выбранный алфавит.</summary>
    [BindProperty]
    public string SelectedAlphabet { get; set; } = "russian";

    /// <summary>Сколько бит на символ выбрал пользователь.</summary>
    [BindProperty]
    public int UserKBits { get; set; } = 6;

    /// <summary>Длина сообщения N (символы).</summary>
    [BindProperty]
    public int MessageLength { get; set; } = 7;

    /// <summary>Ответ пользователя: объём сообщения в байтах (необязательно).</summary>
    [BindProperty]
    public int? UserVolumeBytes { get; set; }

    public AlphabetInfo CurrentAlphabet =>
        Alphabets.FirstOrDefault(a => a.Key == SelectedAlphabet) ?? Alphabets[0];

    public string AlphabetName => CurrentAlphabet.Name;
    public int AlphabetSize => CurrentAlphabet.Size;

    /// <summary>Точное значение log₂(I).</summary>
    public double ExactBitsPerChar => Math.Log2(AlphabetSize);

    /// <summary>log₂(I) для отображения (2 знака, точка как разделитель).</summary>
    public string ExactBitsPerCharText =>
        ExactBitsPerChar.ToString("0.##", CultureInfo.InvariantCulture);

    /// <summary>Минимальное целое число бит, достаточное для кодирования алфавита.</summary>
    public int RequiredKBits => (int)Math.Ceiling(ExactBitsPerChar);

    public bool IsKBitsInRange => UserKBits >= MinKBits && UserKBits <= MaxKBits;

    /// <summary>Недостаточно бит: k_bits &lt; log₂(I).</summary>
    public bool IsKBitsInsufficient => UserKBits < ExactBitsPerChar;

    public bool IsNValid => MessageLength >= MinN && MessageLength <= MaxN;

    /// <summary>Можно ли показывать результат.</summary>
    public bool HasResult => IsNValid && IsKBitsInRange && !IsKBitsInsufficient;

    /// <summary>Итоговый объём в битах: V = N × k_bits.</summary>
    public int TotalBits => MessageLength * UserKBits;

    /// <summary>Итоговый объём в байтах: ceil(V / 8), целочисленно.</summary>
    public int TotalBytes => (TotalBits + 7) / 8;

    /// <summary>Делится ли число бит на 8 нацело.</summary>
    public bool IsExactBytes => TotalBits % 8 == 0;

    /// <summary>Сколько групп бит показываем в визуализации (ограничение для больших N).</summary>
    public int GroupsToShow => Math.Min(MessageLength, 16);

    public bool HasVolumeAnswer => UserVolumeBytes.HasValue;

    public bool IsVolumeAnswerCorrect => UserVolumeBytes == TotalBytes;

    public string BitsWord(int n) => Plural(n, "бит", "бита", "бит");
    public string CharsWord(int n) => Plural(n, "символ", "символа", "символов");
    public string BytesWord(int n) => Plural(n, "байт", "байта", "байт");

    public void OnGet()
    {
        // Значения по умолчанию уже заданы в свойствах.
    }

    public void OnPost()
    {
        // Защита от подмены значения алфавита в форме.
        if (string.IsNullOrWhiteSpace(SelectedAlphabet) ||
            !Alphabets.Any(a => a.Key == SelectedAlphabet))
        {
            SelectedAlphabet = "russian";
        }
    }

    /// <summary>Русская форма слова для числа: 1 символ, 2 символа, 5 символов.</summary>
    private static string Plural(int n, string one, string few, string many)
    {
        var abs = Math.Abs(n) % 100;
        if (abs is >= 11 and <= 19) return many;

        return (abs % 10) switch
        {
            1 => one,
            2 or 3 or 4 => few,
            _ => many
        };
    }
}
