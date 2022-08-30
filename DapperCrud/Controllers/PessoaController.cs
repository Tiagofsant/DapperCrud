using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IConfiguration _config;
        public PessoaController(IConfiguration config)
        {
            _config = config;
        }

        // TESTED - ALL OK
        [HttpGet]   //GET METHOD

        // SELECT ALL OF THE PERSON TABLE - QUERY ASYNC
        public async Task<ActionResult<List<Pessoa>>> SelectAllPessoas()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var pessoas = await connection.QueryAsync<Pessoa>("select * from CadPessoa");
            return Ok(pessoas);
        }


        // TESTED - ALL OK
        //SELECT ALL OF THE DEFINED TABLE WITH ID - QUERY FIRST

        [HttpGet("{idPessoa}")]
        public async Task<ActionResult<Pessoa>> SelectAllPessoas(int idPessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var pessoa = await connection.QueryFirstAsync<Pessoa>("select * from CadPessoa where idPessoa = @Id", new { Id = idPessoa });
            return Ok(pessoa);
        }


        // TESTED - ALL OK

        [HttpPost]   //POST METHOD

        // CREATE A NEW PERSON
        public async Task<ActionResult<List<Pessoa>>> CriaPessoas(Pessoa pessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into CadPessoa (idPessoa, nomePessoa, sobrenomePessoa, emailPessoa, enderecoPessoa, telefonePessoa, nascimentoPessoa) values (@idPessoa, @nomePessoa, @sobrenomePessoa, @emailPessoa, @enderecoPessoa, @telefonePessoa, @nascimentoPessoa)", pessoa);
            return Ok(await SelectAllPessoas(connection));
        }

       
        // TESTED - ALL OK

        [HttpPut("{idPessoa}")]   //UPDATE METHOD

        // UPDATE A PERSON
        public async Task<ActionResult<List<Pessoa>>> AtualizaPessoas(Pessoa pessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update CadPessoa set idPessoa = @idPessoa, nomePessoa = @nomePessoa, sobrenomePessoa = @sobrenomePessoa, emailPessoa = @emailPessoa, enderecoPessoa = @enderecoPessoa," +
                "telefonePessoa = @telefonePessoa, nascimentoPessoa = @nascimentoPessoa", pessoa);
            return Ok(await SelectAllPessoas(connection));
        }

        // TESTED - ALL OK
        [HttpDelete("{idPessoa}")]   //DELETE METHOD

        // DELETE A PERSON
        public async Task<ActionResult<List<Pessoa>>> DeletaPessoas(int idPessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from CadPessoa where idPessoa = @Id", new { Id = idPessoa });
            return Ok(await SelectAllPessoas(connection));
        }

        //I CAUGHT ABOVE SO FAR COMMENT AND I PUT IT HERE
        private static async Task<IEnumerable<Pessoa>> SelectAllPessoas(SqlConnection connection)
        {
            return await connection.QueryAsync<Pessoa>("select * from CadPessoa");
        }
    }
}
