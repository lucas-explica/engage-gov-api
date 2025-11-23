using EngageGov.Domain.Entities;
using System.Collections.Generic;

namespace EngageGov.Application.Services
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetCommentsByLawId(string lawId);
        Comment AddComment(Comment comment);
    }
}
