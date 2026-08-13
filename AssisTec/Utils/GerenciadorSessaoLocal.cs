using System;

namespace AssisTec
{
    public static class GerenciadorSessaoLocal
    {
        public static void SalvarSessao(int usuarioId, int diasValidade = 30)
        {
            Properties.Settings.Default.UsuarioIdSalvo = usuarioId;
            Properties.Settings.Default.DataExpiracaoSessao = DateTime.Now.AddDays(diasValidade);
            Properties.Settings.Default.Save();
        }

        public static int ObterUsuarioIdValido()
        {
            int usuarioId = Properties.Settings.Default.UsuarioIdSalvo;
            DateTime expiracao = Properties.Settings.Default.DataExpiracaoSessao;

            if (usuarioId > 0 && expiracao > DateTime.Now)
            {
                return usuarioId;
            }

            LimparSessao();
            return 0;
        }

        public static void LimparSessao()
        {
            Properties.Settings.Default.UsuarioIdSalvo = 0;
            Properties.Settings.Default.DataExpiracaoSessao = DateTime.MinValue;
            Properties.Settings.Default.Save();
        }
    }
}