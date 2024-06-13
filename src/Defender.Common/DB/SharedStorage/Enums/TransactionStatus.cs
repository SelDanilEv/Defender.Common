namespace Defender.Common.DB.SharedStorage.Enums;

public enum TransactionStatus
{
    Queued,
    Failed,
    Proceed,
    QueuedForRevert,
    Reverted,
    Canceled,
}
