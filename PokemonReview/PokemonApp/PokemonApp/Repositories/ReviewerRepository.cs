using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
	public class ReviewerRepository : IReviewerRepository
	{
		private readonly DataContext _context;

		public ReviewerRepository(DataContext context)
		{
			_context = context;
		}


		public Reviewer GetReviewer(int reviewerId)
		{
			return _context.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
		}

		public ICollection<Reviewer> GetReviewers()
		{
			return _context.Reviewers.OrderBy(r => r.Id).ToList();
		}

		public ICollection<Review> GetReviewsOfAReviewer(int reviewerId)
		{
			return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
		}

		public bool ReviewerExists(int reviewerId)
		{
			return _context.Reviewers.Any(r => r.Id == reviewerId);
		}

		public bool CreateReviewer(Reviewer reviewer)
		{
			_context.Add(reviewer);
			return SaveReviewer();
		}
		public bool SaveReviewer()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateReviewer(Reviewer reviewer)
		{
			_context.Update(reviewer);
			return SaveReviewer();
		}

		public bool DeleteReviewer(Reviewer reviewer)
		{
			_context.Remove(reviewer);
			return SaveReviewer();
		}
	}
}
