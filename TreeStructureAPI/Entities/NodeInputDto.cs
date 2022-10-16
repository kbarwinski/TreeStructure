using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class NodeInputDto
    {
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Parent ID is a required field.")]
        public int ParentId { get; set; }
    }
}
