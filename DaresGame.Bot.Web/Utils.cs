﻿using System;
using File = System.IO.File;

namespace DaresGame.Bot.Web
{
    internal static class Utils
    {
        public static void LogException(Exception ex)
        {
            File.AppendAllText(ExceptionsLogPath, $"{ex}{Environment.NewLine}");
        }

        private const string ExceptionsLogPath = "errors.txt";
    }
}
