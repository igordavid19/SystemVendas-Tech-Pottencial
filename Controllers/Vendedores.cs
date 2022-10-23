using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Context;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Controllers
{
  [ApiController]
    [Route("[controller]")]
        public class VendedoresController : ControllerBase
        {
            private readonly OrganizaContext _context;

            public VendedoresController(OrganizaContext context)
            {
                _context = context;
            }

            // Listar todos os Vendedores (GET)

            [HttpGet("ListaVendedores")]

            public IActionResult ObterTodos()
            {
                var vendedores = _context.Vendedores.ToArray();

                return Ok(vendedores);
            }

            // Listar vendedor específico por id (GET)
            [HttpGet("{id}")]
            public IActionResult ObterVendedorPorId(int id)
            {
                var vendedor = _context.Vendedores.Find(id);

                if (vendedor == null)
                {
                    return NotFound();
                }

                return Ok(vendedor);
            }

            // Adicionar novo vendedor (POST)
            [HttpPost("AdicionaVendedores")]
            public IActionResult AdicionarVendedores(Vendedor vendedor, string nome, string cpf, string telefone)
            {
                if(nome==null)
                    return BadRequest("Informar nome do vendedor");
                if(cpf==null)
                    return BadRequest("Informar o CPF do vendedor");
                if(telefone==null)
                    return BadRequest("Informar o telefone do vendedor");
                
                vendedor.Nome=nome;
                vendedor.Cpf=cpf;
                vendedor.Telefone=telefone;

                _context.Vendedores.Add(vendedor);
                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterVendedorPorId), new { id = vendedor.Id }, vendedor);
            }
        }
    
}
