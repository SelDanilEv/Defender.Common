using MongoDB.Driver;

namespace Defender.Common.Helpers
{
    public static class MongoTransactionHelper
    {
        public static async Task<bool> ExecuteUnderTransactionAsync(
            IClientSessionHandle sessionHandle,
            Func<Task> action)
        {
            using (var session = sessionHandle)
            {
                session.StartTransaction();

                try
                {
                    await action();

                    await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();

                    return false;
                }
            }

            return true;
        }
    }
}
