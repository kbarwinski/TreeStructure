using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class NodeConfiguration : IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            builder.HasData(
                new Node
                {
                    Id = 1,
                    Name = "Root",
                    Depth = 0,
                },
                new Node
                {
                    Id = 2,
                    Name = "A1",
                    ParentId = 1,
                    Depth = 1,
                },
                new Node
                {
                    Id = 3,
                    Name = "A2",
                    ParentId = 1,
                    Depth = 1,
                },
                new Node
                {
                    Id = 4,
                    Name = "B1",
                    ParentId = 2,
                    Depth = 2,
                },
                new Node
                {
                    Id = 5,
                    Name = "B2",
                    ParentId = 2,
                    Depth = 2,
                },
                new Node
                {
                    Id = 6,
                    Name = "C1",
                    ParentId = 4,
                    Depth = 3,
                },
                new Node
                {
                    Id = 7,
                    Name = "C2",
                    ParentId = 4,
                    Depth = 3,
                },
                new Node
                {
                    Id = 8,
                    Name = "C3",
                    ParentId = 4,
                    Depth = 3,
                }
                );
        }
    }
}
