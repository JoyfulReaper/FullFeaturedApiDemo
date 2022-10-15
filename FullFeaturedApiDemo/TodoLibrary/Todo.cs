using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary;
public class Todo
{
    public int TodoId { get; set; }
    public string Task { get; set; } = default!;
    public string OwnerId { get; set; } = default!;
    public bool IsComplete { get; set; }
}
