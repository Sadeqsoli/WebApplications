using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
	public class ReviewRepository : IReviewRepository
	{
		readonly DataContext _context;

		public ReviewRepository(DataContext context)
		{
			_context = context;
		}


		public Review GetReview(int reviewId)
		{
			return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
		}

		public ICollection<Review> GetReviews()
		{
			return _context.Reviews.OrderBy(r => r.Id).ToList();

		}

		public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
		{
			return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
		}

		public bool ReviewExists(int reviewId)
		{
			return _context.Reviews.Any(r => r.Id == reviewId);
		}

		public bool CreateReview(Review review)
		{
			_context.Add(review);
			return SaveReview();
		}
		public bool SaveReview()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateReview(Review review)
		{
			_context.Update(review);
			return SaveReview();
		}
		public bool DeleteReview(Review review)
		{
			_context.Remove(review);
			return SaveReview();
		}

		public bool DeleteReviews(List<Review> reviews)
		{
			_context.RemoveRange(reviews);
			return SaveReview();
		}
	}
}
