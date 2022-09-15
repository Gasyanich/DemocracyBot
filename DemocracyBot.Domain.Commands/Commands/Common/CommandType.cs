namespace DemocracyBot.Domain.Commands.Commands.Common
{
    public enum CommandType
    {
        /// <summary>
        /// Обычная команда, по типу start/stop
        /// </summary>
        Common,

        /// <summary>
        /// Интерактивная команда, исполнение в несколько шагов
        /// </summary>
        /// <example>
        /// /change_name
        /// -как тебя называть?
        /// -*new_name*
        /// -done, *new_name*
        /// </example>
        Interactive
    }
}