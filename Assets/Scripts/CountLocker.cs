#region

using System;
using System.Collections.Generic;

#endregion
/* Description: CountLocker is a simple class that provides a way to lock and unlock a resource using an object as the key.
It uses a List<object> to keep track of the objects that have locked the resource, and a boolean variable "IsLocked" to track the status of the resource.
The class has two main methods: "Lock" and "Unlock", these methods are used to lock and unlock the resource respectively.
The Lock method takes an object as a parameter, which will be used as the key to lock the resource.
If the object is already in the list of lock callers, the resource will not be locked again.
If the resource is already locked, the method will do nothing.When a resource is locked, the "OnLock" method is called which raises the "Locked" event.
The Unlock method also takes an object as a parameter, which will be used as the key to unlock the resource.
If the object is not in the list of lock callers, the resource will not be unlocked.
Once all objects have been unlocked and the list of lock callers is empty, the "IsLocked" variable is set to false and the "OnLockEmpty" method is called which raises the "LockEmpty" event.*/

    //TODO: Figure out what is this
public class CountLocker
{
    private readonly List<object> _lockCallers = new List<object>();
    public bool IsLocked;

    public void Lock(object sender)
    {
        //Check if the sender is already present in the lockCallers list, if yes, return
        if (_lockCallers.Contains(sender)) return;
        //If not, add the sender to the list
        _lockCallers.Add(sender);

        //Check if the lock is already in use, if yes, return
        if (IsLocked) return;
        //If not, set the IsLocked flag to true
        IsLocked = true;
        //Call the OnLock method
        OnLock();
    }

    public void Unlock(object sender)
    {
        //Check if the sender is present in the lockCallers list, if not, return
        if (!_lockCallers.Contains(sender)) return;
        //If yes, remove the sender from the list
        _lockCallers.Remove(sender);

        //Check if the lockCallers list is empty, if not, return
        if (_lockCallers.Count != 0) return;

        //Set the IsLocked flag to false
        IsLocked = false;
        //Call the OnLockEmpty method
        OnLockEmpty();
    }

    //Event to be invoked when the lock is empty
    public event EventHandler LockEmpty;

    //Event to be invoked when the lock is in use
    public event EventHandler Locked;

    private void OnLockEmpty()
    {
        //Invoke the LockEmpty event
        LockEmpty?.Invoke(this, EventArgs.Empty);
    }

    private void OnLock()
    {
        //Invoke the Locked event
        Locked?.Invoke(this, EventArgs.Empty);
    }
}
