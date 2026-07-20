using System;
using System.Linq;
using AssisTec.Repository;

namespace AssisTec.DTO
{
    public class LucroMesDTO
    {
        AppDbContext _context = new AppDbContext();
        public (decimal TotalRecebido, decimal TotalPago, decimal LucroLiquido) ObterLucroDoMes(int mes, int ano)
        {
            var inicioMes = new DateTime(ano, mes, 1);
            var fimMes = inicioMes.AddMonths(1);

            decimal totalRecebido = _context.ContasReceber
                .Where(c => c.status == "PAGA" 
                            && c.data_pagamento >= inicioMes 
                            && c.data_pagamento < fimMes)
                .Sum(c => (decimal?)c.valor) ?? 0m;

            decimal totalPago = _context.Contas_Pagar
                .Where(c => c.status == "PAGA" 
                            && c.data_pagamento >= inicioMes 
                            && c.data_pagamento < fimMes)
                .Sum(c => (decimal?)c.valor) ?? 0m;

            decimal lucroLiquido = totalRecebido - totalPago;

            return (totalRecebido, totalPago, lucroLiquido);
        }
    }
}