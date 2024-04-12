using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
	public interface IReviewerRepository
	{
		ICollection<Reviewer> GetReviewers();
		Reviewer GetReviewer(int reviewerId);
		ICollection<Review> GetReviewsOfAReviewer(int reviewerId);

		bool ReviewerExists(int reviewerId);
		bool CreateReviewer(Reviewer reviewer);
		bool UpdateReviewer(Reviewer reviewer);
		bool DeleteReviewer(Reviewer reviewer);
		bool SaveReviewer();
	}
}
