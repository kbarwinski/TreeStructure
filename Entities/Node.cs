using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Node
    {
        [Column("NodeId")]
        public int Id { get; set; }

        [Required(ErrorMessage="Name is a required field.")]
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Node? ParentNode { get; set; }
        public ICollection<Node> ChildrenNodes { get; set; }
        public int Depth { get; set; } = 0;
    }
}
