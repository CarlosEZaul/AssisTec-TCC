using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssisTec.UserControls
{
    public partial class ucBackupImportar : UserControl
    {
        private readonly string _connectionString;
        private readonly string _diretorioMysql;
        private CancellationTokenSource _cts;

        public ucBackupImportar()
        {
            InitializeComponent();

            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SuaStringConexao"]?.ConnectionString ?? "SERVER=localhost;DATABASE=assistec;UID=root;PWD=;PORT=3306;";

            _diretorioMysql = @"C:\xampp\mysql\bin";

            DesignModerno();
        }

        private void DesignModerno()
        {
            BackColor = Color.FromArgb(39, 55, 76);
        }
        
        private async void btnBackup_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter   = "Arquivo de Backup (*.bak)|*.bak";
                sfd.Title    = "Definir Local do Backup";
                sfd.FileName = $"Backup_Assistec_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                string senha = ObterSenha("Digite uma senha para proteger o backup:");
                if (senha == null)
                {
                    return;
                }

                ControleInterface(false, "Gerando backup...");
                _cts = new CancellationTokenSource();

                try
                {
                    string destino   = sfd.FileName;
                    string caminhoDump = Path.Combine(_diretorioMysql, "mysqldump.exe");

                    await Task.Run(() =>
                        BackupMysql.ExecutarBackup(
                            _connectionString, caminhoDump, destino, senha));

                    MessageBox.Show(
                        "Backup gerado e criptografado com sucesso!",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string destino = sfd.FileName;
                    if (File.Exists(destino))
                        TentarDeletarArquivo(destino);

                    MessageBox.Show(
                        "Falha ao gerar backup:\n" + ex.Message,
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _cts?.Dispose();
                    ControleInterface(true);
                }
            }
        }

        private async void btnImportar_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Arquivo de Backup (*.bak)|*.bak";
                ofd.Title  = "Selecionar Arquivo de Backup";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                string senha = ObterSenha("Digite a senha do backup para restaurar:");
                if (senha == null) return;
                
                var confirmacao = MessageBox.Show("ATENÇÃO: Esta operação substituirá todos os dados atuais do banco.\nDeseja continuar?", "Confirmar Restauração", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacao != DialogResult.Yes) return;

                ControleInterface(false, "Restaurando backup...");
                _cts = new CancellationTokenSource();

                try
                {
                    string origem      = ofd.FileName;
                    string caminhoMysql = Path.Combine(_diretorioMysql, "mysql.exe");

                    await Task.Run(() =>
                        BackupMysql.ExecutarImportacao(
                            _connectionString, caminhoMysql, origem, senha));

                    MessageBox.Show(
                        "Backup restaurado com sucesso!",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Security.Cryptography.CryptographicException)
                {
                    MessageBox.Show(
                        "Senha incorreta ou arquivo corrompido.",
                        "Erro de Criptografia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Falha ao restaurar backup:\n" + ex.Message,
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _cts?.Dispose();
                    ControleInterface(true);
                }
            }
        }
        
        private string ObterSenha(string mensagem)
        {
            using (var form = new PasswordForm_(mensagem))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return null;

                if (string.IsNullOrWhiteSpace(form.Senha))
                {
                    MessageBox.Show(
                        "A senha é obrigatória.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                return form.Senha;
            }
        }

        private void ControleInterface(bool ativo, string labelStatus = null)
        {
            btnBackup.Enabled   = ativo;
            btnImportar.Enabled = ativo;
            Cursor = ativo ? Cursors.Default : Cursors.WaitCursor;

            
        }

        private static void TentarDeletarArquivo(string caminho)
        {
            try { File.Delete(caminho); }
            catch { /* silencioso — melhor deixar o arquivo do que explodir aqui */ }
        }

        private void btnBackup_MouseEnter(object sender, EventArgs e)
            => btnBackup.BackColor = Color.FromArgb(50, 70, 95);

        private void btnBackup_MouseLeave(object sender, EventArgs e)
            => btnBackup.BackColor = SystemColors.Control;
    }
}