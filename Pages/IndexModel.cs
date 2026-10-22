using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Entropy.Pages
{
    public class IndexModel : PageModel
    {
        public int CurrentStep { get; set; } = 0;
        
        /// <summary>Мощность русского алфавита</summary>
        public const int RUSSIAN_ALPHABET_SIZE = 33;   // Кириллица + ~3 пропущенные
        
        /// <summary>Мощность английского алфавита с пробелом и знаками пунктуации</summary>
        public const int ENGLISH_ALPHABET_SIZE = 26;    // Только буквы
        
        /// <summary>Мощность ASCII без управляющих символов</summary>
        public const int ASCII_NO_CONTROL_SIZE = 95;    // Символы 0x20-0x7E

        /// <summary>Максимальная длина сообщения</summary>
        public const int MAX_N = 256;

        /// <summary>Выбранный алфавит пользователя</summary>
        public static string SelectedAlphabet { get; set; } = "russian";

        /// <summary>Текущая мощность выбранного алфавита</summary>
        public static int AlphabetSize 
        { 
            get => SelectedAlphabet switch {
                "russian" => RUSSIAN_ALPHABET_SIZE,
                "english" => ENGLISH_ALPHABET_SIZE,
                "ascii" => ASCII_NO_CONTROL_SIZE,
                _ => RUSSIAN_ALPHABET_SIZE
            };
        }

        /// <summary>Выбранные пользователем биты на символ</summary>
        public static int UserKBits { get; set; } = 3;  // По умолчанию безопасное значение

        /// <summary>Выводится ли предупреждение об ошибке недостаточных бит</summary>
        public static bool ShowError => IsInsufficientBits;

        /// <summary>Является ли выбор k_bits допустимым (k_bits ≥ log₂(I))</summary>
        public static bool IsInsufficientBits 
        {
            get => UserKBits < MinimumKBits && MessageLength > 0;  // Проверка только если N заполнено
            set => errorDisabled = !value && MessageLength > 0;     // Запрет ошибки при редактировании
        }

        private static bool errorDisabled = true;  // Чтобы не показывать ошибку при первом заходе

        /// <summary>Минимальное необходимое бит для кодирования полного алфавита</summary>
        public static double MinimumKBits 
        {
            get => Math.Round(Math.Log(AlphabetSize) / Math.Log(2.0), 1);
        }

        /// <summary>Длина сообщения (символы)</summary>
        public static int MessageLength { get; set; } = 7;

        /// <summary>Введённое пользователем значение объема в битах</summary>
        public static int? UserEnteredVolumeBits { get; set; }

        /// <summary>Допустимый диапазон для N</summary>
        public static bool OutOfNRange => MessageLength < 5 || MessageLength > MAX_N;

        /// <summary>Имя выбранного алфавита</summary>
        public static string AlphabetName 
        {
            get => SelectedAlphabet == "russian" ? "Русский" :
                       SelectedAlphabet == "english" ? "Английский" : 
                       "ASCII без управляющих";
        }

        /// <summary>Всего бит в сообщении</summary>
        public static int TotalBits => MessageLength * UserKBits;

        /// <summary>Объем в байтах (округление вверх до полного байта)</summary>
        public static byte ByteCount => (byte)Math.Ceiling(TotalBits / 8.0);

        /// <summary>Является ли объём целое число бит (не нужно округлять)</summary>
        public static bool IsExactBitCount => TotalBits % 8 == 0;

        /// <summary>Смена выбранного алфавита</summary>
        public static void SetAlphabet(string newAlphabet)
        {
            SelectedAlphabet = newAlphabet;
            
            var newSize = newAlphabet switch {
                "russian" => RUSSIAN_ALPHABET_SIZE,
                "english" => ENGLISH_ALPHABET_SIZE,
                "ascii" => ASCII_NO_CONTROL_SIZE,
                _ => RUSSIAN_ALPHABET_SIZE
            };

            // В этом методе используется только чтение свойств, не нужны backing fields
        }

        /// <summary>Рассчитывает объём при изменении N</summary>
        public static void Recalculate()
        {
            // Расчет происходит автоматически в свойствах
        }

        /// <summary>Статические примеры</summary>
        public static readonly List<(int N, int I, string Type)> Examples = new()
        {
            (N: 5, I: 4, Type: "intro"),       // Вводный пример (I=2²=4)
            (N: 8, I: 16, Type: "baseA"),      // Основной пример А  
            (N: 10, I: 32, Type: "baseB")      // Основной пример Б
        };

        public static readonly List<int> Powers = new() { 2, 4, 8, 10, 12, 16, 20, 32, 64, 128 };

        /// <summary>Фиксированные значения для самостоятельной задачи</summary>
        public static readonly int Exercise_N = 12;     
        public static readonly int Exercise_I = 16;      
    }
}
