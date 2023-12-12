using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursova.Contracts.SupportDirections
{
    public record SupportDirectionResponse(
        int Id,
        string Name,
        string Description,
        string About);
}
