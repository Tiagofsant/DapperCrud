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
        [HttpGet]   //GET METHOD

        // SELECT ALL OF THE PERSON TABLE - QUERY ASYNC
        public async Task<ActionResult<List<PessoaController>>> GetAllPessoas()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var pessoas = await connection.QueryAsync<PessoaController>("select * from pessoas");
            return Ok(pessoas);
        }

        //SELECT ALL OF THE DEFINED TABLE WITH ID - QUERY FIRST

        [HttpGet("{idPessoa}")]
        public async Task<ActionResult<PessoaController>> GetPessoas(int idPessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var pessoa = await connection.QueryFirstAsync<PessoaController>("select * from pessoas where id = @Id", new { Id = idPessoa });
            return Ok(pessoa);
        }




        [HttpPost]   //POST METHOD

        // CREATE A NEW PERSON
        public async Task<ActionResult<List<PessoaController>>> CreatePessoas(PessoaController pessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into pessoa (nomePessoa, sobrenomenomePessoa, emailPessoa, enderecoPessoa, telefonePessoa, nascimentoPessoa) " +
                "value (@nomePessoa, @sobrenomePessoa, @emailPessoa, @enderecoPessoa, @telefonePessoa, @nascimentoPessoa)", pessoa);
            return Ok(await SelectAllPessoas(connection));
        }


        [HttpPut]   //UPDATE METHOD

        // CREATE A NEW PERSON
        public async Task<ActionResult<List<PessoaController>>> UpdatePessoas(PessoaController pessoa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update pessoa set nomePessoa = @nomePessoa, sobrenomenomePessoa = @sobrenomePesso, emailPessoa = @emailPessoa, enderecoPessoa = @enderecoPessoa," +
                " , telefonePessoa = @telefonePessoa, nascimentoPessoa = @nascimentoPessoa", pessoa);
            return Ok(await SelectAllPessoas(connection));
        }



















        //I CAUGHT ABOVE SO FAR COMMENT AND I PUT IT HERE
        private static async Task<IEnumerable<PessoaController>> SelectAllPessoas(SqlConnection connection)
        {
            return await connection.QueryAsync<PessoaController>("select * from pessoa");
        }
    }
}
