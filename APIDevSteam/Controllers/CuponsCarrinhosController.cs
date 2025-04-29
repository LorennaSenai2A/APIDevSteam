using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDevSteam.Data;
using APIDevSteam.Models;

namespace APIDevSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponsCarrinhosController : ControllerBase
    {
        private readonly APIContext _context;

        public CuponsCarrinhosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/CuponsCarrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CupomCarrinho>>> GetCuponsCarrinhos()
        {
            return await _context.CuponsCarrinhos.ToListAsync();
        }

        // GET: api/CuponsCarrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CupomCarrinho>> GetCupomCarrinho(Guid id)
        {
            var cupomCarrinho = await _context.CuponsCarrinhos.FindAsync(id);

            if (cupomCarrinho == null)
            {
                return NotFound();
            }

            return cupomCarrinho;
        }

        // PUT: api/CuponsCarrinhos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupomCarrinho(Guid id, CupomCarrinho cupomCarrinho)
        {
            if (id != cupomCarrinho.CupomCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(cupomCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CupomCarrinhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CuponsCarrinhos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CupomCarrinho>> PostCupomCarrinho(CupomCarrinho cupomCarrinho)
        {
            _context.CuponsCarrinhos.Add(cupomCarrinho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCupomCarrinho", new { id = cupomCarrinho.CupomCarrinhoId }, cupomCarrinho);
        }

        // DELETE: api/CuponsCarrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupomCarrinho(Guid id)
        {
            var cupomCarrinho = await _context.CuponsCarrinhos.FindAsync(id);
            if (cupomCarrinho == null)
            {
                return NotFound();
            }

            _context.CuponsCarrinhos.Remove(cupomCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CupomCarrinhoExists(Guid id)
        {
            return _context.CuponsCarrinhos.Any(e => e.CupomCarrinhoId == id);
        }
        [HttpPost("AplicarCupom")]
        public async Task<IActionResult> AplicarCupom(Guid carrinhoId, Guid cupomId)
        {
            // Verificar se o carrinho existe
            var carrinho = await _context.Carrinhos
                .Include(c => c.ItensCarrinhos)
                .FirstOrDefaultAsync(c => c.CarrinhoId == carrinhoId);

            if (carrinho == null)
                return NotFound("Carrinho não encontrado.");

            if (carrinho.Finalizado == true)
                return BadRequest("O carrinho já foi finalizado.");

            // Verificar se o cupom existe
            var cupom = await _context.Cupons.FirstOrDefaultAsync(c => c.CupomId == cupomId);

            if (cupom == null)
                return NotFound("Cupom não encontrado.");

            // Verificar se o cupom é válido e está ativo
            if (cupom.Ativo == false || (cupom.DataValida.HasValue && cupom.DataValida < DateTime.Now))
                return BadRequest("Cupom inválido ou expirado.");

            // Calcular o total do carrinho
            decimal valorTotal = carrinho.ItemCarrinhos.Sum(item => item.Quantidade * item.ValorUnitario);

            // Calcular o desconto
            decimal desconto = (valorTotal * cupom.Desconto) / 100;

            // Atualizar o valor total do carrinho
            carrinho.ValorTotal = valorTotal - desconto;

            // Registrar a aplicação do cupom no CupomCarrinho
            var cupomCarrinho = new CupomCarrinho
            {
                CarrinhoId = carrinho.CarrinhoId,
                CupomId = cupom.CupomId,
                DataAplicacao = DateTime.Now
            };
            _context.CuponsCarrinhos.Add(cupomCarrinho);

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Retornar os valores calculados
            return Ok(new
            {
                Mensagem = "Cupom aplicado com sucesso!",
                ValorOriginal = valorTotal,
                Desconto = desconto,
                ValorFinal = carrinho.ValorTotal
            });
        }
    }
}
