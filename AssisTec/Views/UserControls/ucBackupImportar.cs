using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Repository;
using Microsoft.VisualBasic;

namespace AssisTec.UserControls
{
    public partial class ucBackupImportar : UserControl
    {
        private readonly BackupRepository _repo;

        public ucBackupImportar()
        {
            InitializeComponent();
            
            string stringConexao = System.Configuration.ConfigurationManager.ConnectionStrings["SuaStringConexao"]?.ConnectionString 
                ?? "Server=localhost;Database=seu_banco;Uid=root;Pwd=;";
                
            _repo = new BackupRepository(stringConexao);
            DesignModerno();
        }

        private void DesignModerno()
        {
            this.Text = "Backup e Importar";
            this.BackColor = Color.FromArgb(39, 55, 76);
        }

        private async void btnBackup_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo de Backup (*.bak)|*.bak";
                sfd.Title = "Definir Local do Backup";
                sfd.FileName = string.Format("Backup_Assistec_{0}", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string senha = Interaction.InputBox("Digite uma senha para o backup (Aviso: o texto ficará visível):", "Criptografia", "");

                    if (string.IsNullOrWhiteSpace(senha))
                    {
                        MessageBox.Show("Operação cancelada. A senha é obrigatória para gerar o backup.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ControleInterface(false);

                    try
                    {
                        string destino = sfd.FileName;
                        await Task.Run(() => _repo.CriarBackup(destino, senha));
                        
                        MessageBox.Show("Backup gerado e criptografado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Falha ao gerar backup: " + ex.Message, "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ControleInterface(true);
                    }
                }
            }
        }

        private async void btnImportar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Arquivo de Backup (*.bak)|*.bak";
                ofd.Title = "Selecionar Arquivo de Backup Criptografado";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string senha = Interaction.InputBox("Digite a senha do backup para descriptografar:", "Descriptografia", "");

                    if (string.IsNullOrWhiteSpace(senha))
                    {
                        MessageBox.Show("Operação cancelada. A senha é obrigatória para restaurar o backup.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ControleInterface(false);

                    try
                    {
                        string origem = ofd.FileName;
                        await Task.Run(() => _repo.ImportarBackup(origem, senha));

                        MessageBox.Show("Backup restaurado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (System.Security.Cryptography.CryptographicException)
                    {
                        MessageBox.Show("Senha incorreta ou arquivo corrompido.", "Erro de Criptografia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Falha ao importar backup: " + ex.Message, "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ControleInterface(true);
                    }
                }
            }
        }

        private void ControleInterface(bool ativo)
        {
            btnBackup.Enabled = ativo;
            btnImportar.Enabled = ativo;
            
            this.Cursor = ativo ? Cursors.Default : Cursors.WaitCursor;
        }

        private void btnBackup_MouseEnter(object sender, EventArgs e)
        {
            btnBackup.BackColor = Color.FromArgb(50, 70, 95);
        }

        private void btnBackup_MouseLeave(object sender, EventArgs e)
        {
            btnBackup.BackColor = SystemColors.Control;
        }
    }
}