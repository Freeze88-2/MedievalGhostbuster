/// <summary>
/// Used by Debugger to perform it's action on the target class
/// </summary>
public interface IDebug
{
    /// <summary>
    /// When called performs the action given on the concrete class
    /// </summary>
    /// <param name="active"> If it shown be on or off </param>
    void RunDebug(bool active);
}
