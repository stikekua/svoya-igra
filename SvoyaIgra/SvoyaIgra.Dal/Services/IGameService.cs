using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services
{
    public interface IGameService
    {
        /// <summary>
        /// create a new game, returns gameId
        /// </summary>
        /// <returns></returns>
        public Task<Guid> CreateGameAsync();

        /// <summary>
        /// returns randomly 18(GameConstants.RoundTopicsCount) topics 
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TopicDto>> GetTopicsAsync(Guid gameId);

        /// <summary>
        /// returns randomly 7(GameConstants.FinalTopicsCount) topics for final round 
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TopicDto>> GetTopicsFinalAsync(Guid gameId);

        /// <summary>
        /// returns random topic with 1 question for "Cat in a bag", with the exception of topics from game
        /// </summary>
        /// <returns></returns>
        public Task<TopicDto> GetCatQuestionAsync(Guid gameId);

    }
}
