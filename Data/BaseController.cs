using Microsoft.AspNetCore.Mvc;

namespace PokemonTrainerAPI.Data
{
    public abstract class BaseController : ControllerBase
    {
        protected AppDbContext db { get; }

        public BaseController(AppDbContext context)
        {
            db = context;
        }
    }

}
