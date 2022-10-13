using Contracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TreeStructureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private readonly INodeRepository nodeRepository;
        private readonly ILogger<NodeController> logger;


        public NodeController(INodeRepository nodeRepository, ILogger<NodeController> logger)
        {
            this.nodeRepository = nodeRepository;
            this.logger = logger;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllNodes([FromQuery] bool ascSort)
        {
            var expandedNode = await nodeRepository.GetAllNodes(ascSort, false);

            return Ok(expandedNode);

        }

        [HttpGet("root")]
        public async Task<IActionResult> GetRootNode()
        {
            var root = await nodeRepository.GetNodeByParentId(null, false);
            return Ok(root);
        }

        [HttpGet("{nodeId}")]
        public async Task<IActionResult> GetNodeChildrenById(int nodeId, [FromQuery] bool ascSort)
        {
            var children = await nodeRepository.GetNodeChildrenById(nodeId,ascSort,false);
            return Ok(children);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNode([FromBody] NodeInputDto nodeDto)
        {
            Node node = new Node() { ParentId = nodeDto.ParentId, Name = nodeDto.Name };
            nodeRepository.CreateNode(node);
            await nodeRepository.Save();
            return NoContent();
        }

        [HttpDelete("{nodeId}")]
        public async Task<IActionResult> DeleteNode(int nodeId)
        {
            var node = await nodeRepository.GetNodeById(nodeId, false);
            nodeRepository.DeleteNode(node);

            await nodeRepository.Save();
            return NoContent();
        }

        [HttpPut("{nodeId}")]
        public async Task<IActionResult> UpdateNode(int nodeId, [FromBody] NodeInputDto nodeDto)
        {
            var nodeToUpdate = await nodeRepository.GetNodeById(nodeId, true);

            nodeToUpdate.Name = nodeDto.Name;
            nodeToUpdate.ParentId = nodeDto.ParentId;

            await nodeRepository.Save();
            return NoContent();

        }
    }
}
