using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssisTec
{
    public static class ContatoWhatsApp
    {
        public static async Task<bool> EntrarContato(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            string apenasNumeros = Regex.Replace(telefone, @"[^\d]", "");

            if (apenasNumeros.Length == 11 || apenasNumeros.Length == 10)
            {
                apenasNumeros = "55" + apenasNumeros;
            }

            try
            {
                string urlWhatsApp = $"https://api.whatsapp.com/send?phone={apenasNumeros}";

                await Task.Run(() =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlWhatsApp,
                        UseShellExecute = true
                    });
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}