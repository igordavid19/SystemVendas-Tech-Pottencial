using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Context;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Controllers
{
  [ApiController]
    [Route("[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly OrganizaContext _context;

        public VendaController(OrganizaContext context)
        {
            _context = context;
        }


        // Buscar venda por Id (GET)
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var venda = _context.Vendas.Find(id);

            if(venda == null)
                return NotFound();

            return Ok(venda);
        }

        //Inserir nova venda (POST)

        [HttpPost("NovaVenda")]
        public IActionResult RegistrarVenda(Venda venda, int vendedorId, string itens)
        {
            if (venda.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data não pode ser vazia."});

            DateTime data = DateTime.Now;
            data.ToShortDateString();
            venda.VendedorId = vendedorId;
            venda.Status = "Aguardando pagamento";
            venda.StatusDisponíveis = "Pagamento aprovado, Cancelado";
            venda.Data = data;
            venda.Itens = itens;

            
            _context.Vendas.Add(venda);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = venda.Id}, venda);
        }

        // Modificar venda por id (PUT)
        [HttpPut("{id}")]
        public IActionResult AtualizarStatus (int id, string status, Venda venda)
        {
            string[] statusDb=new string[2];

            Venda vendaBd = _context.Vendas.Find(id);

            if(vendaBd.StatusDisponíveis.ToUpper() == "ENVIADO PARA TRANSPORTADORA" || vendaBd.StatusDisponíveis.ToUpper() == "CANCELADO")
            {
                return BadRequest($"O pedido em tela não aceita modoficações no seu status: {venda.Status}");
            }

            statusDb = vendaBd.StatusDisponíveis.Split(",");
            statusDb[0].Trim();
            statusDb[1].Trim();

            if(status.ToUpper() != statusDb[0].ToUpper() && status.ToUpper() != statusDb[1].ToUpper() )
            {
                return BadRequest($"O pedido atual só aceita os status: {statusDb[0]} ou {statusDb[1]}");
            }
                DateTime data=DateTime.Now;

                data.ToShortDateString();
                vendaBd.Status=status;

                if(status.ToUpper() == "PAGAMENTO APROVADO")
                {
                    vendaBd.StatusDisponíveis="Enviado para Transportadora, Cancelado";
                } else if (status.ToUpper() == "ENVIADO PARA TRANSPORTADORA")
                {
                    vendaBd.StatusDisponíveis="Entregue";
                }

                _context.Entry(vendaBd).State= EntityState.Modified;

   
                _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new {id = venda.Id}, venda);
        }
        


    }
}