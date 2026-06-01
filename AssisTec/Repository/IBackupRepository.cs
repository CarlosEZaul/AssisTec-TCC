namespace AssisTec.Repository
{
    public interface IBackupRepository
    {
        bool gerarBackup(string caminho);
        bool importarBackup(string caminho);
    }
}