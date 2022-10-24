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
        /// returns randomly 18 topics
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TopicDto>> GetTopicsAsync(Guid gameId);
        
        /// <summary>
        /// returns randomly 7 topics for final round
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TopicDto>> GetTopicsFinalAsync(Guid gameId);

        /// <summary>
        /// returns random question for "Cat in a bag"
        /// </summary>
        /// <returns></returns>
        public Task<QuestionDto> GetCatQuestionAsync(Guid gameId);

    }
}
