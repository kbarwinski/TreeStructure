using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class NodeRepository : RepositoryBase<Node>, INodeRepository
    {
        public NodeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<Node>> GetAllNodes(bool trackChanges)
        {
            var root = await FindAll(false).Where(x => x.ParentId == null).FirstOrDefaultAsync();
            var query = $"SELECT * FROM getnodes({root.Id},{int.MaxValue})";


            return await RunFromQuery(query, trackChanges).Skip(1).ToListAsync();
        }

        public async Task<List<Node>> GetNestedNodes(int nodeId, bool trackChanges)
        {
            var node = await FindAll(false).Where(x => x.Id == nodeId).FirstOrDefaultAsync();
            var query = $"SELECT * FROM getnodes({node.Id},{int.MaxValue})";


            return await RunFromQuery(query, trackChanges).Skip(1).ToListAsync();
        }

        public async Task<List<Node>> GetNodeChildrenById(int nodeId,bool trackChanges)
        {
            var query = $"SELECT * FROM getnodes({nodeId},{1})";

            return await RunFromQuery(query, trackChanges).Skip(1).ToListAsync();
        }

        public async Task<Node> GetNodeById(int nodeId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id == nodeId, trackChanges).FirstOrDefaultAsync();
        }

        public async Task<Node> GetNodeByParentId(int? parentId, bool trackChanges)
        {
            return await FindByCondition(x => x.ParentId == parentId, trackChanges).FirstOrDefaultAsync();
        }

        public void CreateNode(Node node)
        {
            Create(node);
        }

        public void DeleteNode(Node node)
        {
            Delete(node);
        }
    }
}
