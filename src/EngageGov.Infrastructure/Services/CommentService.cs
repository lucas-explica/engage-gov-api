using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngageGov.Domain.Entities;
using EngageGov.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using EngageGov.Infrastructure.Data;

namespace EngageGov.Infrastructure.Services
{
    public class CommentService : EngageGov.Application.Services.ICommentService
    {
        private readonly EngageGovDbContext _db;
        public CommentService(EngageGovDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Comment> GetCommentsByLawId(string lawId)
        {
            if (!Guid.TryParse(lawId, out var proposalId)) return new List<Comment>();
            return _db.Comments
                .Where(c => c.ProposalId == proposalId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public Comment AddComment(Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return comment;
        }
    }
}
