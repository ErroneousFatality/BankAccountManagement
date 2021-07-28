using Domain.Entities.Users;
using System;

namespace Domain.Entities.ModerationActions
{
    public class ModerationAction
    {
        // Properties
        public Guid Id { get; private set; }
        public DateTime DateTime { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public ModerationActionType Type { get; private set; }
        public string Reason { get; private set; }


        // Constructors

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid type.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Invalid reason.
        /// </exception>
        public ModerationAction(Guid userId, ModerationActionType type, string reason)
        {
            if (!Enum.IsDefined(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type), $"Invalid {nameof(ModerationActionType)} value = {type}.");
            }
            if (string.IsNullOrWhiteSpace(reason) || reason.Length > ReasonMaxLength)
            {
                throw new ArgumentException($"{nameof(reason)} must be specified and not longer than {ReasonMaxLength}.", nameof(reason));
            }
            DateTime = DateTime.Now;
            UserId = userId;
            Type = type;
            Reason = reason;
        }

        private ModerationAction() { }

        // Constants
        public const int ReasonMaxLength = 500;
    }
}