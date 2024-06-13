using MongoDB.Driver;

namespace Defender.Common.Helpers
{
    public static class MongoTransactionHelper
    {
        public static async Task<(bool, MongoCommandException?)> ExecuteUnderTransactionWithExceptionAsync(
            IClientSessionHandle sessionHandle,
            Func<Task> action)
        {
            using var session = sessionHandle;
            session.StartTransaction();

            try
            {
                await action();

                await session.CommitTransactionAsync();
            }
            catch (Exception ex) when (ex.InnerException is MongoCommandException mongoEx)
            {
                await session.AbortTransactionAsync();

                return (false, mongoEx);
            }
            catch
            {
                await session.AbortTransactionAsync();

                return (false, null);
            }

            return (true, null);
        }

        public static async Task<bool> ExecuteUnderTransactionAsync(
            IClientSessionHandle sessionHandle,
            Func<Task> action)
        {
            using var session = sessionHandle;
            session.StartTransaction();

            try
            {
                await action();

                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();

                return false;
            }

            return true;
        }
    }
}
