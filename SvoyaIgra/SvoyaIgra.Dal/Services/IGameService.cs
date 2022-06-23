using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services
{
    public interface IGameService
    {
        /// <summary>
        /// returns randomly 18 topics (6 per each round)
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TopicDto>> GetTopicsAsync();
        
        /// <summary>
        /// returns randomly 7 topics for final round
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TopicDto>> GetTopicsFinalAsync();
    }
}
