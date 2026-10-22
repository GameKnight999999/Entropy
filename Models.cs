using System.Collections.Generic;

namespace Entropy.Models;

/// <summary>Конфигурация примера расчёта</summary>
public static class ExampleConfig
{
    public static int InitialAlphabet { get; } = 4;
    public static int MaxPower => 16;

    /// <summary>Формат: V = N · log₂(I)</summary>
    public const string FormulaText = "V = N · log₂(I)";
    
    /// <summary>Список двоичных кодов для символов</summary>
    public static List<string> BinaryCodes => new List<string>(Enumerable.Range(0, 16).Select(i => i.ToString("D4")));

    /// <summary>Вопросы пользователя</summary>
    public static List<(string Question, string Explanation)> Questions => new List<(string, string)>(new[]
    {
        ("Какая формула используется для расчёта информационного объёма?", 
         "Формула: V = N · log₂(I), где I — мощность алфавита."),
        
        ("Почему биты на символ K = log₂(I)?",
         "log₂(4) = 2, log₂(8) = 3… Биты растут по мере увеличения алфавита."),
    });
}