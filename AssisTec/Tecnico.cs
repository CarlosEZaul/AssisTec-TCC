namespace AssisTec
{
    public class Tecnico:Pessoa
    {
        private string Status;

        private string Periodo;
        public string status
        {
            get { return Status; }
            set { Status = value; }
        }

        public string periodo
        {
            get { return Periodo; }
            set { Periodo = value; }
        }
    }
}