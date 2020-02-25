using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebTransactions.Model
{
    public class Estado
    {
        public bool Finalizado { get; set; }

        public ObjectResult ObjectResult { get; set; }
    }
}
