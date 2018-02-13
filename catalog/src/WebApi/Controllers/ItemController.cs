using Chiron.Catalog.Command.Model.ViewModels;
using Chiron.Catalog.Query;
using Chiron.Catalog.Query.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cortside.Common.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Catalog.WebApi.Controllers {
    [Route("api/[controller]")]
    //[Authorize]
    public class CatalogController : Controller {
        public IQueryDispatcher QueryDispatcher { get; }

        public CatalogController(IQueryDispatcher queryDispatcher) {
            QueryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IEnumerable<SimpleItemModel>> Get() {
            return await QueryDispatcher.Dispatch<GetItemsQuery, IEnumerable<SimpleItemModel>>(new GetItemsQuery());
        }
    }
}
