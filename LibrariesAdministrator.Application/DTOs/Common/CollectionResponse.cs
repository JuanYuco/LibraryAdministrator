using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrariesAdministrator.Application.DTOs.Common
{
    public class CollectionResponse<TEntity> : ResponseBase
    {
        public ICollection<TEntity> EntityCollection { get; set; }
    }
}
