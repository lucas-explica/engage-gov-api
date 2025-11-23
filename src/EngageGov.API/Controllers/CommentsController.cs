using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using EngageGov.Domain.Entities;
using EngageGov.Application.Services;

namespace EngageGov.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentsController(ICommentService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public ActionResult<List<Comment>> Get([FromQuery] string lawId)
        {
            var comments = _service.GetCommentsByLawId(lawId);
            return Ok(comments);
        }

        [HttpPost]
        public ActionResult<Comment> Post([FromBody] CommentInput input)
        {
            if (!Guid.TryParse(input.LawId, out var proposalId) || !Guid.TryParse(input.CitizenId, out var citizenId) || string.IsNullOrWhiteSpace(input.Content))
            {
                return BadRequest("Dados inválidos para comentário.");
            }
            var comment = new Comment(proposalId, citizenId, input.Content, EngageGov.Domain.Enums.CommentSentiment.Neutral);
            var result = _service.AddComment(comment);
            return Ok(result);
        }
    }

    public class CommentInput
    {
        public string? LawId { get; set; }
        public string? CitizenId { get; set; }
        public string? Content { get; set; }
    }
}
    public class CommentsController : ControllerBase
    {
        private readonly EngageGov.Application.Services.ICommentService _service;

        public CommentsController(EngageGov.Application.Services.ICommentService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public ActionResult<List<Comment>> Get([FromQuery] string lawId)
        {
            var comments = _service.GetCommentsByLawId(lawId);
            return Ok(comments);
        }

        [HttpPost]
        public ActionResult<Comment> Post([FromBody] CommentInput input)
        {
            if (!Guid.TryParse(input.LawId, out var proposalId) || !Guid.TryParse(input.CitizenId, out var citizenId) || string.IsNullOrWhiteSpace(input.Content))
            {
                return BadRequest("Dados inválidos para comentário.");
            }
            var comment = new Comment(proposalId, citizenId, input.Content, EngageGov.Domain.Enums.CommentSentiment.Neutral);
            var result = _service.AddComment(comment);
            return Ok(result);
        }
    }

    public class CommentInput
    {
        public string? LawId { get; set; }
        public string? CitizenId { get; set; }
        public string? Content { get; set; }
    }