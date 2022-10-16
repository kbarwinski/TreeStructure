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
        private readonly INodeRepository _nodeRepository;
        private readonly ILogger<NodeController> _logger;


        public NodeController(INodeRepository nodeRepository, ILogger<NodeController> logger)
        {
            this._nodeRepository = nodeRepository;
            this._logger = logger;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllNodes()
        {
            var nodesList = await _nodeRepository.GetAllNodes(false);
            if(nodesList == null)
            {
                var message = "Root node not found.";
                _logger.LogWarning(message);
                return NotFound(new { message = message });
            }

            var nodesDto = nodesList.Select(n=> new NodeDto() { 
                Id = n.Id, 
                Name = n.Name, 
                ParentId = n.ParentId }).ToList();

            return Ok(nodesDto);

        }

        [HttpGet("root")]
        public async Task<IActionResult> GetRootNode()
        {
            var rootNode = await _nodeRepository.GetNodeByParentId(null, false);
            if (rootNode == null)
            {
                var message = "Root node not found.";
                _logger.LogWarning(message);
                return NotFound(new { message = message });
            }

            var nodeDto = new NodeDto()
            {
                Id = rootNode.Id,
                ParentId = rootNode.ParentId,
                Name = rootNode.Name,
            };

            return Ok(nodeDto);
        }

        [HttpGet("{nodeId}")]
        public async Task<IActionResult> GetNodeChildrenById(int nodeId)
        {
            var node = await _nodeRepository.GetNodeById(nodeId, false);
            if(node == null)
            {
                var message = $"Node with the ID of {nodeId} does not exist.";
                _logger.LogError(message);
                return BadRequest(new { message = message });
            }

            var children = await _nodeRepository.GetNodeChildrenById(nodeId,false);
            if(children == null)
            {
                var message = "No children are available for this node.";
                _logger.LogInformation(message);
                return NotFound(new { message = message });
            }

            var childrenDto = children.Select(n => new NodeDto()
            {
                Id = n.Id,
                Name = n.Name,
                ParentId = n.ParentId
            }).ToList();

            return Ok(childrenDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNode([FromBody] NodeInputDto nodeDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the NodeInputDto object.");
                return UnprocessableEntity(ModelState);
            }

            var parentNode = await _nodeRepository.GetNodeById(nodeDto.ParentId, false);
            if(parentNode == null)
            {
                var message = $"Parent node with the ID of {nodeDto.ParentId} does not exist.";
                _logger.LogError(message);
                return BadRequest(new { message = message });
            }

            Node node = new Node() { ParentId = nodeDto.ParentId, Name = nodeDto.Name };

            _nodeRepository.CreateNode(node);
            await _nodeRepository.Save();
            _logger.LogInformation($"Node with the ID of {node.Id} created successfully.");

            return NoContent();
        }

        [HttpDelete("{nodeId}")]
        public async Task<IActionResult> DeleteNode(int nodeId)
        {
            var node = await _nodeRepository.GetNodeById(nodeId, false);
            if (node == null)
            {
                var message = $"Node with the ID of {nodeId} does not exist.";
                _logger.LogError(message);
                return BadRequest(new { message = message });
            }

            _nodeRepository.DeleteNode(node);
            await _nodeRepository.Save();
            _logger.LogInformation($"Node with the ID of {node.Id} deleted successfully.");

            return NoContent();
        }

        [HttpPut("{nodeId}")]
        public async Task<IActionResult> UpdateNode(int nodeId, [FromBody] NodeInputDto nodeDto)
        {
            var node = await _nodeRepository.GetNodeById(nodeId, true);
            if (node == null)
            {
                var message = $"Node with the ID of {nodeId} does not exist.";
                _logger.LogError(message);
                return BadRequest(new { message = message });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the NodeInputDto object.");
                return UnprocessableEntity(ModelState);
            }

            var parentNode = await _nodeRepository.GetNodeById(nodeDto.ParentId, false);
            if (parentNode == null)
            {
                var message = $"Parent node with the ID of {nodeDto.ParentId} does not exist.";
                _logger.LogError(message);
                return BadRequest(new { message = message });
            }

            if(nodeDto.ParentId != node.ParentId)
            {
                var nodeChildren = await _nodeRepository.GetNestedNodes(node.Id, false);
                if (nodeChildren.Any(n=>n.Id == parentNode.Id))
                {
                    var message = $"New parent node can't be a child of an updated node.";
                    _logger.LogError(message);
                    return BadRequest(new { message = message });
                }
            }

            node.Name = nodeDto.Name;
            node.ParentId = nodeDto.ParentId;
            await _nodeRepository.Save();
            _logger.LogInformation($"Node with the ID of {node.Id} deleted successfully.");

            return NoContent();

        }
    }
}
