using Entities;

namespace Contracts
{
    public interface INodeRepository
    {
        Task<List<Node>> GetAllNodes(bool ascSort, bool trackChanges);
        Task<List<Node>> GetNodeChildrenById(int nodeId, bool ascSort, bool trackChanges);
        Task<Node> GetNodeById(int nodeId, bool trackChanges);
        Task<Node> GetNodeByParentId(int? parentId, bool trackChanges);
        void CreateNode(Node node);
        void DeleteNode(Node node);
        Task<int> Save();
    }
}