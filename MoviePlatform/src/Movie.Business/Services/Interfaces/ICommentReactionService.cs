namespace Movie.Business.Services.Interfaces
{
    public interface ICommentReactionService
    {
        Task<bool?> Like(int id);
        Task<bool?> Dislike(int id);
    }
}
