using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    [Table("contas_receber")]
    public class ContasReceber : LancamentoFinanceiro
    {
        [Key]
        [Column("id_conta_receber")]
        public int id_conta_receber { get; set; }

        [Column("id_os_fk")]
        public int? id_os_fk { get; set; }

        [ForeignKey("id_os_fk")]
        public OrdemServico OrdemServico { get; set; }

        [NotMapped] public string filtroDataInicio { get; set; } = string.Empty;

        [NotMapped]
        public string filtroDataFim { get; set; } = string.Empty;

        [NotMapped]
        public string filtroDescricao { get; set; } = string.Empty;

        [NotMapped]
        public string filtroStatus { get; set; } = string.Empty;

        [NotMapped]
        public int? filtroIdFormaPagamento { get; set; }

    }
}